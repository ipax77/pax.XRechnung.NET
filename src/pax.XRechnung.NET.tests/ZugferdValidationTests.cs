using System.Diagnostics.Tracing;
using System.Reflection;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Xml.Schema;

namespace pax.XRechnung.NET.tests;

[TestClass]
public sealed class ZugferdValidationTests
{
    private static readonly XNamespace Ram = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100";

    [TestMethod]
    public void EmbeddedArtifactsMatchOfficialHashesAndIncludeEverySchemaDependency()
    {
        const string prefix = "pax.XRechnung.NET.Resources.Zugferd.2.3.3.Schema/3. Factur-X_1.07.3_EN16931/";
        var expected = new Dictionary<string, string>
        {
            ["Factur-X_1.07.3_EN16931.xsd"] = "d71afa234624a8ee03ec876caa757702806c68e2da5f77f67ae1df91964fc485",
            ["Factur-X_1.07.3_EN16931_urn_un_unece_uncefact_data_standard_QualifiedDataType_100.xsd"] = "5a3ce756cfa8d4f2ff3165d68123cfb7fbf3c7b64664edb0de38cca64d5c413b",
            ["Factur-X_1.07.3_EN16931_urn_un_unece_uncefact_data_standard_ReusableAggregateBusinessInformationEntity_100.xsd"] = "0c024357d6e5f96a0699cbb425d21bb8db4c65450ad9c265c30a8aad50c46e46",
            ["Factur-X_1.07.3_EN16931_urn_un_unece_uncefact_data_standard_UnqualifiedDataType_100.xsd"] = "9590182879865144dfc6ae069ef7499defaa7d31cb62e30ccacf9146ace5306d",
            ["_XSLT_EN16931/FACTUR-X_EN16931.xslt"] = "af0473fbae13ca9d46c7c70a5dc2ef95985544e779d9be8a7fc84c2555864cea",
            ["_XSLT_EN16931/FACTUR-X_EN16931_codedb.xml"] = "33d04741e2970478d2e1129854b8960aef972e16e0352dd16108d35f94ad147a"
        };
        var assembly = typeof(ZugferdXmlValidator).Assembly;
        CollectionAssert.AreEquivalent(expected.Keys.Select(n => prefix + n).ToArray(),
            assembly.GetManifestResourceNames().Where(n => n.StartsWith(prefix, StringComparison.Ordinal)).ToArray());
        foreach (var (name, hash) in expected)
        {
            using var stream = assembly.GetManifestResourceStream(prefix + name)!;
            Assert.AreEqual(hash, Convert.ToHexString(SHA256.HashData(stream)).ToLowerInvariant(), name);
            if (!name.EndsWith(".xsd", StringComparison.Ordinal)) continue;
            stream.Position = 0;
            XNamespace xs = "http://www.w3.org/2001/XMLSchema";
            var schema = XDocument.Load(stream);
            foreach (var dependency in schema.Root!.Elements().Where(e =>
                e.Name == xs + "import" || e.Name == xs + "include" || e.Name == xs + "redefine"))
            {
                var location = (string?)dependency.Attribute("schemaLocation");
                if (location is not null) Assert.IsTrue(expected.ContainsKey(location), $"Missing dependency: {location}");
            }
        }
    }

