﻿using pax.XRechnung.NET.Dtos;
using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET;

public static partial class XmlInvoiceMapper
{
    private static XmlInvoice Map2XmlInvoice(InvoiceDto invoiceDto, string currencyID = "EUR")
    {
        return new XmlInvoice()
        {
            CustomizationId = invoiceDto.CustomizationId,
            ProfileId = invoiceDto.ProfileId,
            Id = new() { Content = invoiceDto.Id },
            IssueDate = GetXmlDate(invoiceDto.IssueDate),
            DueDate = invoiceDto.DueDate == DateTime.MinValue || invoiceDto.DueDate == null ? null : GetXmlDate(invoiceDto.DueDate),
            InvoiceTypeCode = invoiceDto.InvoiceTypeCode,
            Note = string.IsNullOrWhiteSpace(invoiceDto.Note) ? null : invoiceDto.Note,
            DocumentCurrencyCode = invoiceDto.DocumentCurrencyCode,
            BuyerReference = invoiceDto.BuyerReference,
            AdditionalDocumentReferences = GetAdditionalDocumentReference(invoiceDto.AdditionalDocumentReferences),
            SellerParty = GetSellerParty(invoiceDto.Seller),
            BuyerParty = GetBuyerParty(invoiceDto.Buyer),
            PaymentMeans = GetPaymentInstructions(invoiceDto.PaymentMeans),
            TaxTotal = GetTaxTotal(invoiceDto.TaxTotal, currencyID),
            LegalMonetaryTotal = GetLegalMonetaryTotal(invoiceDto.LegalMonetaryTotal, currencyID),
            InvoiceLines = [.. invoiceDto.InvoiceLines.Select(s => GetInvoiceLine(s, currencyID))],
        };
    }

    private static XmlInvoiceLine GetInvoiceLine(InvoiceLineDto dto, string currencyId)
    {
        return new()
        {
            Id = new() { Content = dto.Id },
            Note = dto.Note,
            ObjectIdentifier = string.IsNullOrEmpty(dto.ObjectIdentifier) ? null :
             new() { Content = dto.ObjectIdentifier, SchemeIdentifier = dto.ObjectIdentifierSchema },
            InvoicedQuantity = new() { Value = dto.InvoicedQuantity, UnitCode = dto.InvoicedQuantityCode },
            LineExtensionAmount = new() { Value = dto.LineExtensionAmount, CurrencyID = currencyId },
            ReferencedPurchaseOrderLineReference = dto.ReferencedPurchaseOrderLineReference,
            BuyerAccountingReference = dto.BuyerAccountingReference,
            Item = new()
            {
                Description = dto.Description,
                Name = dto.Name,
                SellersIdentifier = string.IsNullOrEmpty(dto.SellersIdentifier) ? null :
                 new() { Content = dto.SellersIdentifier },
                BuyersIdentifier = string.IsNullOrEmpty(dto.BuyersIdentifier) ? null :
                 new() { Content = dto.BuyersIdentifier },
                StandardIdentifier = string.IsNullOrEmpty(dto.StandardIdentifier) ? null :
                 new() { Content = dto.StandardIdentifier },
                ClassificationIdentifiers = [.. dto.ClassificationIdentifiers.Select(s => new Identifier() { Content = s })],
                CountryOfOrigin = dto.CountryOfOrigin,
                Attributes = [.. dto.Attributes.Select(s => new XmlItemAttributes() {
                    Name = s.Name,
                    Value = s.Value,
                })],
                ClassifiedTaxCategory = new()
                {
                    Id = new() { Content = dto.TaxId },
                    Percent = dto.TaxPercent,
                    TaxScheme = new() { Id = new() { Content = dto.TaxScheme } },
                },
            },
            PriceDetails = new()
            {
                PriceAmount = new() { Value = dto.PriceAmount, CurrencyID = currencyId },
                PriceDiscount = dto.PriceDiscount == null ? null : new() { Value = dto.PriceDiscount.Value, CurrencyID = currencyId },
                GrossPrice = dto.GrossPrice == null ? null : new() { Value = dto.GrossPrice.Value, CurrencyID = currencyId },
                // PriceBaseQuantity = dto.PriceBaseQuantity == null ? null : new()
                // {
                //     Value = dto.PriceBaseQuantity.Value,
                //     UnitCode = dto.PriceBaseQuantityUnitOfMeasureCode ?? "HUR",
                // },
            },
            InvoiceLines = [.. dto.InvoiceLines.Select(s => GetInvoiceLine(s, currencyId))],
        };
    }

    private static XmlDocumentTotals GetLegalMonetaryTotal(DocumentTotalsDto dto, string currencyId)
    {
        return new()
        {
            LineExtensionAmount = new() { Value = dto.LineExtensionAmount, CurrencyID = currencyId },
            TaxExclusiveAmount = new() { Value = dto.TaxExclusiveAmount, CurrencyID = currencyId },
            TaxInclusiveAmount = new() { Value = dto.TaxInclusiveAmount, CurrencyID = currencyId },
            PayableAmount = new() { Value = dto.PayableAmount, CurrencyID = currencyId },
        };
    }

