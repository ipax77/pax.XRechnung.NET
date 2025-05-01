namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// IInvoiceBaseDto used for mapping
/// </summary>
public interface IInvoiceBaseDto
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    string GlobalTaxCategory { get; set; }
    string GlobalTaxScheme { get; set; }
    double GlobalTax { get; set; }
    string Id { get; set; }
    DateTime IssueDate { get; set; }
    DateTime? DueDate { get; set; }
    string InvoiceTypeCode { get; set; }
    string? Note { get; set; }
    string DocumentCurrencyCode { get; set; }
    string BuyerReference { get; set; }
    List<IDocumentReferenceBaseDto> AdditionalDocumentReferences { get; set; }
    IPartyBaseDto SellerParty { get; set; }
    IPartyBaseDto BuyerParty { get; set; }
    IPaymentMeansBaseDto PaymentMeans { get; set; }
    string PaymentMeansTypeCode { get; set; }
    string PaymentTermsNote { get; set; }
    double PayableAmount { get; set; }
    List<IInvoiceLineBaseDto> InvoiceLines { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

/// <summary>
/// Base DTO for simplified XRechnung invoices.
/// Supports a single currency and a single VAT category/scheme for all lines.
/// Amounts are expected to be net (exclusive of tax).
/// </summary>
public partial class InvoiceBaseDto : IInvoiceBaseDto
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
    /// Ein Code, der den Funktionstyp der Rechnung angibt.
    ///   Anmerkung: Der Rechnungstyp muss gemäß UNTDID 1001, spezifiziert werden.
    ///   Folgende Codes aus der Codeliste sollen verwendet werden:
    ///     • 326 (Partial invoice)
    ///     • 380 (Commercial invoice)
    ///     • 384 (Corrected invoice)
    ///     • 389 (Self-billed invoice)
    ///     • 381 (Credit note)
    ///     • 875 (Partial construction invoice)
    ///     • 876 (Partial final construction invoice)
    ///     • 877 (Final construction invoice)
    /// </summary>
    public string InvoiceTypeCode { get; set; } = "380";
    /// <summary>
    ///  Eine Gruppe von Informationselementen für rechnungsrelevante Erläuterungen mit Hinweisen auf den 
    ///  Rechnungsbetreff.
    /// </summary>
    public string? Note { get; set; }
    /// <summary>
    /// ISO 4217 currency code (e.g., "EUR"). Used on all amounts
    /// </summary>
    public string DocumentCurrencyCode { get; set; } = "EUR";
    /// <summary>
    /// Ein vom Erwerber zugewiesener und für interne Lenkungszwecke benutzter Bezeichner
    /// </summary>
    public string BuyerReference { get; set; } = string.Empty;
    /// <summary>
    /// Additional documents attached to the invoice (e.g., contract, timesheet)
    /// </summary>
    public List<DocumentReferenceBaseDto> AdditionalDocumentReferences { get; set; } = [];
    /// <summary>
    /// Seller
    /// </summary>
    public PartyBaseDto SellerParty { get; set; } = new PartyBaseDto();
    /// <summary>
    /// Buyer
    /// </summary>
    public PartyBaseDto BuyerParty { get; set; } = new PartyBaseDto();
    /// <summary>
    /// Bank account info for payment
    /// </summary>
    public PaymentMeansBaseDto PaymentMeans { get; set; } = new PaymentMeansBaseDto();
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

    IPartyBaseDto IInvoiceBaseDto.SellerParty { get => SellerParty; set => SellerParty = (PartyBaseDto)value; }
    IPartyBaseDto IInvoiceBaseDto.BuyerParty { get => BuyerParty; set => BuyerParty = (PartyBaseDto)value; }
    IPaymentMeansBaseDto IInvoiceBaseDto.PaymentMeans { get => PaymentMeans; set => PaymentMeans = (PaymentMeansBaseDto)value; }
    List<IInvoiceLineBaseDto> IInvoiceBaseDto.InvoiceLines
    {
        get => InvoiceLines.Cast<IInvoiceLineBaseDto>().ToList();
        set => InvoiceLines = value.Cast<InvoiceLineBaseDto>().ToList();
    }
    List<IDocumentReferenceBaseDto> IInvoiceBaseDto.AdditionalDocumentReferences
    {
        get => AdditionalDocumentReferences.Cast<IDocumentReferenceBaseDto>().ToList();
        set => AdditionalDocumentReferences = value.Cast<DocumentReferenceBaseDto>().ToList();
    }
}
