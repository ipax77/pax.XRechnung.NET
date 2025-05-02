
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using pax.XRechnung.NET.BaseDtos;
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

    [TestMethod]
    public void CanDeSerializeDto()
    {
        var invoiceDto = BaseDtoTests.GetInvoiceBaseDto();
        var mapper = new InvoiceMapper();
        var xmlInvoice = mapper.ToXml(invoiceDto);
        var xmlText = XmlInvoiceWriter.Serialize(xmlInvoice);
        var serializer = new XmlSerializer(typeof(XmlInvoice));
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlText));
        stream.Position = 0;
        var deserializedInvoice = (XmlInvoice?)serializer.Deserialize(stream);
        Assert.IsNotNull(deserializedInvoice);
        var deserializedDto = mapper.FromXml(deserializedInvoice);
        var json1 = JsonSerializer.Serialize(invoiceDto);
        var json2 = JsonSerializer.Serialize(deserializedDto);
        Assert.AreEqual(json1, json2);
    }
}
