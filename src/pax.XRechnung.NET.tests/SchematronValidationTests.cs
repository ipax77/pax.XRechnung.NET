
using pax.XRechnung.NET.BaseDtos;
using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.tests;

[TestClass]
public class SchematronValidationTests
{
    private static readonly string validatorUri = "http://localhost:8080";
    private static bool kositServerIsRunning = false;

    [ClassInitialize()]
    public static async Task CheckKositAvailability(TestContext context)
    {
        kositServerIsRunning = await IsKositServerAvailable();
    }

    public static async Task<bool> IsKositServerAvailable(Uri? kostiUri = null)
    {
        if (kositServerIsRunning)
        {
            return true;
        }
        try
        {
            using var client = new HttpClient
            {
                BaseAddress = kostiUri ?? new Uri(validatorUri),
                Timeout = TimeSpan.FromSeconds(5)
            };

            // Try HEAD request for fast check
            var response = await client.GetAsync("/");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public static XmlInvoice GetStandardXmlInvoice()
    {
        return new()
        {
            Id = new() { Content = "1" },
            IssueDate = DateOnly.FromDateTime(DateTime.UtcNow),
            DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(14)),
            InvoiceTypeCode = "380",
            Note = "Test Note",
            DocumentCurrencyCode = "EUR",
            BuyerReference = "04011000-12345-34",
            SellerParty = new()
            {
                Party = new()
                {
                    EndpointId = new() { SchemeId = "EM", Content = "seller@test.org" },
                    Identifiers = [
                                new()
                        {
                            Id = new() { Content = "123/12345/123" },
                        }
                            ],
                    PartyName = new() { Name = "Seller Name" },
                    PostalAddress = new()
                    {
                        StreetName = "Test Street",
                        City = "Test City",
                        PostCode = "12345",
                        Country = new() { IdentificationCode = "DE" }
                    },
                    PartyTaxScheme = new()
                    {
                        CompanyId = "DE1234567",
                        TaxScheme = new()
                        {
                            Id = new() { Content = "S" }
                        }
                    },
                    PartyLegalEntity = new()
                    {
                        RegistrationName = "IT-Dienstleistungen"
                    },
                    Contact = new()
                    {
                        Name = "Seller Name",
                        Telephone = "1234/56789",
                        Email = "seller@test.org",
                    }
                }
            },
            BuyerParty = new()
            {
                Party = new()
                {
                    EndpointId = new() { SchemeId = "EM", Content = "buyer@test.org" },
                    PartyName = new() { Name = "Buyer Name" },
                    PostalAddress = new()
                    {
                        StreetName = "Test Street",
                        City = "Test City",
                        PostCode = "12345",
                        Country = new() { IdentificationCode = "DE" }
                    },
                    PartyLegalEntity = new()
                    {
                        RegistrationName = "Buyer Name"
                    },
                    Contact = new()
                    {
                        Name = "Buyer Name",
                        Telephone = "1234/56789",
                        Email = "buyer@test.org",
                    }
                }
            },
            PaymentMeans = new()
            {
                PaymentMeansTypeCode = "30",
                PayeeFinancialAccount = new()
                {
                    Id = new() { Content = "IBAN 1234 1234 1234 124 12" },
                    Name = "Seller Name",
                    FinancialInstitutionBranch = new()
                    {
                        Id = new() { Content = "BIC12345" },
                    }
                }
            },
            PaymentTerms = new()
            {
                Note = "Zahlbar innerhalb von 14 Tagen."
            },
            TaxTotal = new()
            {
                TaxAmount = new() { CurrencyID = "EUR", Value = 19.0m },
                TaxSubTotal = [
                    new()
                    {
                        TaxableAmount = new() { CurrencyID = "EUR", Value = 100.0m },
                        TaxAmount = new() { CurrencyID = "EUR", Value = 19.0m },
                        TaxCategory = new()
                        {
                            Id = new() { Content = "S" },
                            Percent = 19.0m,
                            TaxScheme = new() { Id = new() { Content = "VAT" }},
                        }
                    }
                        ]
            },
            LegalMonetaryTotal = new()
            {
                LineExtensionAmount = new() { CurrencyID = "EUR", Value = 100.0m },
                TaxExclusiveAmount = new() { CurrencyID = "EUR", Value = 100.0m },
                TaxInclusiveAmount = new() { CurrencyID = "EUR", Value = 119.0m },
                PayableAmount = new() { CurrencyID = "EUR", Value = 119.0m },
            },
            InvoiceLines = [
                        new()
                {
                    Id = new() { Content = "1" },
                    InvoicedQuantity = new() { UnitCode = "HUR", Value = 1.0m },
                    LineExtensionAmount = new() { CurrencyID = "EUR", Value = 100.0m },
                    Item = new()
                    {
                        Description = "Item Desc",
                        Name ="Item Name",
                        ClassifiedTaxCategory = new()
                        {
                            Id = new() { Content = "S" },
                            Percent = 19.0m,
                            TaxScheme = new()
                            {
                                Id = new() { Content = "VAT" },
                            }
                        }

                    },
                    PriceDetails = new()
                    {
                        PriceAmount = new() { CurrencyID = "EUR", Value = 100.0m }
                    }
                }
            ]
        };
    }

    [TestMethod]
    public async Task CanProduceValidXmlInvoice()
    {
        if (!kositServerIsRunning)
        {
            Assert.Inconclusive("Kosit Validator is not running on localhost:8080.");
        }
        XmlInvoice xmlInvoice = GetStandardXmlInvoice();

        var schemaResult = XmlInvoiceValidator.Validate(xmlInvoice);
        Assert.IsTrue(schemaResult.IsValid);

        var result = await XmlInvoiceValidator.ValidateSchematron(xmlInvoice);
        var resultText = string.Join(Environment.NewLine, result.Validations.Select(s => $"{s.Severity}:\t{s.Message}"));
        Assert.IsTrue(result.Validations.Count == 0, resultText);
        Assert.IsTrue(result.IsValid, resultText);
    }

    [TestMethod]
    public async Task CanIdentifyXmlInvoiceValidationErrors()
    {
        if (!kositServerIsRunning)
        {
            Assert.Inconclusive("Kosit Validator is not running on localhost:8080.");
        }
        XmlInvoice xmlInvoice = GetStandardXmlInvoice();
        xmlInvoice.TaxTotal.TaxAmount.Value = 18.0m;

        var result = await XmlInvoiceValidator.ValidateSchematron(xmlInvoice);
        var resultText = string.Join(Environment.NewLine, result.Validations.Select(s => $"{s.Severity}:\t{s.Message}"));
        var hasSumError = result.Validations.Where(x => x.Severity == System.Xml.Schema.XmlSeverityType.Error)
            .Any(a => a.Message.Contains("[BR-CO-15]-Invoice total amount with VAT (BT-112) = Invoice total amount without VAT (BT-109) + Invoice total VAT amount (BT-110)", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(hasSumError, resultText);
    }

    [TestMethod]
    public async Task CanValidateBaseDtoSchematronTest()
    {
        var invoiceBaseDto = BaseDtoTests.GetInvoiceBaseDto();
        InvoiceMapper<InvoiceBaseDto> invoiceMapper = new();
        XmlInvoice xmlInvoice = invoiceMapper.ToXml(invoiceBaseDto);
        var result = await XmlInvoiceValidator.ValidateSchematron(xmlInvoice);
        var resultText = string.Join(Environment.NewLine, result.Validations.Select(s => $"{s.Severity}:\t{s.Message}"));
        Assert.IsTrue(result.Validations.Count == 0, resultText);
        Assert.IsTrue(result.IsValid, resultText);
    }

    [TestMethod]
    public async Task CanValidateBaseDtoSchematronWithRoundingTest()
    {
        var invoiceBaseDto = BaseDtoTests.GetInvoiceBaseDto();

        var quantity = 1.17777;
        var unitPrice = 117.17;
        invoiceBaseDto.InvoiceLines[0].Quantity = quantity;
        invoiceBaseDto.InvoiceLines[0].UnitPrice = unitPrice;
        invoiceBaseDto.PayableAmount = quantity * unitPrice;

        InvoiceMapper<InvoiceBaseDto> invoiceMapper = new();
        XmlInvoice xmlInvoice = invoiceMapper.ToXml(invoiceBaseDto);
        var result = await XmlInvoiceValidator.ValidateSchematron(xmlInvoice);
        var resultText = string.Join(Environment.NewLine, result.Validations.Select(s => $"{s.Severity}:\t{s.Message}"));
        Assert.IsTrue(result.Validations.Count == 0, resultText);
        Assert.IsTrue(result.IsValid, resultText);
    }

    [TestMethod]
    public async Task CanProduceValidInvoiceDto()
    {
        if (!kositServerIsRunning)
        {
            Assert.Inconclusive("Kosit Validator is not running on localhost:8080.");
        }
        XmlInvoice xmlInvoice = GetStandardXmlInvoice();
        InvoiceMapper<InvoiceBaseDto> invoiceMapper = new();
        InvoiceBaseDto invoiceDto = invoiceMapper.FromXml(xmlInvoice);
        XmlInvoice mappedXmlInvoice = invoiceMapper.ToXml(invoiceDto);

        var schemaResult = XmlInvoiceValidator.Validate(mappedXmlInvoice);
        Assert.IsTrue(schemaResult.IsValid);

        var result = await XmlInvoiceValidator.ValidateSchematron(mappedXmlInvoice);
        var resultText = string.Join(Environment.NewLine, result.Validations.Select(s => $"{s.Severity}:\t{s.Message}"));
        Assert.IsTrue(result.Validations.Count == 0, resultText);
        Assert.IsTrue(result.IsValid, resultText);
    }
}