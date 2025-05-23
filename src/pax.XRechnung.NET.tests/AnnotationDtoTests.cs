
using System.ComponentModel.DataAnnotations;
using pax.XRechnung.NET.AnnotatedDtos;
using pax.XRechnung.NET.BaseDtos;

namespace pax.XRechnung.NET.tests;

[TestClass]
public class AnnotationDtoTests
{
    public static InvoiceAnnotationDto GetInvoiceAnnDto()
    {
        return new()
        {
            GlobalTaxCategory = "S",
            GlobalTaxScheme = "VAT",
            GlobalTax = 19.0,
            Id = "1",
            IssueDate = DateTime.UtcNow,
            InvoiceTypeCode = "380",
            DocumentCurrencyCode = "EUR",
            SellerParty = new SellerAnnotationDto()
            {
                Name = "Seller Name",
                StreetName = "Test Street",
                City = "Test City",
                PostCode = "123456",
                CountryCode = "DE",
                Telefone = "1234/54321",
                Email = "seller@example.com",
                RegistrationName = "Seller Name",
                TaxId = "DE12345678"
            },
            BuyerParty = new BuyerAnnotationDto()
            {
                Name = "Buyer Name",
                StreetName = "Test Street",
                City = "Test City",
                PostCode = "123456",
                CountryCode = "DE",
                Telefone = "1234/54321",
                Email = "buyer@example.com",
                RegistrationName = "Buyer Name",
                BuyerReference = "04011000-12345-34",
            },
            PaymentMeans = new PaymentAnnotationDto()
            {
                Iban = "DE12 1234 1234 1234 1234 12",
                Bic = "BICABCDE",
                Name = "Bank Name",
                PaymentMeansTypeCode = "30",
            },
            PaymentTermsNote = "Zahlbar innerhalb 14 Tagen nach Erhalt der Rechnung.",
            PayableAmount = 119.0,
            InvoiceLines = [
                new InvoiceLineAnnotationDto()
                {
                    Id = "1",
                    Quantity = 1.0,
                    QuantityCode = "HUR",
                    UnitPrice = 100.0,
                    Name = "Test Job"
                }
            ]
        };
    }

    [TestMethod]
    public void InvoiceBaseDtoSchemaIsValidTest()
    {
        var invoiceAnnDto = GetInvoiceAnnDto();
        var mapper = new InvoiceAnnotationMapper();
        var xmlInvoice = mapper.ToXml(invoiceAnnDto);
        var result = XmlInvoiceValidator.Validate(xmlInvoice);
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void FromXml_MapsCorrectlyToDto()
    {
        var xml = SchematronValidationTests.GetStandardXmlInvoice();
        var mapper = new InvoiceAnnotationMapper();

        var dto = mapper.FromXml(xml);

        Assert.AreEqual(xml.Id.Content, dto.Id);
        Assert.AreEqual(xml.DocumentCurrencyCode, dto.DocumentCurrencyCode);
        Assert.AreEqual(xml.SellerParty.Party.PartyName.Name, dto.SellerParty.Name);
        Assert.AreEqual(xml.InvoiceLines.Count, dto.InvoiceLines.Count);
    }

    [TestMethod]
    public void Roundtrip_ProducesEquivalentXml()
    {
        var original = SchematronValidationTests.GetStandardXmlInvoice();
        var mapper = new InvoiceAnnotationMapper();

        var dto = mapper.FromXml(original);
        var roundtripXml = mapper.ToXml(dto);

        Assert.AreEqual(original.Id.Content, roundtripXml.Id.Content);
        Assert.AreEqual(original.InvoiceLines.Count, roundtripXml.InvoiceLines.Count);
        Assert.AreEqual(original.SellerParty.Party.PartyName.Name, roundtripXml.SellerParty.Party.PartyName.Name);
    }

    [TestMethod]
    public void AnnotationIsValid()
    {
        var invoiceAnnDto = GetInvoiceAnnDto();

        // Validate the DTO properties
        var validationContext = new ValidationContext(invoiceAnnDto);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(invoiceAnnDto, validationContext, validationResults, true);

        // Check model validation (ValidCode, Required, etc.)
        Assert.IsTrue(isValid, string.Join("; ", validationResults.Select(r => r.ErrorMessage)));
    }

    [TestMethod]
    public void AnnotationIsInValid()
    {
        var invoiceAnnDto = GetInvoiceAnnDto();
        invoiceAnnDto.GlobalTaxCategory = "__InvalidCode__";

        // Validate the DTO properties
        var validationContext = new ValidationContext(invoiceAnnDto);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(invoiceAnnDto, validationContext, validationResults, true);

        Assert.IsFalse(isValid);
        Assert.IsTrue(validationResults.Any());
        Assert.AreEqual("The code '__InvalidCode__' is not valid for list 'UNTDID_5305_3'.",
         validationResults.FirstOrDefault()?.ErrorMessage);
    }
}
