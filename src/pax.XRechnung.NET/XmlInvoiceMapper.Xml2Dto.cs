using pax.XRechnung.NET.Dtos;
using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET;

public static partial class XmlInvoiceMapper
{
    private static InvoiceDto Map2InvoiceDto(XmlInvoice xmlInvoice)
    {
        return new InvoiceDto()
        {
            CustomizationId = xmlInvoice.CustomizationId,
            ProfileId = xmlInvoice.ProfileId,
            Id = xmlInvoice.Id.Content,
            IssueDate = xmlInvoice.IssueDate,
            DueDate = xmlInvoice.DueDate,
            InvoiceTypeCode = xmlInvoice.InvoiceTypeCode,
            Note = xmlInvoice.Note,
            DocumentCurrencyCode = xmlInvoice.DocumentCurrencyCode,
            BuyerReference = xmlInvoice.BuyerReference,
            AdditionalDocumentReferences = GetAdditionalDocumentReference(xmlInvoice.AdditionalDocumentReferences),
            Seller = GetSellerDto(xmlInvoice.SellerParty),
            Buyer = GetBuyerDto(xmlInvoice.BuyerParty),
            PaymentMeans = GetPaymentInstructions(xmlInvoice.PaymentMeans),
            TaxTotal = GetTaxTotal(xmlInvoice.TaxTotal),
            LegalMonetaryTotal = GetLegalMonetaryTotal(xmlInvoice.LegalMonetaryTotal),
            InvoiceLines = [.. xmlInvoice.InvoiceLines.Select(s => GetInvoiceLine(s))],
        };
    }

    private static InvoiceLineDto GetInvoiceLine(XmlInvoiceLine xml)
    {
        return new()
        {
            Id = xml.Id.Content,
            Note = xml.Note,
            ObjectIdentifier = xml.ObjectIdentifier?.Content,
            ObjectIdentifierSchema = xml.ObjectIdentifier?.SchemeIdentifier,
            InvoicedQuantity = xml.InvoicedQuantity.Value,
            InvoicedQuantityCode = xml.InvoicedQuantity.UnitCode,
            LineExtensionAmount = xml.LineExtensionAmount.Value,
            ReferencedPurchaseOrderLineReference = xml.ReferencedPurchaseOrderLineReference,
            BuyerAccountingReference = xml.BuyerAccountingReference,
            Description = xml.Item.Description,
            Name = xml.Item.Name,
            SellersIdentifier = xml.Item.SellersIdentifier?.Content,
            BuyersIdentifier = xml.Item.BuyersIdentifier?.Content,
            StandardIdentifier = xml.Item.StandardIdentifier?.Content,
            ClassificationIdentifiers = [.. xml.Item.ClassificationIdentifiers.Select(s => s.Content)],
            CountryOfOrigin = xml.Item.CountryOfOrigin,
            Attributes = [.. xml.Item.Attributes.Select(s => new ItemAttributeDto() {
                Name = s.Name,
                Value = s.Value,
            })],
            TaxId = xml.Item.ClassifiedTaxCategory.Id.Content,
            TaxPercent = xml.Item.ClassifiedTaxCategory.Percent,
            TaxScheme = xml.Item.ClassifiedTaxCategory.TaxScheme.Id.Content,
            PriceAmount = xml.PriceDetails.PriceAmount.Value,
            PriceDiscount = xml.PriceDetails.PriceDiscount?.Value,
            GrossPrice = xml.PriceDetails.GrossPrice?.Value,
            PriceBaseQuantity = xml.PriceDetails.PriceBaseQuantity?.Value,
            PriceBaseQuantityUnitOfMeasureCode = xml.PriceDetails.PriceBaseQuantity?.UnitCode,
            InvoiceLines = [.. xml.InvoiceLines.Select(s => GetInvoiceLine(s))],
        };
    }

    private static DocumentTotalsDto GetLegalMonetaryTotal(XmlDocumentTotals xml)
    {
        return new()
        {
            LineExtensionAmount = xml.LineExtensionAmount.Value,
            TaxExclusiveAmount = xml.TaxExclusiveAmount.Value,
            TaxInclusiveAmount = xml.TaxInclusiveAmount.Value,
            PayableAmount = xml.PayableAmount.Value,
        };
    }

