
using pax.XRechnung.NET.Dtos;

namespace pax.XRechnung.NET.tests;

[TestClass]
public class DtoValidationTests
{
    private static InvoiceDto GetStandardInvoiceDto()
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
            },
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
            },
            PaymentMeans = new()
            {
                PaymentMeansTypeCode = "30",
                IBAN = "DE21081508151234123412",
                BankName = "Test Bank"
            },
            TaxTotal = new()
            {
                TaxAmount = 4.27M,
                TaxableAmount = 22.45M,
                TaxCategoryId = "S",
                Percent = 19.0M,
                TaxScheme = "VAT"
            },
            LegalMonetaryTotal = new()
            {
                LineExtensionAmount = 22.45M,
                TaxExclusiveAmount = 22.45M,
                TaxInclusiveAmount = 26.72M,
                PayableAmount = 26.72M,
            },
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
        return invoiceDto;
    }

    [TestMethod]
    public void CanValidateStandardDto()
    {
        var invoiceDto = GetStandardInvoiceDto();
        var xmlInvoice = XmlInvoiceMapper.MapToXmlInvoice(invoiceDto);
        var validationResult = XmlInvoiceValidator.Validate(xmlInvoice);

        var message = validationResult.Error != null ? validationResult.Error
         : string.Join(Environment.NewLine, validationResult.Validations.Select(s => s.Message));

        Assert.IsTrue(validationResult.IsValid, message);
    }

    [TestMethod]
    public void CanValidateDtoWithTaxRegistrationName()
    {
        var invoiceDto = GetStandardInvoiceDto();
        invoiceDto.Seller.TaxRegistrationName = "000/000/00000";
        var xmlInvoice = XmlInvoiceMapper.MapToXmlInvoice(invoiceDto);
        var validationResult = XmlInvoiceValidator.Validate(xmlInvoice);

        var message = validationResult.Error != null ? validationResult.Error
         : string.Join(Environment.NewLine, validationResult.Validations.Select(s => s.Message));

        Assert.IsTrue(validationResult.IsValid, message);
    }

    [TestMethod]
    public void CanValidateDtoWithAdditionalItemProperty()
    {
        var invoiceDto = GetStandardInvoiceDto();

        invoiceDto.InvoiceLines[0].Attributes = [
            new() { Name = "Startzeit", Value = "08:00" },
            new() { Name = "Endzeit", Value = "12:00" },
        ];

        var xmlInvoice = XmlInvoiceMapper.MapToXmlInvoice(invoiceDto);
        var validationResult = XmlInvoiceValidator.Validate(xmlInvoice);

        var message = validationResult.Error != null ? validationResult.Error
         : string.Join(Environment.NewLine, validationResult.Validations.Select(s => s.Message));

        Assert.IsTrue(validationResult.IsValid, message);
    }

    [TestMethod]
    public void CanValidateDtoWithItemQuantity()
    {
        var invoiceDto = GetStandardInvoiceDto();

        var line = invoiceDto.InvoiceLines[0];
        line.PriceBaseQuantity = 1;
        line.PriceBaseQuantityUnitOfMeasureCode = "HUR";
        line.PriceAmount = 60;
        line.InvoicedQuantity = 8;
        line.InvoicedQuantityCode = "HUR";
        line.LineExtensionAmount = 480;


        var xmlInvoice = XmlInvoiceMapper.MapToXmlInvoice(invoiceDto);
        var validationResult = XmlInvoiceValidator.Validate(xmlInvoice);

        var message = validationResult.Error != null ? validationResult.Error
         : string.Join(Environment.NewLine, validationResult.Validations.Select(s => s.Message));

        Assert.IsTrue(validationResult.IsValid, message);
    }
}