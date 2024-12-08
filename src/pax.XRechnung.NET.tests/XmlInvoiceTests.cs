﻿using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.tests;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void CanSerialize()
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
                    EndpointId = new() { Content = "buyer@email.com" },
                    PartyName = new() { Name = "Verkäufer" },
                    // TaxRegistrationIdentifier = new() { Content = "000/000/00000" },
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
                        RegistrationName = "Verkäufer"
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
                        RegistrationName = "Käufer"
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
        };

        var xmlText = XmlInvoiceWriter.Serialize(invoice);
        Assert.IsTrue(xmlText.Length > 0);
    }
}
