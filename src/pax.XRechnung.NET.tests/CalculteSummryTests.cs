
using pax.XRechnung.NET.Dtos;

namespace pax.XRechnung.NET.tests;
[TestClass]
public class CalculateSummaryTests
{
    [TestMethod]
    public void CanCalculteSummry()
    {
        var invoice = DtoValidationTests.GetStandardInvoiceDto();
        invoice.InvoiceLines.Clear();
        var line = new InvoiceLineDto()
        {
            Id = "1",
            Note = "Test Note",
            Name = "Test",
            Description = "Test Desc",
            TaxId = "S",
            TaxPercent = 19.0M,
            TaxScheme = "VAT",
            InvoicedQuantity = 1,
            InvoicedQuantityCode = "HUR",
            LineExtensionAmount = 100M,
            PriceAmount = 100M,
        };
        invoice.InvoiceLines.Add(line);

        invoice.CalculateTotals();

        Assert.AreEqual(100M, invoice.TaxTotal.TaxableAmount);
        Assert.AreEqual(19M, invoice.TaxTotal.TaxAmount);
        Assert.AreEqual(100M, invoice.LegalMonetaryTotal.LineExtensionAmount);
        Assert.AreEqual(100M, invoice.LegalMonetaryTotal.TaxExclusiveAmount);
        Assert.AreEqual(119M, invoice.LegalMonetaryTotal.TaxInclusiveAmount);
        Assert.AreEqual(119M, invoice.LegalMonetaryTotal.PayableAmount);
    }

    [TestMethod]
    public void CanCalculteComplexSummry()
    {
        var invoice = DtoValidationTests.GetStandardInvoiceDto();
        invoice.InvoiceLines.Clear();
        var line1 = new InvoiceLineDto()
        {
            Id = "1",
            Note = "Test Note",
            Name = "Test",
            Description = "Test Desc",
            TaxId = "S",
            TaxPercent = 19.0M,
            TaxScheme = "VAT",
            InvoicedQuantity = 1,
            InvoicedQuantityCode = "HUR",
            LineExtensionAmount = 100M,
            PriceAmount = 100M,
        };
        var line2 = new InvoiceLineDto()
        {
            Id = "2",
            Note = "Test Note",
            Name = "Test",
            Description = "Test Desc",
            TaxId = "S",
            TaxPercent = 19.0M,
            TaxScheme = "VAT",
            InvoicedQuantity = 2,
            InvoicedQuantityCode = "HUR",
            LineExtensionAmount = 100M,
            PriceAmount = 50M,
        };
        invoice.InvoiceLines.Add(line1);
        invoice.InvoiceLines.Add(line2);

        invoice.CalculateTotals();

        Assert.AreEqual(200M, invoice.TaxTotal.TaxableAmount);
        Assert.AreEqual(38M, invoice.TaxTotal.TaxAmount);
        Assert.AreEqual(200M, invoice.LegalMonetaryTotal.LineExtensionAmount);
        Assert.AreEqual(200M, invoice.LegalMonetaryTotal.TaxExclusiveAmount);
        Assert.AreEqual(238M, invoice.LegalMonetaryTotal.TaxInclusiveAmount);
        Assert.AreEqual(238M, invoice.LegalMonetaryTotal.PayableAmount);
    }
}
