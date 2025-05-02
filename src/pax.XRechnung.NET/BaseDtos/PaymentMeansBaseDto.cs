namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// IPaymentMeansBaseDto used for mapping
/// </summary>
public interface IPaymentMeansBaseDto
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    string Iban { get; set; }
    string Bic { get; set; }
    string Name { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

/// <summary>
/// IBAN and BIC info
/// </summary>
public partial class PaymentMeansBaseDto : IPaymentMeansBaseDto
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
