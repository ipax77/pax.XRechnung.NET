namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// IBAN and BIC info
/// </summary>
public partial class PaymentMeansBaseDto
{
    /// <summary>
    /// Iban
    /// </summary>
    public string Iban { get; set; } = string.Empty;
    /// <summary>
    /// Bic
    /// </summary>
    public string Bic { get; set; } = string.Empty;
    /// <summary>
    /// Bank Name
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
