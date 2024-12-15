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
        };
    }
}
