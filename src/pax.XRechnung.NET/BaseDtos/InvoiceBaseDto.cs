namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// Base DTO for simplified XRechnung invoices.
/// Supports a single currency and a single VAT category/scheme for all lines.
/// Amounts are expected to be net (exclusive of tax).
/// </summary>
public partial class InvoiceBaseDto
{
    /// <summary>
    /// Global tax category (e.g., "S" for Standard rate).
    /// Applied to all invoice lines.
    /// </summary>
    public string GlobalTaxCategory { get; set; } = "S";
    /// <summary>
    /// Global tax scheme (e.g., "VAT").
    /// Applied to all invoice lines.
    /// </summary>
    public string GlobalTaxScheme { get; set; } = "VAT";
    /// <summary>
    /// Global tax used for all lines
    /// </summary>
    public double GlobalTax { get; set; } = 19.0;
    /// <summary>
    /// Eine eindeutige Kennung der Rechnung, die diese im System des Verkäufers identifiziert.
    /// </summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// Das Datum, an dem die Rechnung ausgestellt wurde
    /// </summary>
    public DateTime IssueDate { get; set; }
    /// <summary>
    /// das Fälligkeitsdatum des Rechnungsbetrages
    /// </summary>
    public DateTime? DueDate { get; set; }
    /// <summary>
    /// ISO 4217 currency code (e.g., "EUR"). Used on all amounts
    /// </summary>
    public string DocumentCurrencyCode { get; set; } = "EUR";
    /// <summary>
    /// Ein vom Erwerber zugewiesener und für interne Lenkungszwecke benutzter Bezeichner
    /// </summary>
    public string BuyerReference { get; set; } = string.Empty;
    /// <summary>
    /// Seller
    /// </summary>
    public PartyBaseDto SellerParty { get; set; } = new();
    /// <summary>
    /// Buyer
    /// </summary>
    public PartyBaseDto BuyerParty { get; set; } = new();
    /// <summary>
    /// Bank account info for payment
    /// </summary>
    public PaymentMeansBaseDto PaymentMeans { get; set; } = new();
    /// <summary>
    /// Payment type code, e.g. "30"
    /// </summary>
    public string PaymentMeansTypeCode { get; set; } = "30";
    /// <summary>
    /// Payment terms note
    /// </summary>
    public string PaymentTermsNote { get; set; } = string.Empty;
    /// <summary>
    /// Final payable amount
    /// </summary>
    public double PayableAmount { get; set; }
    /// <summary>
    /// Invoice lines
    /// </summary>
    public List<InvoiceLineBaseDto> InvoiceLines { get; set; } = [];
}
