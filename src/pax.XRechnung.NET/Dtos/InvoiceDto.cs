using System.ComponentModel.DataAnnotations;

namespace pax.XRechnung.NET.Dtos;

/// <summary>
/// Das Wurzelelement INVOICE
/// </summary>
public record InvoiceDto
{
    /// <summary>
    /// CustomizationID
    /// </summary>
    public string CustomizationId { get; set; } = "urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0";
    /// <summary>
    /// ProfileID
    /// </summary>
    public string ProfileId { get; set; } = "urn:fdc:peppol.eu:2017:poacc:billing:01:1.0";
    /// <summary>
    ///  Eine eindeutige Kennung der Rechnung, die diese im System des Verkäufers identifiziert.
    /// </summary>
    [Required]
    public required string Id { get; set; }
    /// <summary>
    /// Das Datum, an dem die Rechnung ausgestellt wurde
    /// </summary>
    [Required]
    public DateTime IssueDate { get; set; }
    /// <summary>
    /// Das Fälligkeitsdatum des Rechnungsbetrages
    /// </summary>
    public DateTime? DueDate { get; set; }
    /// <summary>
    /// Ein Code, der den Funktionstyp der Rechnung angibt.
    ///   Anmerkung: Der Rechnungstyp muss gemäß UNTDID 1001, spezifiziert werden.
    ///   Folgende Codes aus der Codeliste sollen verwendet werden:
    ///     * 326 (Partial invoice)
    ///     * 380 (Commercial invoice)
    ///     * 384 (Corrected invoice)
    ///     * 389 (Self-billed invoice)
    ///     * 381 (Credit note)
    ///     * 875 (Partial construction invoice)
    ///     * 876 (Partial final construction invoice)
    ///     * 877 (Final construction invoice)
    /// </summary>
    [Required]
    public string InvoiceTypeCode { get; set; } = "380";
    /// <summary>
    ///  Eine Gruppe von Informationselementen für rechnungsrelevante Erläuterungen mit Hinweisen auf den 
    ///  Rechnungsbetreff.
    /// </summary>
    public string? Note { get; set; }
    /// <summary>
    /// Die Währung, in der alle Rechnungsbeträge angegeben werden, ausgenommen ist der Umsatzsteuer-Gesamtbetrag,
    /// der in der Abrechnungswährung anzugeben ist
    /// </summary>
    [Required]
    public string DocumentCurrencyCode { get; set; } = string.Empty;
    /// <summary>
    /// Ein vom Erwerber zugewiesener und für interne Lenkungszwecke benutzter Bezeichner
    /// </summary>
    [Required]
    public string BuyerReference { get; set; } = string.Empty;
    /// <summary>
    ///  Eine Gruppe von Informationselementen mit Informationen über rechnungsbegründende Unterlagen, 
    ///  die Belege für die in der Rechnung gestellten Ansprüche enthalten.
    /// </summary>
    public AdditionalDocumentReferenceDto? AdditionalDocumentReference { get; set; }
    /// <summary>
    /// AccountingSupplierParty
    /// </summary>
    public SellerDto Seller { get; set; } = new();
    /// <summary>
    /// AccountingCustomerParty
    /// </summary>
    public BuyerDto Buyer { get; set; } = new();
    /// <summary>
    /// Eine Gruppe von Informationselementen, die Informationen darüber enthalten, wie die Zahlung erfolgen soll.
    /// </summary>
    public PaymentInstructionsDto PaymentMeans { get; set; } = new();
    /// <summary>
    /// Represents the VAT breakdown for different categories, rates, and exemptions.
    /// </summary>
    public VatBreakdownDto TaxTotal { get; set; } = new();
    /// <summary>
    /// Eine Gruppe von Informationselementen, die die monetären Gesamtbeträge der Rechnung enthalten.
    /// </summary>
    public DocumentTotalsDto LegalMonetaryTotal { get; set; } = new();
}

/// <summary>
///  Eine Gruppe von Informationselementen, die Informationen über einzelne Rechnungspositionen enthalten.
/// </summary>
public record InvoiceLineDto
{

}