
using System.Reflection;
using AutoFixture;
using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.tests;

[TestClass]
public sealed class ValidationTests
{
    [DataTestMethod]
    [DataRow("02.01a-INVOICE_ubl.xml")]
    [DataRow("02.02a-INVOICE_ubl.xml")]
    [DataRow("02.03a-INVOICE_ubl.xml")]
    [DataRow("02.04a-INVOICE_ubl.xml")]
    [DataRow("02.05a-INVOICE_ubl.xml")]
    [DataRow("02.06a-INVOICE_ubl.xml")]
    [DataRow("03.01a-INVOICE_ubl.xml")]
    [DataRow("03.02a-INVOICE_ubl.xml")]
    public void CanValidate(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var ressourceName = "pax.XRechnung.NET.tests.Resources.standard." + fileName;
        using var stream = assembly.GetManifestResourceStream(ressourceName);
        Assert.IsNotNull(stream, $"File error: {ressourceName}");

        using var reader = new StreamReader(stream);
        var xmlContent = reader.ReadToEnd();

        var result = XmlInvoiceValidator.ValidateXmlText(xmlContent);
        Assert.IsNull(result.Error, result.Error);
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void XmlIsValid()
    {
        Fixture fixture = new();
        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        XmlInvoice invoice = fixture.Create<XmlInvoice>();

        var result = XmlInvoiceValidator.Validate(invoice);
        Assert.IsNull(result.Error, result.Error, result.Error);
        Assert.IsTrue(result.IsValid, string.Join(Environment.NewLine, result.Validations
            .Where(x => x.Severity == System.Xml.Schema.XmlSeverityType.Error).Select(s => s.Message)));
    }
}
