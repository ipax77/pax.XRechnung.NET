
using pax.XRechnung.NET.BaseDtos;

namespace pax.XRechnung.NET.tests;

[TestClass]
public class BaseDtoTests
{
    [TestMethod]
    public async Task BaseSchematronIsValid()
    {
        InvoiceBaseDto baseDto = new()
        {
            Id = "1",
            IssueDate = DateTime.UtcNow,
            DocumentCurrencyCode = "EUR",
            BuyerReference = "bab@zack.org",
            SellerParty = new()
            {
                Name = "Seller Name",
                StreetName = "TestStreet",
                City = "TestCity",
                PostCode = "123456",
                CountryCode = "DE",
                RegistrationName = "bab@zack.org"
            },
            BuyerParty = new()
            {
                Name = "Buyer Name",
                StreetName = "TestStreet",
                City = "TestCity",
                PostCode = "123456",
                CountryCode = "DE",
                RegistrationName = "bab@zack.org"
            },
            InvoiceLines = [
                new() {
                    Id = "1",
                    Note = "Test Note",
                    Quantity = 1,
                    QuantityCode = "HUR",
                    Amount = 100.0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddHours(1),
                    Description = "Test Desc",
                    Name = "Test Name"
                },
            ]
        };

        var mapper = new InvoiceMapper<InvoiceBaseDto>();
        var xmlInvoice = mapper.ToXml(baseDto);

        var result = await XmlInvoiceValidator.ValidateSchematron(xmlInvoice);

        var resultText = string.Join(Environment.NewLine, result.Validations.Select(s => $"{s.Severity}:\t{s.Message}"));

        Assert.IsTrue(result.IsValid, resultText);
    }
}
