using System.Text.Json;
using pax.XRechnung.NET.Dtos;
using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.tests;

[TestClass]
public class MapTests
{
    private JsonSerializerOptions jsonOptions = new() { WriteIndented = true };

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

        var xmlInvoice = XmlInvoiceMapper.MapToXmlInvoice(invoiceDto);

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
        var xmlInvoice = XmlInvoiceMapper.MapToXmlInvoice(invoiceDto);

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

        var invoiceDto = XmlInvoiceMapper.MapToInvoiceDto(xmlInvoice);

        Assert.IsNotNull(invoiceDto);
        Assert.AreEqual("1", invoiceDto.Id);
        Assert.AreEqual(invoiceDto.IssueDate, xmlInvoice.IssueDate);
        Assert.AreEqual(invoiceDto.DueDate, xmlInvoice.DueDate);
        Assert.AreEqual("380", invoiceDto.InvoiceTypeCode);
        Assert.AreEqual("EUR", invoiceDto.DocumentCurrencyCode);
        Assert.AreEqual("123", invoiceDto.BuyerReference);
        Assert.AreEqual("Test Note", invoiceDto.Note);
    }

    [TestMethod]
    public void CanMapTwiceBase()
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

        XmlInvoice xmlInvoice = XmlInvoiceMapper.MapToXmlInvoice(invoiceDto);
        InvoiceDto invoiceDto2 = XmlInvoiceMapper.MapToInvoiceDto(xmlInvoice);
        
        var json1 = JsonSerializer.Serialize(invoiceDto, jsonOptions);
        var json2 = JsonSerializer.Serialize(invoiceDto2, jsonOptions);

        Assert.AreEqual(json1, json2);
    }

    [TestMethod]
    public void CanMapTwiceWithDocContent()
    {
        InvoiceDto invoiceDto = new()
        {
            Id = "1",
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            InvoiceTypeCode = "380",
            Note = "Test Note",
            DocumentCurrencyCode = "EUR",
            BuyerReference = "123",
            AdditionalDocumentReference = new()
            {
                Id = "invoice 123",
                DocumentDescription = "human readable pdf invoice",
                MimeCode = "application/pdf",
                FileName = "invoice.pdf",
                Content = "ZWYNCjE0OTk0Nw0KJSVFT0Y=",
            }
        };

        XmlInvoice xmlInvoice = XmlInvoiceMapper.MapToXmlInvoice(invoiceDto);
        InvoiceDto invoiceDto2 = XmlInvoiceMapper.MapToInvoiceDto(xmlInvoice);

        var json1 = JsonSerializer.Serialize(invoiceDto, jsonOptions);
        var json2 = JsonSerializer.Serialize(invoiceDto2, jsonOptions);

        Assert.AreEqual(json1, json2);
    }

    [TestMethod]
    public void CanMapTwiceWithParticipants()
    {
        InvoiceDto invoiceDto = new()
        {
            Id = "1",
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            InvoiceTypeCode = "380",
            Note = "Test Note",
            DocumentCurrencyCode = "EUR",
            BuyerReference = "123",
            Seller = new()
            {
                Email = "seller@email.com",
                Name = "Seller",
                StreetName = "TestStreet",
                City = "TestCity",
                PostCode = "12345",
                ContactName = "ContactName",
                ContactTelephone = "12345",
                ContactEmail = "contact@email.com",
                TaxCompanyId = "DE1234567",
                TaxSchemeId = "VAT",
                RegistrationName = "Seller Name",
            },
            Buyer = new()
            {
                Email = "buyer@email.com",
                Name = "Buyer",
                StreetName = "TestStreet1",
                City = "TestCity1",
                PostCode = "54321",
                ContactName = "ContactName1",
                ContactTelephone = "54321",
                ContactEmail = "contact1@email.com"
            }
        };

        XmlInvoice xmlInvoice = XmlInvoiceMapper.MapToXmlInvoice(invoiceDto);
        InvoiceDto invoiceDto2 = XmlInvoiceMapper.MapToInvoiceDto(xmlInvoice);

        var json1 = JsonSerializer.Serialize(invoiceDto, jsonOptions);
        var json2 = JsonSerializer.Serialize(invoiceDto2, jsonOptions);

        Assert.AreEqual(json1, json2);
    }

    [TestMethod]
    public void CanMapTwiceWithPaymentInstructions()
    {
        InvoiceDto invoiceDto = new()
        {
            Id = "1",
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            InvoiceTypeCode = "380",
            Note = "Test Note",
            DocumentCurrencyCode = "EUR",
            BuyerReference = "123",
            PaymentMeans = new()
            {
                PaymentMeansTypeCode = "30",
                IBAN = "DE21081508151234123412",
                BankName = "Test Bank"
            }
        };

        XmlInvoice xmlInvoice = XmlInvoiceMapper.MapToXmlInvoice(invoiceDto);
        InvoiceDto invoiceDto2 = XmlInvoiceMapper.MapToInvoiceDto(xmlInvoice);

        var json1 = JsonSerializer.Serialize(invoiceDto, jsonOptions);
        var json2 = JsonSerializer.Serialize(invoiceDto2, jsonOptions);

        Assert.AreEqual(json1, json2);
    }

    [TestMethod]
    public void CanMapTwiceWithTaxTotal()
    {
        InvoiceDto invoiceDto = new()
        {
            Id = "1",
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            InvoiceTypeCode = "380",
            Note = "Test Note",
            DocumentCurrencyCode = "EUR",
            BuyerReference = "123",
            TaxTotal = new()
            {
                TaxAmount = 4.27M,
                TaxableAmount = 22.45M,
                TaxCategoryId = "S",
                Percent = 19.0M,
                TaxScheme = "VAT"
            },
        };

        XmlInvoice xmlInvoice = XmlInvoiceMapper.MapToXmlInvoice(invoiceDto);
        InvoiceDto invoiceDto2 = XmlInvoiceMapper.MapToInvoiceDto(xmlInvoice);

        var json1 = JsonSerializer.Serialize(invoiceDto, jsonOptions);
        var json2 = JsonSerializer.Serialize(invoiceDto2, jsonOptions);

        Assert.AreEqual(json1, json2);
    }

    [TestMethod]
    public void CanMapTwiceWithMonetaryTotal()
    {
        InvoiceDto invoiceDto = new()
        {
            Id = "1",
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            InvoiceTypeCode = "380",
            Note = "Test Note",
            DocumentCurrencyCode = "EUR",
            BuyerReference = "123",
            LegalMonetaryTotal = new()
            {
                LineExtensionAmount = 22.45M,
                TaxExclusiveAmount = 22.45M,
                TaxInclusiveAmount = 26.72M,
                PayableAmount = 26.72M,
            },
        };

        XmlInvoice xmlInvoice = XmlInvoiceMapper.MapToXmlInvoice(invoiceDto);
        InvoiceDto invoiceDto2 = XmlInvoiceMapper.MapToInvoiceDto(xmlInvoice);

        var json1 = JsonSerializer.Serialize(invoiceDto, jsonOptions);
        var json2 = JsonSerializer.Serialize(invoiceDto2, jsonOptions);

        Assert.AreEqual(json1, json2);
    }

    [TestMethod]
    public void CanMapTwiceWithInvoiceLine()
    {
        InvoiceDto invoiceDto = new()
        {
            Id = "1",
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            InvoiceTypeCode = "380",
            Note = "Test Note",
            DocumentCurrencyCode = "EUR",
            BuyerReference = "123",
            InvoiceLines = [
                new() {
                    Id = "1",
                    Note = "Test Note",
                    Name = "Test",
                    Description ="Test Desc",
                    TaxId = "S",
                    TaxPercent = 19.0M,
                    TaxScheme = "VAT",
                    InvoicedQuantity = 1,
                    InvoicedQuantityCode = "XPP",
                    LineExtensionAmount = 22.45M,
                    PriceAmount = 22.45M,
                },
            ]
        };

        XmlInvoice xmlInvoice = XmlInvoiceMapper.MapToXmlInvoice(invoiceDto);
        InvoiceDto invoiceDto2 = XmlInvoiceMapper.MapToInvoiceDto(xmlInvoice);

        var json1 = JsonSerializer.Serialize(invoiceDto, jsonOptions);
        var json2 = JsonSerializer.Serialize(invoiceDto2, jsonOptions);

        Assert.AreEqual(json1, json2);
    }
}
