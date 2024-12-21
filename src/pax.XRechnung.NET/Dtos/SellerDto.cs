namespace pax.XRechnung.NET.Dtos;

/// <summary>
/// AccountingSupplierParty
/// </summary>
public record SellerDto : InvoiceParticipantDto
{
    /// <summary>
    /// Steuerliche Kennung des Verkäufers (optional).
    /// </summary>
    public string? TaxRegistrationIdentifier { get; set; }

}

/// <summary>
/// AccountingCustomerParty
/// </summary>
public record BuyerDto : InvoiceParticipantDto
{

}