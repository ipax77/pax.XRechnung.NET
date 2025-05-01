using pax.XRechnung.NET.BaseDtos;

namespace pax.XRechnung.NET.AnnotatedDtos;

/// <summary>
/// DocumentReferenceMapper
/// </summary>
public class DocumentReferenceAnnotationMapper : DocumentReferenceMapperBase<DocumentReferenceAnnotationDto>
{
}

/// <summary>
/// InvoiceSellerPartyMapper
/// </summary>
public class InvoiceSellerPartyAnnotationMapper : InvoiceSellerPartyMapperBase<SellerAnnotationDto>
{
}

/// <summary>
/// InvoiceBuyerPartyMapper
/// </summary>
public class InvoiceBuyerPartyAnnotationMapper : InvoiceBuyerPartyMapperBase<BuyerAnnotationDto>
{
}

/// <summary>
/// InvoiceLineMapper
/// </summary>
public class InvoiceLineAnnotationMapper : InvoiceLineMapperBase<InvoiceLineAnnotationDto>
{
}

/// <summary>
/// Invoice Mapper for annotated DTOs
/// </summary>
public class InvoiceAnnotationMapper : InvoiceMapperBase<InvoiceAnnotationDto, DocumentReferenceAnnotationDto, SellerAnnotationDto, BuyerAnnotationDto, InvoiceLineAnnotationDto>
{
    /// <summary>
    /// InvoiceMapper
    /// </summary>
    public InvoiceAnnotationMapper()
    : base(
        new DocumentReferenceAnnotationMapper(),
        new InvoiceSellerPartyAnnotationMapper(),
        new InvoiceBuyerPartyAnnotationMapper(),
        new InvoiceLineAnnotationMapper()
    )
    {
    }
}