    private static XmlVatBreakdown GetTaxTotal(VatBreakdownDto dto, string currencyId)
    {
        return new()
        {
            TaxAmount = new Amount() { Value = dto.TaxAmount, CurrencyID = currencyId },
            TaxSubTotal = [
                new() {
                    TaxAmount = new Amount() { Value = dto.TaxAmount, CurrencyID = currencyId },
                    TaxableAmount = new Amount() { Value = dto.TaxableAmount, CurrencyID = currencyId },
                    TaxCategory = new() {
                        Id = new() { Content = dto.TaxCategoryId },
                        Percent = dto.Percent,
                        TaxScheme = new() { Id = new() { Content = dto.TaxScheme }}
                    },
                }
            ],
        };
    }

    private static XmlPaymentInstructions GetPaymentInstructions(PaymentInstructionsDto dto)
    {
        return new()
        {
            PaymentMeansTypeCode = dto.PaymentMeansTypeCode,
            PaymentMeansText = dto.PaymentMeansText,
            PayeeFinancialAccount = string.IsNullOrEmpty(dto.IBAN) ? [] :
            [
                new() {
                    Id = new() { Content = dto.IBAN },
                    Name = dto.AccountHolder,
                    FinancialInstitutionBranch = string.IsNullOrEmpty(dto.BIC) ? null :
                        new() {
                            Id = new() { Content = dto.BIC },
                            Name = dto.BankName
                        }
                }
            ]
        };
    }

    private static XmlBuyerParty GetBuyerParty(BuyerDto dto)
    {
        return new()
        {
            Party = new()
            {
                Contact = string.IsNullOrEmpty(dto.ContactName) ? null : new()
                {
                    Name = dto.ContactName,
                    Telephone = dto.ContactTelephone,
                    Email = dto.ContactEmail
                },
                EndpointId = new XmlEndpointId() { Content = dto.Email, SchemeId = "EM" },
                PartyName = new() { Name = dto.Name },
                PostalAddress = new()
                {
                    StreetName = dto.StreetName,
                    AdditionalStreetName = dto.AdditionalStreetName,
                    BlockName = dto.BlockName,
                    City = dto.City,
                    PostCode = dto.PostCode,
                    Country = new XmlCountry() { IdentificationCode = dto.Country }
                },
                PartyLegalEntity = new()
                {
                    RegistrationName = new() { Content = dto.Name }
                }
            }
        };
    }

    private static XmlSellerParty GetSellerParty(SellerDto dto)
    {
        return new()
        {
            Party = new()
            {
                Contact = new()
                {
                    Name = dto.ContactName,
                    Telephone = dto.ContactTelephone,
                    Email = dto.ContactEmail
                },
                Website = dto.Website,
                LogoReferenceId = dto.LogoReferenceId,
                EndpointId = new XmlEndpointId() { Content = dto.Email, SchemeId = "EM" },
                PartyName = new() { Name = dto.Name },
                Identifiers = string.IsNullOrEmpty(dto.TaxId) ? [] : [
                    new() { Id = new() { Content = dto.TaxId } }
                ],
                PostalAddress = new()
                {
                    StreetName = dto.StreetName,
                    AdditionalStreetName = dto.AdditionalStreetName,
                    BlockName = dto.BlockName,
                    City = dto.City,
                    PostCode = dto.PostCode,
                    Country = new XmlCountry() { IdentificationCode = dto.Country }
                },
                PartyTaxScheme = string.IsNullOrEmpty(dto.TaxCompanyId) ? null : new()
                {
                    RegistrationName = string.IsNullOrEmpty(dto.TaxRegistrationName) ? null : new()
                    {
                        Content = dto.TaxRegistrationName,
                    },
                    CompanyId = dto.TaxCompanyId,
                    TaxScheme = new() { Id = new() { Content = dto.TaxSchemeId } }
                },
                PartyLegalEntity = new()
                {
                    RegistrationName = new() { Content = dto.RegistrationName }
                },
            }
        };
    }

    private static List<XmlAdditionalDocumentReference> GetAdditionalDocumentReference(List<AdditionalDocumentReferenceDto> dtos)
    {
        if (dtos is null || dtos.Count == 0)
        {
            return [];
        }
        return [.. dtos.Select(s => new XmlAdditionalDocumentReference()
        {
            Id = new Identifier() { Content = s.Id },
            DocumentDescription = s.DocumentDescription,
            DocumentLocation = s.DocumentLocation,
            Attachment = string.IsNullOrEmpty(s.Content) ? null : new()
            {
                EmbeddedDocumentBinaryObject = new()
                {
                    MimeCode = s.MimeCode,
                    FileName = s.FileName,
                    Content = s.Content
                }
            }
        })];
    }

    internal static DateTime? GetXmlDate(DateTime? date)
    {
        if (date is null)
        {
            return null;
        }
        return GetXmlDate(date.Value);
    }

    internal static DateTime GetXmlDate(DateTime date)
    {
        var xmlDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
        return xmlDate;
    }
}
