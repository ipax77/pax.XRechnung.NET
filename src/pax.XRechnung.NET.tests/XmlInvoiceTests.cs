using System.Xml;
using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.tests;

[TestClass]
public sealed class InvoiceTests
{
    public static XmlInvoice GetTestInvoice()
    {
        XmlInvoice invoice = new()
        {
            Id = new Identifier() { Content = "1" },
            InvoiceTypeCode = "380",
            BuyerReference = "991-33333TEST-33",
            DocumentCurrencyCode = "EUR",
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            Note = "Zahlbar innerhalb von 14 Tagen.",
            SellerParty = new()
            {
                Party = new()
                {
                    Website = "https://www.einvoicetest.com",
                    EndpointId = new() { Content = "buyer@email.com" },
                    PartyName = new() { Name = "Verkäufer" },
                    PostalAddress = new()
                    {
                        StreetName = "TestStraße",
                        City = "Testhausen",
                        PostCode = "12345",
                    },
                    PartyTaxScheme = new()
                    {
                        CompanyId = "DE123456789",
                        TaxScheme = new() { Id = new() { Content = "VAT" } },
                    },
                    PartyLegalEntity = new()
                    {
                        RegistrationName = new() { Content = "Verkäufer" },
                    },
                    Contact = new()
                    {
                        Name = "Test",
                        Telephone = "12345",
                        Email = "test@example.com"
                    }
                }
            },
            BuyerParty = new()
            {
                Party = new()
                {
                    EndpointId = new() { Content = "seller@email.com" },
                    PartyName = new() { Name = "Käufer" },
                    PartyLegalEntity = new()
                    {
                        RegistrationName = new() { Content = "Käufer" }
                    },
                    PostalAddress = new()
                    {
                        StreetName = "TestStraße",
                        City = "Testhausen",
                        PostCode = "12345",
                    }
                }
            },
            PaymentMeans = new()
            {
                PaymentMeansTypeCode = "58",
                PayeeFinancialAccount = [
                    new() {
                        Id = new() { Content = "DE21081508151234123412" },
                        Name = "Test"
                    }
                ]
            },
            LegalMonetaryTotal = new()
            {
                LineExtensionAmount = new() { Value = 22.45M, CurrencyID = "EUR" },
                TaxExclusiveAmount = new() { Value = 22.45M, CurrencyID = "EUR" },
                TaxInclusiveAmount = new() { Value = 26.72M, CurrencyID = "EUR" },
                PayableAmount = new() { Value = 26.72M, CurrencyID = "EUR" },
            },
            TaxTotal = new()
            {
                TaxAmount = new() { Value = 4.27M, CurrencyID = "EUR" },
                TaxSubTotal = [
                    new() {
                        TaxableAmount = new() { Value = 22.45M, CurrencyID = "EUR" },
                        TaxAmount = new() { Value = 4.27M, CurrencyID = "EUR" },
                        TaxCategory = new() {
                            Id = new() { Content = "S" },
                            Percent = 19.0M,
                            TaxScheme = new()
                            {
                                Id = new() { Content = "VAT" }
                            }
                        }
                    }
                ]
            }
        };

        invoice.InvoiceLines.Add(new()
        {
            Id = new() { Content = "0" },
            Note = "Und es war Sommer",
            Item = new()
            {
                Name = "Test",
                Description = "XRechnung validation",
                ClassifiedTaxCategory = new()
                {
                    Id = new() { Content = "S" },
                    Percent = 19,
                    TaxScheme = new()
                    {
                        Id = new() { Content = "VAT" }
                    }
                },
            },
            InvoicedQuantity = new() { Value = 1, UnitCode = "XPP" },
            LineExtensionAmount = new() { Value = 22.45M, CurrencyID = "EUR" },
            PriceDetails = new()
            {
                PriceAmount = new Amount() { Value = 22.45M, CurrencyID = "EUR" }
            }
        });
        return invoice;
    }

    [TestMethod]
    public void CanSerialize()
    {
        var invoice = GetTestInvoice();
        var xmlText = XmlInvoiceWriter.Serialize(invoice);
        Assert.IsTrue(xmlText.Length > 0);

        // File.WriteAllText("/data/xrechnung/testinvoice2.xml", xmlText);
    }

    [TestMethod]
    public void CanValidate()
    {
        var invoice = GetTestInvoice();
        var xmlText = XmlInvoiceWriter.Serialize(invoice);
        XmlDocument document = new();
        document.Schemas = XmlInvoiceWriter.GetSchemaSet();

        // remove xml declaration
        var lines = xmlText.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var adjustedText = String.Join(Environment.NewLine, lines[1..]);
        document.LoadXml(adjustedText);

        // Act
        bool validationErrorsFound = false;
        document.Validate((sender, e) =>
        {
            validationErrorsFound = true;
            Console.WriteLine($"{e.Severity}: {e.Message}");
        });

        // Assert
        Assert.IsFalse(validationErrorsFound, "XML validation errors were found.");
    }
}
