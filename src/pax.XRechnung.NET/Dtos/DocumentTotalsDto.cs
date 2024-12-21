namespace pax.XRechnung.NET.Dtos;

/// <summary>
/// Eine Gruppe von Informationselementen, die die monetären Gesamtbeträge der Rechnung enthalten.
/// </summary>
public record DocumentTotalsDto
{
    /// <summary>
    /// Sum of all net amounts for invoice lines.
    /// </summary>
    public decimal LineExtensionAmount { get; set; }
    /// <summary>
    /// Total invoice amount without VAT.
    /// </summary>
    public decimal TaxExclusiveAmount { get; set; }
    /// <summary>
    /// Total invoice amount with VAT.
    /// </summary>
    public decimal TaxInclusiveAmount { get; set; }
    /// <summary>
    /// Total amount due for payment.
    /// </summary>
    public decimal PayableAmount { get; set; }
}