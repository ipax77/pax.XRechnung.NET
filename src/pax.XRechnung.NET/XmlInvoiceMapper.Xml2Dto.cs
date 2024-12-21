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
            AdditionalDocumentReference = GetAdditionalDocumentReference(xmlInvoice.AdditionalDocumentReference),
            Seller = GetSellerDto(xmlInvoice.SellerParty),
            Buyer = GetBuyerDto(xmlInvoice.BuyerParty),
            PaymentMeans = GetPaymentInstructions(xmlInvoice.PaymentMeans),
            TaxTotal = GetTaxTotal(xmlInvoice.TaxTotal),
            LegalMonetaryTotal = GetLegalMonetaryTotal(xmlInvoice.LegalMonetaryTotal),
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
        return new()
        {
            TaxAmount = xml.TaxAmount.Value,
            TaxableAmount = xml.TaxSubTotal.FirstOrDefault()?.TaxableAmount.Value ?? 0,
            TaxCategoryId = xml.TaxSubTotal.FirstOrDefault()?.TaxCategory.Id.Content ?? "",
            Percent = xml.TaxSubTotal.FirstOrDefault()?.TaxCategory.Percent ?? 0,
            TaxScheme = xml.TaxSubTotal.FirstOrDefault()?.TaxCategory.TaxScheme.Id.Content ?? "",
        };
    }

    private static PaymentInstructionsDto GetPaymentInstructions(XmlPaymentInstructions xml)
    {
        return new()
        {
            PaymentMeansTypeCode = xml.PaymentMeansTypeCode,
            PaymentMeansText = xml.PaymentMeansText,
            IBAN = xml.PayeeFinancialAccount.FirstOrDefault()?.Id.Content,
            BankName = xml.PayeeFinancialAccount.FirstOrDefault()?.Name,
            BIC = xml.PayeeFinancialAccount.FirstOrDefault()?.Identifier?.Content,
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
            TaxRegistrationIdentifier = sellerParty.Party.TaxRegistrationIdentifier?.Content,
            ContactName = sellerParty.Party.Contact?.Name ?? "unknown",
            ContactTelephone = sellerParty.Party.Contact?.Telephone ?? "unknown",
            ContactEmail = sellerParty.Party.Contact?.Email ?? "unknown",
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
            RegistrationName = sellerParty.Party.PartyLegalEntity.RegistrationName,
        };
    }

    private static AdditionalDocumentReferenceDto? GetAdditionalDocumentReference(XmlAdditionalDocumentReference? xml)
    {
        if (xml is null)
        {
            return null;
        }
        var dto = new AdditionalDocumentReferenceDto()
        {
            Id = xml.Id.Content,
            DocumentDescription = xml.DocumentDescription,
            DocumentLocation = xml.DocumentLocation,
        };

        if (xml.Attachment is not null)
        {
            dto.MimeCode = xml.Attachment.EmbeddedDocumentBinaryObject.MimeCode;
            dto.FileName = xml.Attachment.EmbeddedDocumentBinaryObject.FileName;
            dto.Content = xml.Attachment.EmbeddedDocumentBinaryObject.Content;
        }

        return dto;
    }
}
