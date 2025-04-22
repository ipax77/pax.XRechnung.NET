using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// InvoiceMapper Interface
/// </summary>
/// <typeparam name="T">InvoiceBaseDto</typeparam>
public interface IInvoiceMapper<T> where T : InvoiceBaseDto
{
    /// <summary>
    /// Map xmlInvoice to T
    /// </summary>
    /// <param name="xmlInvoice"></param>
    /// <returns></returns>
    T FromXml(XmlInvoice xmlInvoice);
    /// <summary>
    /// Map T to XmlInvoice
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    XmlInvoice ToXml(T dto);
}
