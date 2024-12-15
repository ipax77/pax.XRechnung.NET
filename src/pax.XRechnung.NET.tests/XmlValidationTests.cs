
using System.Reflection;

namespace pax.XRechnung.NET.tests;

[TestClass]
public sealed class ValidationTests
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
    public void CanValidate(string fileName)
    {
        Assert.IsTrue(assemblyPath != null, "Could not get ExecutionAssembly path");
        if (assemblyPath == null)
        {
            return;
        }
        var filePath = Path.Combine(assemblyPath, "Ressources", "Standard", fileName);

        var result = XmlInvoiceValidator.ValidateFile(filePath);
        Assert.IsNull(result.Error, result.Error);
        Assert.IsTrue(result.IsValid);
    }
}
