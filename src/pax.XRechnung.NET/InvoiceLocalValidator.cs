using System.Globalization;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Saxon.Api;

namespace pax.XRechnung.NET;

internal static class InvoiceLocalValidator
{
    private const string CustomizationId = "urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0";
    private const string ResourcePrefix = "pax.XRechnung.NET.Resources.Schematrons.XRechnung-2026-08-31.";
    private static readonly XNamespace Svrl = "http://purl.oclc.org/dsdl/svrl";
    private static readonly Lazy<CompiledRules> Rules = new(CompileRules);

    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "The validation API reports parser, schema, and third-party engine failures through Error.")]
    internal static InvoiceValidationResult Validate(string xml)
    {
        try
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(xml);
            using var reader = CreateReader(XmlInvoiceValidator.GetRawXmlText(xml));
            var document = new XmlDocument { XmlResolver = null };
            document.Load(reader);
            var root = document.DocumentElement;
            if (root?.LocalName != "Invoice" || root.NamespaceURI != XmlInvoiceWriter.InvoiceSchema ||
                root["CustomizationID", XmlInvoiceWriter.CommonBasicComponents]?.InnerText != CustomizationId)
            {
                return new InvoiceValidationResult
                {
                    Error = "Local validation supports only standard XRechnung 3.0 UBL Invoice documents."
                };
            }

            var messages = new List<ValidationMessage>();
            document.Schemas = XmlInvoiceWriter.GetSchemaSet();
            document.Validate((_, e) => messages.Add(new ValidationMessage(e)));
            if (messages.Any(m => m.Severity == XmlSeverityType.Error))
            {
                return new InvoiceValidationResult(messages);
            }

            var rules = Rules.Value;
            var source = rules.Processor.NewDocumentBuilder().Build(document);
            foreach (var executable in rules.Executables)
            {
                var transformer = executable.Load();
                transformer.InitialContextNode = source;
                using var output = new StringWriter(CultureInfo.InvariantCulture);
                transformer.Run(rules.Processor.NewSerializer(output));
                using var reportReader = CreateReader(output.ToString());
                var report = XDocument.Load(reportReader);
                foreach (var finding in report.Descendants().Where(e =>
                    e.Name == Svrl + "failed-assert" || e.Name == Svrl + "successful-report"))
                {
                    var code = (string?)finding.Attribute("id");
                    var level = GetSeverity(code, (string?)finding.Attribute("flag") ?? (string?)finding.Attribute("role"));
                    var text = finding.Element(Svrl + "text")?.Value.Trim() ?? string.Empty;
                    messages.Add(new ValidationMessage(new XmlSchemaException(text), text,
                        level == "error" ? XmlSeverityType.Error : XmlSeverityType.Warning)
                    {
                        Code = code,
                        Path = (string?)finding.Attribute("location"),
                        RuleSeverity = level
                    });
                }
            }

            var result = new InvoiceValidationResult(messages);
            result.Conformity = "XRechnung 3.0.2; KoSIT configuration 2026-08-31; EN16931 1.3.16; XRechnung Schematron 2.6.0";
            result.Evaluation = result.IsValid ? "The document conforms to the supported XRechnung profile." : "The document contains validation errors.";
            return result;
        }
        catch (Exception ex)
        {
            return new InvoiceValidationResult { Error = ex.Message, IsValid = false };
        }
    }

    private static string GetSeverity(string? code, string? flag) => code switch
    {
        "BR-CL-21" or "BR-CL-23" => "warning",
        "UBL-CR-646" => "error",
        _ => flag?.ToUpperInvariant() switch
        {
            "WARNING" => "warning",
            "INFORMATION" or "INFO" => "information",
            _ => "error"
        }
    };

    private static XmlReader CreateReader(string text) => XmlReader.Create(new StringReader(text),
        new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null }, "urn:pax:xrechnung:invoice");

    private static CompiledRules CompileRules()
    {
        var processor = new Processor();
        processor.SetProperty("http://saxon.sf.net/feature/allow-external-functions", "false");
        processor.SetProperty("http://saxon.sf.net/feature/allowedProtocols", "");
        processor.SetResourceResolver(request => throw new XmlException($"External resource access is disabled: {request.Uri}"), false);
        var compiler = processor.NewXsltCompiler();
        var executables = new List<XsltExecutable>();
        foreach (var name in new[] { "EN16931-UBL-validation.xsl", "XRechnung-UBL-validation.xsl" })
        {
            using var stream = typeof(InvoiceLocalValidator).Assembly.GetManifestResourceStream(ResourcePrefix + name)
                ?? throw new InvalidOperationException($"Missing validation resource: {name}");
            using var reader = XmlReader.Create(stream,
                new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null }, "urn:pax:xrechnung:" + name);
            executables.Add(compiler.Compile(reader));
        }
        return new CompiledRules(processor, executables.ToArray());
    }

    private sealed record CompiledRules(Processor Processor, XsltExecutable[] Executables);
}