    private static XDocument Invoice()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("zugferd-invoice.xml")!;
        return XDocument.Load(stream);
    }

    private static void AssertValid(InvoiceValidationResult result) => Assert.IsTrue(result.IsValid,
        result.Error ?? string.Join("\n", result.Validations.Select(m => $"{m.Code}: {m.Message}")));

    [TestMethod]
    public void ValidFixtureAndRepeatedCallsRequireNoHttp()
    {
        var xml = Invoice().ToString();
        for (var i = 0; i < 2; i++)
        {
            var result = ZugferdXmlValidator.Validate(xml);
            AssertValid(result);
            Assert.AreEqual(string.Empty, result.HttpStatusCode);
            Assert.Contains("2.3.3", result.Conformity!);
        }
    }

    [TestMethod]
    public void IncorrectTotalHasRuleCodeAndLocation()
    {
        var invoice = Invoice();
        invoice.Descendants(Ram + "DuePayableAmount").Single().Value = "1.00";
        var result = ZugferdXmlValidator.Validate(invoice.ToString());
        Assert.IsFalse(result.IsValid);
        Assert.IsNull(result.Error);
        Assert.IsTrue(result.Validations.Any(m => m.Code == "FX-SCH-A-000122" && m.Message.Contains("BR-CO-16", StringComparison.Ordinal) &&
            m.Path != null && m.RuleSeverity == "error"));
    }

    [TestMethod]
    public void CodeListsAreResolvedFromEmbeddedResources()
    {
        var invoice = Invoice();
        invoice.Descendants(Ram + "InvoiceCurrencyCode").Single().Value = "ZZZ";
        var result = ZugferdXmlValidator.Validate(invoice.ToString());
        Assert.IsFalse(result.IsValid);
        Assert.IsNull(result.Error);
        Assert.IsTrue(result.Validations.Any(m => m.Message.Contains("not allowed", StringComparison.Ordinal)));
    }

    [TestMethod]
    [DoNotParallelize]
    public void InvoiceCannotRequestRemoteSchemasOrStylesheets()
    {
        using var network = new NetworkActivityListener();
        var invoice = Invoice();
        XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
        invoice.Root!.SetAttributeValue(xsi + "schemaLocation", invoice.Root.Name.NamespaceName + " http://127.0.0.1:1/evil.xsd");
        invoice.AddFirst(new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='http://127.0.0.1:1/evil.xsl'"));
        AssertValid(ZugferdXmlValidator.Validate(invoice.ToString()));
        Assert.AreEqual(0, network.Attempts, "Validation attempted an HTTP request, DNS lookup, or socket connection.");
    }

    private sealed class NetworkActivityListener : EventListener
    {
        private int attempts;
        internal int Attempts => Volatile.Read(ref attempts);

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            if (eventSource.Name is "System.Net.Http" or "System.Net.NameResolution" or "System.Net.Sockets")
                EnableEvents(eventSource, EventLevel.Verbose);
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData.EventName is "RequestStart" or "ResolutionStart" or "ConnectStart")
                Interlocked.Increment(ref attempts);
        }
    }

    [TestMethod]
    public void MissingSellerNameFailsBusinessRules()
    {
        var invoice = Invoice();
        invoice.Descendants(Ram + "SellerTradeParty").Single().Element(Ram + "Name")!.Remove();
        var result = ZugferdXmlValidator.Validate(invoice.ToString());
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Validations.Any(m => m.Code == "FX-SCH-A-000015" && m.Message.Contains("BR-06", StringComparison.Ordinal)));
    }

    [TestMethod]
    public void SchemaFailureStopsBeforeSchematron()
    {
        var invoice = Invoice();
        invoice.Descendants(Ram + "InvoiceCurrencyCode").Single().AddBeforeSelf(new XElement(Ram + "Unexpected"));
        var result = ZugferdXmlValidator.Validate(invoice.ToString());
        Assert.IsFalse(result.IsValid);
        Assert.IsNull(result.Error);
        Assert.IsTrue(result.Validations.All(m => m.Code == null));
        Assert.IsTrue(result.Validations.Any(m => m.Severity == XmlSeverityType.Error));
    }

    [TestMethod]
    public void SuccessfulReportsRemainNonblocking()
    {
        var invoice = Invoice();
        invoice.Root!.Elements().Single(e => e.Name.LocalName == "ExchangedDocument").Element(Ram + "ID")!.SetAttributeValue("schemeID", "unused");
        var result = ZugferdXmlValidator.Validate(invoice.ToString());
        AssertValid(result);
        Assert.IsTrue(result.Validations.Any(m => m.RuleSeverity == "warning" && m.Path != null));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("<CrossIndustryInvoice>")]
    [DataRow("<Invoice/>")]
    [DataRow("<CrossIndustryInvoice xmlns='wrong'/>")]
    [DataRow("<!DOCTYPE CrossIndustryInvoice [<!ENTITY x SYSTEM 'file:///secret'>]><CrossIndustryInvoice>&x;</CrossIndustryInvoice>")]
    [DataRow("<!DOCTYPE CrossIndustryInvoice SYSTEM 'http://127.0.0.1:1/evil.dtd'><CrossIndustryInvoice/>")]
    public void InvalidAndUnsupportedInputsFailClosed(string xml)
    {
        var result = ZugferdXmlValidator.Validate(xml);
        Assert.IsFalse(result.IsValid);
        Assert.IsFalse(string.IsNullOrWhiteSpace(result.Error));
    }

    [TestMethod]
    public void UnsupportedGuidelineFailsClosed()
    {
        var invoice = Invoice();
        invoice.Descendants(Ram + "GuidelineSpecifiedDocumentContextParameter").Single().Element(Ram + "ID")!.Value = "urn:factur-x.eu:1p0:basic";
        var result = ZugferdXmlValidator.Validate(invoice.ToString());
        Assert.IsFalse(result.IsValid);
        Assert.Contains("supports only", result.Error!);
    }

    [TestMethod]
    public async Task ConcurrentCallsHaveIndependentState()
    {
        var valid = Invoice().ToString();
        var invalidInvoice = Invoice();
        invalidInvoice.Descendants(Ram + "DuePayableAmount").Single().Value = "1.00";
        var invalid = invalidInvoice.ToString();
        var results = await Task.WhenAll(Enumerable.Range(0, 8).Select(i => Task.Run(() =>
            ZugferdXmlValidator.Validate(i % 2 == 0 ? valid : invalid))));
        for (var i = 0; i < results.Length; i++)
        {
            Assert.AreEqual(i % 2 == 0, results[i].IsValid, results[i].Error);
        }
    }
}
