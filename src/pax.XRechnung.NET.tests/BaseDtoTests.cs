
using pax.XRechnung.NET.BaseDtos;

namespace pax.XRechnung.NET.tests;

[TestClass]
public class BaseDtoTests
{
    public static InvoiceBaseDto GetInvoiceBaseDto()
    {
        return new()
        {
            GlobalTaxCategory = "S",
            GlobalTaxScheme = "VAT",
            GlobalTax = 19.0,
            Id = "1",
            IssueDate = new DateTime(2025, 05, 01),
            InvoiceTypeCode = "380",
            DocumentCurrencyCode = "EUR",
            SellerParty = new PartyBaseDto()
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
            BuyerParty = new PartyBaseDto()
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
            PaymentMeans = new PaymentMeansBaseDto()
            {
                Iban = "DE12 1234 1234 1234 1234 12",
                Bic = "BICABCDE",
                Name = "Bank Name",
                PaymentMeansTypeCode = "30",
            },
            PaymentTermsNote = "Zahlbar innerhalb 14 Tagen nach Erhalt der Rechnung.",
            PayableAmount = 119.0,
            InvoiceLines = [
                new InvoiceLineBaseDto()
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
        var invoiceBaseDto = GetInvoiceBaseDto();
        var mapper = new InvoiceMapper();
        var xmlInvoice = mapper.ToXml(invoiceBaseDto);
        var result = XmlInvoiceValidator.Validate(xmlInvoice);
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void FromXml_MapsCorrectlyToDto()
    {
        var xml = SchematronValidationTests.GetStandardXmlInvoice();
        var mapper = new InvoiceMapper();

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
        var mapper = new InvoiceMapper();

        var dto = mapper.FromXml(original);
        var roundtripXml = mapper.ToXml(dto);

        Assert.AreEqual(original.Id.Content, roundtripXml.Id.Content);
        Assert.AreEqual(original.InvoiceLines.Count, roundtripXml.InvoiceLines.Count);
        Assert.AreEqual(original.SellerParty.Party.PartyName.Name, roundtripXml.SellerParty.Party.PartyName.Name);
    }
}