    private static VatBreakdownDto GetTaxTotal(XmlVatBreakdown xml)
    {
        var tax = xml.TaxSubTotal.FirstOrDefault();
        return new()
        {
            TaxAmount = xml.TaxAmount.Value,
            TaxableAmount = tax?.TaxableAmount.Value ?? 0,
            TaxCategoryId = tax?.TaxCategory.Id.Content ?? "",
            Percent = tax?.TaxCategory.Percent ?? 0,
            TaxScheme = tax?.TaxCategory.TaxScheme.Id.Content ?? "",
        };
    }

    private static PaymentInstructionsDto GetPaymentInstructions(XmlPaymentInstructions xml)
    {
        var account = xml.PayeeFinancialAccount.FirstOrDefault();
        return new()
        {
            PaymentMeansTypeCode = xml.PaymentMeansTypeCode,
            PaymentMeansText = xml.PaymentMeansText,
            IBAN = account?.Id.Content,
            AccountHolder = account?.Name,
            BIC = account?.FinancialInstitutionBranch?.Id.Content,
            BankName = account?.FinancialInstitutionBranch?.Name,
        };
    }

    private static BuyerDto GetBuyerDto(XmlBuyerParty buyerParty)
    {
        return new()
        {
            ContactName = buyerParty.Party.Contact?.Name,
            ContactTelephone = buyerParty.Party.Contact?.Telephone,
            ContactEmail = buyerParty.Party.Contact?.Email ?? "",
            Email = buyerParty.Party.EndpointId.Content,
            Name = buyerParty.Party.PartyName.Name,
            StreetName = buyerParty.Party.PostalAddress?.StreetName,
            AdditionalStreetName = buyerParty.Party.PostalAddress?.AdditionalStreetName,
            BlockName = buyerParty.Party.PostalAddress?.BlockName,
            City = buyerParty.Party.PostalAddress?.City ?? string.Empty,
            PostCode = buyerParty.Party.PostalAddress?.PostCode ?? string.Empty,
            Country = buyerParty.Party.PostalAddress?.Country?.IdentificationCode ?? string.Empty
        };
    }

    private static SellerDto GetSellerDto(XmlSellerParty sellerParty)
    {
        return new()
        {
            TaxRegistrationName = sellerParty.Party.PartyTaxScheme?.RegistrationName?.Content,
            ContactName = sellerParty.Party.Contact?.Name ?? "unknown",
            ContactTelephone = sellerParty.Party.Contact?.Telephone ?? "unknown",
            ContactEmail = sellerParty.Party.Contact?.Email ?? "unknown",
            Website = sellerParty.Party.Website,
            LogoReferenceId = sellerParty.Party.LogoReferenceId,
            Email = sellerParty.Party.EndpointId.Content,
            Name = sellerParty.Party.PartyName.Name,
            StreetName = sellerParty.Party.PostalAddress?.StreetName,
            AdditionalStreetName = sellerParty.Party.PostalAddress?.AdditionalStreetName,
            BlockName = sellerParty.Party.PostalAddress?.BlockName,
            City = sellerParty.Party.PostalAddress?.City ?? string.Empty,
            PostCode = sellerParty.Party.PostalAddress?.PostCode ?? string.Empty,
            Country = sellerParty.Party.PostalAddress?.Country?.IdentificationCode ?? string.Empty,
            TaxCompanyId = sellerParty.Party.PartyTaxScheme?.CompanyId ?? "",
            TaxSchemeId = sellerParty.Party.PartyTaxScheme?.TaxScheme.Id.Content ?? "",
            TaxId = sellerParty.Party.Identifiers.Count > 0 ? sellerParty.Party.Identifiers.First().Id.Content : string.Empty,
            RegistrationName = sellerParty.Party.PartyLegalEntity.RegistrationName.Content,
        };
    }

    private static List<AdditionalDocumentReferenceDto> GetAdditionalDocumentReference(List<XmlAdditionalDocumentReference> xmls)
    {
        if (xmls is null || xmls.Count == 0)
        {
            return [];
        }

        return [.. xmls.Select(s => new AdditionalDocumentReferenceDto() {
            Id = s.Id.Content,
            DocumentDescription = s.DocumentDescription,
            DocumentLocation = s.DocumentLocation,
            MimeCode = s.Attachment?.EmbeddedDocumentBinaryObject.MimeCode ?? string.Empty,
            FileName = s.Attachment?.EmbeddedDocumentBinaryObject.FileName ?? string.Empty,
            Content = s.Attachment?.EmbeddedDocumentBinaryObject.Content,
        })];
    }
}
