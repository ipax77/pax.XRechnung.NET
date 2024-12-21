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
        };
    }
}
