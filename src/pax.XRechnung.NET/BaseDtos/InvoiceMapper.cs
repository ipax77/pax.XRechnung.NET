namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// DocumentReferenceMapper
/// </summary>
public class DocumentReferenceMapper : DocumentReferenceMapperBase<DocumentReferenceBaseDto>
{
}

/// <summary>
/// InvoiceSellerPartyMapper
/// </summary>
public class InvoiceSellerPartyMapper : InvoiceSellerPartyMapperBase<PartyBaseDto>
{
}

/// <summary>
/// InvoiceBuyerPartyMapper
/// </summary>
public class InvoiceBuyerPartyMapper : InvoiceBuyerPartyMapperBase<PartyBaseDto>
{
}

/// <summary>
/// InvoiceLineMapper
/// </summary>
public class InvoiceLineMapper : InvoiceLineMapperBase<InvoiceLineBaseDto>
{
}

/// <summary>
/// InvoiceMapper implementation
/// </summary>
public class InvoiceMapper : InvoiceMapperBase<InvoiceBaseDto, DocumentReferenceBaseDto, PartyBaseDto, PartyBaseDto, InvoiceLineBaseDto>
{
    /// <summary>
    /// InvoiceMapper
    /// </summary>
    public InvoiceMapper()
    : base(
        new DocumentReferenceMapper(),
        new InvoiceSellerPartyMapper(),
        new InvoiceBuyerPartyMapper(),
        new InvoiceLineMapper()
    )
    {
    }
}
