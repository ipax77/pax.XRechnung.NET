
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
    public static InvoiceDto GetInvoiceDto(XmlInvoice xmlInvoice)
    {
        ArgumentNullException.ThrowIfNull(xmlInvoice);
        return Map2InvoiceDto(xmlInvoice);
    }

    /// <summary>
    /// Map InvoiceDto to XmlInvoice
    /// </summary>
    /// <param name="invoiceDto"></param>
    /// <returns></returns>
    public static XmlInvoice GetXmlInvoice(InvoiceDto invoiceDto)
    {
        ArgumentNullException.ThrowIfNull(invoiceDto);
        return Map2XmlInvoice(invoiceDto);
    }
}