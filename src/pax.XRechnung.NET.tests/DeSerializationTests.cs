
using System.Reflection;
using System.Xml.Serialization;
using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.tests;

[TestClass]
public sealed class DeSerializationTests
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
    public void CanDeSerialize(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var ressourceName = "pax.XRechnung.NET.tests.Resources.standard." + fileName;
        using var stream = assembly.GetManifestResourceStream(ressourceName);
        Assert.IsNotNull(stream, $"File error: {ressourceName}");

        var serializer = new XmlSerializer(typeof(XmlInvoice));
        var invoice = (XmlInvoice?)serializer.Deserialize(stream);
        Assert.IsNotNull(invoice);
        var id = invoice.Id.Content;
        Assert.IsNotNull(id, invoice.Id.Content);
    }
}
