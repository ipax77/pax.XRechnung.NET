using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// InvoiceMapper Interface
/// </summary>
public interface IInvoiceMapper<TInvoiceDto, TDocumentReferenceDto, TSellerPartyDto, TBuyerPartyDto, TLineDto>
    where TInvoiceDto : IInvoiceBaseDto, new()
    where TDocumentReferenceDto : IDocumentReferenceBaseDto, new()
    where TSellerPartyDto : IPartyBaseDto, new()
    where TBuyerPartyDto : IPartyBaseDto, new()
    where TLineDto : IInvoiceLineBaseDto, new()
{
    /// <summary>
    /// Map xmlInvoice to T
    /// </summary>
    /// <param name="xmlInvoice"></param>
    /// <returns></returns>
    TInvoiceDto FromXml(XmlInvoice xmlInvoice);
    /// <summary>
    /// Map T to XmlInvoice
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    XmlInvoice ToXml(TInvoiceDto dto);
}

