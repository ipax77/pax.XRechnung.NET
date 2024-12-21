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

    ///// <summary>
    /////  Eine Gruppe von Informationselementen mit Informationen �ber rechnungsbegr�ndende Unterlagen, 
    /////  die Belege f�r die in der Rechnung gestellten Anspr�che enthalten.
    ///// </summary>
    //public XmlAdditionalDocumentReference? AdditionalDocumentReference { get; set; }
    ///// <summary>
    ///// Eine Gruppe von Informationselementen, die Informationen �ber den Verk�ufer enthalten.
    ///// </summary>
    //public XmlSellerParty SellerParty { get; set; } = new();
    ///// <summary>
    ///// Eine Gruppe von Informationselementen, die Informationen �ber den Erwerber enthalten.
    ///// </summary>
    //public XmlBuyerParty BuyerParty { get; set; } = new();
    ///// <summary>
    ///// Eine Gruppe von Informationselementen, die Informationen dar�ber enthalten, wie die Zahlung erfolgen soll.
    ///// </summary>
    //public XmlPaymentInstructions PaymentMeans { get; set; } = new();
    ///// <summary>
    ///// 
    ///// </summary>
    //public XmlVatBreakdown TaxTotal { get; set; } = new();
    ///// <summary>
    ///// Eine Gruppe von Informationselementen, die die monet�ren Gesamtbetr�ge der Rechnung enthalten.
    ///// </summary>
    //public XmlDocumentTotals LegalMonetaryTotal { get; set; } = new();
    ///// <summary>
    /////  Eine Gruppe von Informationselementen, die Informationen �ber einzelne Rechnungspositionen enthalten.
    ///// </summary>
    //public List<XmlInvoiceLine> InvoiceLines { get; set; } = [];
}

