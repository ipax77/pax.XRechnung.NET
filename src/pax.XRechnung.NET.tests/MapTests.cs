using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.tests;

[TestClass]
public class MapTests
{
    [TestMethod]
    public void CanMap2Xml()
    {
        InvoiceDto invoiceDto = new()
        {
            Id = "1",
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            InvoiceTypeCode = "380",
            Note = "Test Note",
            DocumentCurrencyCode = "EUR",
            BuyerReference = "123"
        };

        var xmlInvoice = XmlInvoiceMapper.GetXmlInvoice(invoiceDto);

        Assert.IsNotNull(xmlInvoice);
        Assert.AreEqual("1", xmlInvoice.Id.Content);
        Assert.AreEqual(invoiceDto.IssueDate, xmlInvoice.IssueDate);
        Assert.AreEqual(invoiceDto.DueDate, xmlInvoice.DueDate);
        Assert.AreEqual("380", xmlInvoice.InvoiceTypeCode);
        Assert.AreEqual("EUR", xmlInvoice.DocumentCurrencyCode);
        Assert.AreEqual("123", xmlInvoice.BuyerReference);
        Assert.AreEqual("Test Note", xmlInvoice.Note);
    }

    [TestMethod]
    public void CanMap2Xml_NullableFields()
    {
        // Arrange
        InvoiceDto invoiceDto = new()
        {
            Id = "1",
            IssueDate = DateTime.UtcNow,
            DueDate = null,
            InvoiceTypeCode = "380",
            DocumentCurrencyCode = "EUR",
            BuyerReference = "123",
            Note = ""
        };

        // Act
        var xmlInvoice = XmlInvoiceMapper.GetXmlInvoice(invoiceDto);

        // Assert
        Assert.IsNotNull(xmlInvoice);
        Assert.AreEqual("1", xmlInvoice.Id.Content);
        Assert.AreEqual(xmlInvoice.IssueDate, invoiceDto.IssueDate);
        Assert.IsNull(xmlInvoice.DueDate);
        Assert.AreEqual("380", xmlInvoice.InvoiceTypeCode);
        Assert.AreEqual("EUR", xmlInvoice.DocumentCurrencyCode);
        Assert.AreEqual("123", xmlInvoice.BuyerReference);
        Assert.IsNull(xmlInvoice.Note);
    }

    [TestMethod]
    public void CanMap2Dto()
    {
        XmlInvoice xmlInvoice = new()
        {
            Id = new() { Content = "1" },
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            InvoiceTypeCode = "380",
            Note = "Test Note",
            DocumentCurrencyCode = "EUR",
            BuyerReference = "123"
        };

        var invoiceDto = XmlInvoiceMapper.GetInvoiceDto(xmlInvoice);

        Assert.IsNotNull(invoiceDto);
        Assert.AreEqual("1", invoiceDto.Id);
        Assert.AreEqual(invoiceDto.IssueDate, xmlInvoice.IssueDate);
        Assert.AreEqual(invoiceDto.DueDate, xmlInvoice.DueDate);
        Assert.AreEqual("380", invoiceDto.InvoiceTypeCode);
        Assert.AreEqual("EUR", invoiceDto.DocumentCurrencyCode);
        Assert.AreEqual("123", invoiceDto.BuyerReference);
        Assert.AreEqual("Test Note", invoiceDto.Note);
    }
}
