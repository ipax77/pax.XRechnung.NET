namespace pax.XRechnung.NET.Dtos;

/// <summary>
/// Represents payment instructions for the invoice.
/// </summary>
public record PaymentInstructionsDto
{
    /// <summary>
    /// Expected or used payment means as a code. UNTDID 4461
    /// </summary>
    public string PaymentMeansTypeCode { get; set; } = "30";
    /// <summary>
    /// Expected or used payment means as text.
    /// </summary>
    public string? PaymentMeansText { get; set; }
    /// <summary>
    /// Text for linking payment to the issued invoice.
    /// </summary>
    public string? RemittanceInformation { get; set; }
    /// <summary>
    /// Account Holder Name
    /// </summary>
    public string? AccountHolder { get; set; }
    /// <summary>
    /// Konto IBAN
    /// </summary>
    public string? IBAN { get; set; }
    /// <summary>
    /// Bank BIC
    /// </summary>
    public string? BIC { get; set; }
    /// <summary>
    /// Bank Name
    /// </summary>
    public string? BankName { get; set; }
}