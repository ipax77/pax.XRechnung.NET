using System.Reflection;
using System.Xml.Linq;
using System.Xml.Schema;

namespace pax.XRechnung.NET.tests;

[TestClass]
public sealed class LocalValidationTests
{
    private static readonly XNamespace Cbc = XmlInvoiceWriter.CommonBasicComponents;
    private static readonly XNamespace Cac = XmlInvoiceWriter.CommonAggregateComponents;

    private static XDocument Invoice()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
            "pax.XRechnung.NET.tests.Resources.standard.01.01a-INVOICE_ubl.xml")!;
        return XDocument.Load(stream);
    }

    public static IEnumerable<object[]> InvoiceResources => Assembly.GetExecutingAssembly()
        .GetManifestResourceNames().Where(n => n.Contains(".Resources.standard.", StringComparison.Ordinal) && n.EndsWith(".xml", StringComparison.Ordinal))
        .Select(n => new object[] { n });

    [TestMethod]
    [DynamicData(nameof(InvoiceResources))]
    public async Task ValidatesStandardFixtures(string name)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name)!;
        using var reader = new StreamReader(stream);
        var result = await XmlInvoiceValidator.ValidateSchematron(await reader.ReadToEndAsync());
        Assert.IsTrue(result.IsValid, result.Error ?? string.Join("\n", result.Validations.Select(m => m.Message)));
    }

    [TestMethod]
    public async Task ValidInvoiceDoesNotRequireHttp()
    {
        var result = await XmlInvoiceValidator.ValidateSchematron(Invoice().ToString());
        Assert.IsTrue(result.IsValid, result.Error);
        Assert.AreEqual(string.Empty, result.HttpStatusCode);
        Assert.Contains("2026-08-31", result.Conformity!);
        Assert.IsTrue(result.Validations.All(m => m.RuleSeverity == "information"));
    }

    [TestMethod]
    public async Task ReportsBusinessRuleCodesAndPaths()
    {
        var xml = Invoice();
        xml.Descendants(Cbc + "BuyerReference").Remove();
        var result = await XmlInvoiceValidator.ValidateSchematron(xml.ToString());
        Assert.IsFalse(result.IsValid);
        var message = result.Validations.Single(m => m.Code == "BR-DE-15");
        Assert.AreEqual(XmlSeverityType.Error, message.Severity);
        Assert.IsFalse(string.IsNullOrWhiteSpace(message.Path));
        Assert.IsNull(result.Error);
    }

    [TestMethod]
    public async Task ReportsIncorrectTotals()
    {
        var xml = Invoice();
        xml.Descendants(Cac + "TaxTotal").First().Element(Cbc + "TaxAmount")!.Value = "18.00";
        var result = await XmlInvoiceValidator.ValidateSchematron(xml.ToString());
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Validations.Any(m => m.Code == "BR-CO-15"));
    }

    [TestMethod]
    public async Task InvalidCurrencyIsRejected()
    {
        var xml = Invoice();
        xml.Descendants(Cbc + "DocumentCurrencyCode").Single().Value = "ZZZ";
        var result = await XmlInvoiceValidator.ValidateSchematron(xml.ToString());
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Validations.Any(m => m.Code == "BR-CL-04"));
    }

    [TestMethod]
    public async Task UnitCodeOverrideRemainsAWarning()
    {
        var xml = Invoice();
        xml.Descendants(Cbc + "InvoicedQuantity").First().SetAttributeValue("unitCode", "ZZZ");
        var result = await XmlInvoiceValidator.ValidateSchematron(xml.ToString());
        var finding = result.Validations.Single(m => m.Code == "BR-CL-23");
        Assert.AreEqual("warning", finding.RuleSeverity);
        Assert.AreEqual(XmlSeverityType.Warning, finding.Severity);
        Assert.IsTrue(result.IsValid, string.Join("\n", result.Validations.Select(m => m.Message)));
    }

    [TestMethod]
    public async Task ItemIdentifierOverrideRemainsAWarning()
    {
        var xml = Invoice();
        xml.Descendants(Cac + "SellersItemIdentification").First().AddAfterSelf(new XElement(Cac + "StandardItemIdentification",
            new XElement(Cbc + "ID", new XAttribute("schemeID", "ZZZ"), "example")));
        var result = await XmlInvoiceValidator.ValidateSchematron(xml.ToString());
        Assert.AreEqual("warning", result.Validations.Single(m => m.Code == "BR-CL-21").RuleSeverity);
        Assert.IsTrue(result.IsValid, result.Error);
    }

    [TestMethod]
    public async Task SubInvoiceLineOverrideIsAnError()
    {
        var xml = Invoice();
        var line = xml.Descendants(Cac + "InvoiceLine").First();
        var subline = new XElement(line) { Name = Cac + "SubInvoiceLine" };
        line.Add(subline);
        var result = await XmlInvoiceValidator.ValidateSchematron(xml.ToString());
        Assert.AreEqual("error", result.Validations.Single(m => m.Code == "UBL-CR-646").RuleSeverity);
        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public async Task SchemaErrorsDoNotRunSchematron()
    {
        var xml = Invoice();
        xml.Descendants(Cbc + "IssueDate").Single().Value = "not-a-date";
        var result = await XmlInvoiceValidator.ValidateSchematron(xml.ToString());
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Validations.Any(m => m.Severity == XmlSeverityType.Error));
        Assert.IsTrue(result.Validations.All(m => m.Code is null));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("<Invoice>")]
    [DataRow("<!DOCTYPE Invoice [<!ENTITY test SYSTEM 'file:///does-not-exist'>]><Invoice>&test;</Invoice>")]
    [DataRow("<Invoice xmlns='urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2'/>")]
    public async Task InvalidInputsCannotPass(string xml)
    {
        var result = await XmlInvoiceValidator.ValidateSchematron(xml);
        Assert.IsFalse(result.IsValid);
        Assert.IsFalse(string.IsNullOrWhiteSpace(result.Error));
    }

    [TestMethod]
    public async Task UnsupportedCustomizationCannotPass()
    {
        var xml = Invoice();
        xml.Descendants(Cbc + "CustomizationID").Single().Value = "urn:cen.eu:en16931:2017";
        var result = await XmlInvoiceValidator.ValidateSchematron(xml.ToString());
        Assert.IsFalse(result.IsValid);
        Assert.Contains("standard XRechnung", result.Error!);
    }

    [TestMethod]
    public async Task ConcurrentValidationHasIndependentState()
    {
        var valid = Invoice().ToString();
        var invalid = Invoice();
        invalid.Descendants(Cbc + "BuyerReference").Remove();
        var invalidText = invalid.ToString();
        var results = await Task.WhenAll(Enumerable.Range(0, 8).Select(i =>
            Task.Run(() => XmlInvoiceValidator.ValidateSchematron(i % 2 == 0 ? valid : invalidText))));
        for (var i = 0; i < results.Length; i++)
        {
            Assert.AreEqual(i % 2 == 0, results[i].IsValid, results[i].Error);
            Assert.AreEqual(i % 2 != 0, results[i].Validations.Any(m => m.Code == "BR-DE-15"));
        }
    }
}
