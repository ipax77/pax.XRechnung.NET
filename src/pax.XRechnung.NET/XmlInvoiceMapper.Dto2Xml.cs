using pax.XRechnung.NET.Dtos;
using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET;

public static partial class XmlInvoiceMapper
{
    private static XmlInvoice Map2XmlInvoice(InvoiceDto invoiceDto)
    {
        return new XmlInvoice()
        {
            CustomizationId = invoiceDto.CustomizationId,
            ProfileId = invoiceDto.ProfileId,
            Id = new() { Content = invoiceDto.Id },
            IssueDate = invoiceDto.IssueDate,
            DueDate = invoiceDto.DueDate == DateTime.MinValue || invoiceDto.DueDate == null ? null : invoiceDto.DueDate,
            InvoiceTypeCode = invoiceDto.InvoiceTypeCode,
            Note = string.IsNullOrWhiteSpace(invoiceDto.Note) ? null : invoiceDto.Note,
            DocumentCurrencyCode = invoiceDto.DocumentCurrencyCode,
            BuyerReference = invoiceDto.BuyerReference,
            AdditionalDocumentReference = GetAdditionalDocumentReference(invoiceDto.AdditionalDocumentReference),
            SellerParty = GetSellerParty(invoiceDto.Seller),
            BuyerParty = GetBuyerParty(invoiceDto.Buyer),
            PaymentMeans = GetPaymentInstructions(invoiceDto.PaymentMeans),
            TaxTotal = GetTaxTotal(invoiceDto.TaxTotal),
            LegalMonetaryTotal = GetLegalMonetaryTotal(invoiceDto.LegalMonetaryTotal),
            InvoiceLines = [.. invoiceDto.InvoiceLines.Select(s => GetInvoiceLine(s))],
        };
    }

    private static XmlInvoiceLine GetInvoiceLine(InvoiceLineDto dto)
    {
        return new()
        {
            Id = new() { Content = dto.Id },
            Note = dto.Note,
            ObjectIdentifier = string.IsNullOrEmpty(dto.ObjectIdentifier) ? null :
             new() { Content = dto.ObjectIdentifier, SchemeIdentifier = dto.ObjectIdentifierSchema },
            InvoicedQuantity = new() { Value = dto.InvoicedQuantity, UnitCode = dto.InvoicedQuantityCode },
            LineExtensionAmount = new() { Value = dto.LineExtensionAmount },
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
                PriceAmount = new() { Value = dto.PriceAmount },
                PriceDiscount = dto.PriceDiscount == null ? null : new() { Value = dto.PriceDiscount.Value },
                GrossPrice = dto.GrossPrice == null ? null : new() { Value = dto.GrossPrice.Value },
                PriceBaseQuantity = dto.PriceBaseQuantity == null ? null : new() { Value = dto.PriceBaseQuantity.Value },
                PriceBaseQuantityUnitOfMeasureCode = dto.PriceBaseQuantityUnitOfMeasureCode,
            },
            InvoiceLines = [.. dto.InvoiceLines.Select(s => GetInvoiceLine(s))],
        };
    }

    private static XmlDocumentTotals GetLegalMonetaryTotal(DocumentTotalsDto dto)
    {
        return new()
        {
            LineExtensionAmount = new() { Value = dto.LineExtensionAmount },
            TaxExclusiveAmount = new() { Value = dto.TaxExclusiveAmount },
            TaxInclusiveAmount = new() { Value = dto.TaxInclusiveAmount },
            PayableAmount = new() { Value = dto.PayableAmount },
        };
    }

    private static XmlVatBreakdown GetTaxTotal(VatBreakdownDto dto)
    {
        return new()
        {
            TaxAmount = new Amount() { Value = dto.TaxAmount },
            TaxSubTotal = [
                new() {
                    TaxAmount = new Amount() { Value = dto.TaxAmount },
                    TaxableAmount = new Amount() { Value = dto.TaxableAmount },
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
                    Identifier = string.IsNullOrEmpty(dto.BIC) ? null : new() { Content = dto.BIC },
                    Name = dto.BankName
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
                EndpointId = new XmlEndpointId() { Content = dto.Email },
                PartyName = new() { Name = dto.Name },
                PostalAddress = new()
                {
                    StreetName = dto.StreetName,
                    AdditionalStreetName = dto.AdditionalStreetName,
                    BlockName = dto.BlockName,
                    City = dto.City,
                    PostCode = dto.PostCode,
                    Country = new XmlCountry() { IdentificationCode = dto.Country }
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
                EndpointId = new XmlEndpointId() { Content = dto.Email },
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
                }
            }
        };
    }

    private static XmlAdditionalDocumentReference? GetAdditionalDocumentReference(AdditionalDocumentReferenceDto? dto)
    {
        if (dto is null)
        {
            return null;
        }
        return new()
        {
            Id = new Identifier() { Content = dto.Id },
            DocumentDescription = dto.DocumentDescription,
            DocumentLocation = dto.DocumentLocation,
            Attachment = string.IsNullOrEmpty(dto.Content) ? null : new()
            {
                EmbeddedDocumentBinaryObject = new()
                {
                    MimeCode = dto.MimeCode,
                    FileName = dto.FileName,
                    Content = dto.Content
                }
            }
        };
    }
}
