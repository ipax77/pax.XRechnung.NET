namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// Partial Base Invoice DTO
/// With one currency and one VAT for all amounts. Amounts are net only.
/// </summary>
public partial class InvoiceBaseDto
{
    /// <summary>
    /// Eine eindeutige Kennung der Rechnung, die diese im System des Verkäufers identifiziert.
    /// </summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// Das Datum, an dem die Rechnung ausgestellt wurde
    /// </summary>
    public DateTime IssueDate { get; set; }
    /// <summary>
    /// Die Währung, in der alle Rechnungsbeträge angegeben werden, ausgenommen ist der Umsatzsteuer-Gesamtbetrag,
    /// der in der Abrechnungswährung anzugeben ist
    /// </summary>
    public string DocumentCurrencyCode { get; set; } = "EUR";
    /// <summary>
    /// Invoice lines
    /// </summary>
    public List<InvoiceLineBaseDto> InvoiceLines { get; set; } = [];
}


/// <summary>
/// Invoice Line
/// </summary>
public partial class InvoiceLineBaseDto
{
    /// <summary>
    /// Eindeutige Bezeichnung für die betreffende Rechnungsposition.
    /// </summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// Ein Textvermerk, der unstrukturierte Informationen enthält, die für die Rechnungsposition maßgeblich sind.
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Die Menge zu dem in der betreffenden Zeile in Rechnung gestellten Einzelposten (Waren oder Dienstleistungen).
    /// </summary>
    public double Quantity { get; set; }
    /// <summary>
    /// Quantity unitCode UN_ECE_Recommendation_N_20_3
    /// </summary>
    public string QuantityCode { get; set; } = "HUR";
    /// <summary>
    /// net amount
    /// </summary>
    public double Amount { get; set; }

    // InvoicePeriod
    /// <summary>
    /// Start
    /// </summary>
    public DateTime? StartDate { get; set; }
    /// <summary>
    /// End
    /// </summary>
    public DateTime? EndDate { get; set; }

    // Item
    /// <summary>
    /// Beschreibung des Postens. (Optional)
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Name des Postens. (Erforderlich)
    /// </summary>
    public string Name { get; set; } = string.Empty;
}

