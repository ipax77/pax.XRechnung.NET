
using pax.XRechnung.NET.Dtos;
using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET;

/// <summary>
/// XmlInvoiceMapper
/// </summary>
public static partial class XmlInvoiceMapper
{
    /// <summary>
    /// Map XmlInvoice to InvoiceDto
    /// </summary>
    /// <param name="xmlInvoice"></param>
    /// <returns></returns>
    public static InvoiceDto MapToInvoiceDto(XmlInvoice xmlInvoice)
    {
        ArgumentNullException.ThrowIfNull(xmlInvoice);
        return Map2InvoiceDto(xmlInvoice);
    }

    /// <summary>
    /// Map InvoiceDto to XmlInvoice
    /// </summary>
    /// <param name="invoiceDto"></param>
    /// <returns></returns>
    public static XmlInvoice MapToXmlInvoice(InvoiceDto invoiceDto)
    {
        ArgumentNullException.ThrowIfNull(invoiceDto);
        return Map2XmlInvoice(invoiceDto);
    }
}