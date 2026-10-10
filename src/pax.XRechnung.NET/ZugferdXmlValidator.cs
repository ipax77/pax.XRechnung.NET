using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Saxon.Api;

namespace pax.XRechnung.NET;

/// <summary>Local XML validation for ZUGFeRD 2.3.3 / Factur-X 1.07.3 EN16931 (Comfort).</summary>
public static class ZugferdXmlValidator
{
    private const string ResourcePrefix = "pax.XRechnung.NET.Resources.Zugferd.2.3.3.";
    private const string SchemaDirectory = "Schema/3. Factur-X_1.07.3_EN16931/";
    private const string StylesheetDirectory = SchemaDirectory + "_XSLT_EN16931/";
    private const string ResourceUri = "https://pax.invalid/zugferd/2.3.3/";
    private const string GuidelineId = "urn:cen.eu:en16931:2017";
    private const string Conformity = "ZUGFeRD 2.3.3; Factur-X 1.07.3; EN16931 (Comfort)";
    private static readonly XNamespace Rsm = "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100";
    private static readonly XNamespace Ram = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100";
    private static readonly XNamespace Svrl = "http://purl.oclc.org/dsdl/svrl";
    private static readonly Lazy<XmlSchemaSet> Schemas = new(CompileSchemas);
    private static readonly Lazy<CompiledRules> Rules = new(CompileRules);

    /// <summary>Checks the XML schema and pinned profile business rules without network access.</summary>
    /// <param name="xmlText">The exact CII XML to validate.</param>
    /// <returns>Findings with original rule IDs, locations, and severity. Warnings do not invalidate XML.</returns>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Parser, schema, and engine failures are reported through the result-based API.")]
    public static InvoiceValidationResult Validate(string xmlText)
    {
        try
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(xmlText);
            using var reader = CreateReader(XmlInvoiceValidator.GetRawXmlText(xmlText), ResourceUri + "invoice.xml");
            var document = new XmlDocument { XmlResolver = null };
            document.Load(reader);
            var root = document.DocumentElement;
            if (root?.LocalName != "CrossIndustryInvoice" || root.NamespaceURI != Rsm.NamespaceName ||
                root["ExchangedDocumentContext", Rsm.NamespaceName]?["GuidelineSpecifiedDocumentContextParameter", Ram.NamespaceName]?
                    ["ID", Ram.NamespaceName]?.InnerText != GuidelineId)
            {
                return new InvoiceValidationResult
                {
                    Error = "Local ZUGFeRD validation supports only CII EN16931 (Comfort) with guideline ID " + GuidelineId + "."
                };
            }

            var messages = new List<ValidationMessage>();
            document.Schemas = Schemas.Value;
            document.Validate((_, e) => messages.Add(new ValidationMessage(e)));
            if (!messages.Any(m => m.Severity == XmlSeverityType.Error))
            {
                var rules = Rules.Value;
                var transformer = rules.Executable.Load();
                transformer.InitialContextNode = rules.Processor.NewDocumentBuilder().Build(document);
                using var output = new StringWriter(CultureInfo.InvariantCulture);
                transformer.Run(rules.Processor.NewSerializer(output));
                using var reportReader = CreateReader(output.ToString(), ResourceUri + "report.xml");
                var report = XDocument.Load(reportReader);
                foreach (var finding in report.Descendants().Where(e =>
                    e.Name == Svrl + "failed-assert" || e.Name == Svrl + "successful-report"))
                {
                    var level = GetSeverity(finding);
                    var text = finding.Element(Svrl + "text")?.Value.Trim() ?? string.Empty;
                    messages.Add(new ValidationMessage(new XmlSchemaException(text), text,
                        level == "error" ? XmlSeverityType.Error : XmlSeverityType.Warning)
                    {
                        Code = (string?)finding.Attribute("id"),
                        Path = (string?)finding.Attribute("location"),
                        RuleSeverity = level
                    });
                }
            }

            var result = new InvoiceValidationResult(messages) { Conformity = Conformity };
            result.Evaluation = result.IsValid ? "The XML conforms to the supported ZUGFeRD profile." : "The XML contains validation errors.";
            return result;
        }
        catch (Exception ex)
        {
            return new InvoiceValidationResult { Error = ex.Message, IsValid = false };
        }
    }

    private static string GetSeverity(XElement finding) =>
        ((string?)finding.Attribute("flag") ?? (string?)finding.Attribute("role"))?.ToUpperInvariant() switch
        {
            "WARNING" => "warning",
            "INFORMATION" or "INFO" => "information",
            "ERROR" or "FATAL" => "error",
            _ => finding.Name == Svrl + "failed-assert" ? "error" : "warning"
        };

    private static XmlReader CreateReader(string text, string uri) => XmlReader.Create(new StringReader(text),
        new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null }, uri);

    private static Stream OpenResource(string name) => typeof(ZugferdXmlValidator).Assembly.GetManifestResourceStream(ResourcePrefix + name)
        ?? throw new InvalidOperationException($"Missing ZUGFeRD validation resource: {name}");

    private static XmlSchemaSet CompileSchemas()
    {
        var schemas = new XmlSchemaSet { XmlResolver = null };
        // Dependencies are added before their importers; schemaLocation never accesses a file or URL.
        foreach (var suffix in new[]
        {
            "_urn_un_unece_uncefact_data_standard_UnqualifiedDataType_100",
            "_urn_un_unece_uncefact_data_standard_QualifiedDataType_100",
            "_urn_un_unece_uncefact_data_standard_ReusableAggregateBusinessInformationEntity_100", ""
        })
        {
            var name = SchemaDirectory + "Factur-X_1.07.3_EN16931" + suffix + ".xsd";
            using var stream = OpenResource(name);
            using var reader = XmlReader.Create(stream,
                new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null }, ResourceUri + name);
            schemas.Add(null, reader);
        }
        schemas.Compile();
        return schemas;
    }

    private static CompiledRules CompileRules()
    {
        var processor = new Processor();
        processor.SetProperty("http://saxon.sf.net/feature/allow-external-functions", "false");
        processor.SetProperty("http://saxon.sf.net/feature/allowedProtocols", "");
        const string codeDbName = StylesheetDirectory + "FACTUR-X_EN16931_codedb.xml";
        using var codesStream = OpenResource(codeDbName);
        using var codesReader = XmlReader.Create(codesStream,
            new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null }, ResourceUri + codeDbName);
        var codes = processor.NewDocumentBuilder().Build(codesReader);
        processor.SetResourceResolver(request => request.Uri == new Uri(ResourceUri + codeDbName)
            ? new NodeResource(codes)
            : throw new XmlException($"External resource access is disabled: {request.Uri}"), false);
        const string name = StylesheetDirectory + "FACTUR-X_EN16931.xslt";
        using var stream = OpenResource(name);
        using var reader = XmlReader.Create(stream,
            new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null }, ResourceUri + name);
        return new CompiledRules(processor, processor.NewXsltCompiler().Compile(reader));
    }

    private sealed record CompiledRules(Processor Processor, XsltExecutable Executable);
}
