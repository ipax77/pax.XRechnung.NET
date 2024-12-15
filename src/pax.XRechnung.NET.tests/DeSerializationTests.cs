
using System.Reflection;
using System.Xml.Serialization;
using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.tests;

[TestClass]
public sealed class DeSerializationTests
{
    public static readonly string? assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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
        Assert.IsTrue(assemblyPath != null, "Could not get ExecutionAssembly path");
        if (assemblyPath == null)
        {
            return;
        }
        var filePath = Path.Combine(assemblyPath, "Ressources", "Standard", fileName);

        var serializer = new XmlSerializer(typeof(XmlInvoice));
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var invoice = (XmlInvoice?)serializer.Deserialize(stream);
        Assert.IsNotNull(invoice);
        var id = invoice.Id.Content;
        Assert.IsNotNull(id, invoice.Id.Content);
    }
}
