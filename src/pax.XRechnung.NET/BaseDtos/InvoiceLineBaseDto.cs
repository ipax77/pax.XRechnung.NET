namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// IInvoiceLineBaseDto  used for mapping
/// </summary>
public interface IInvoiceLineBaseDto
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    string Id { get; set; }
    string? Note { get; set; }
    double Quantity { get; set; }
    string QuantityCode { get; set; }
    double UnitPrice { get; set; }
    DateTime? StartDate { get; set; }
    DateTime? EndDate { get; set; }
    string? Description { get; set; }
    string Name { get; set; }
    double LineTotal { get; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

/// <summary>
/// Invoice Line
/// </summary>
public partial class InvoiceLineBaseDto : IInvoiceLineBaseDto
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
    /// Net price per unit.
    /// </summary>
    public double UnitPrice { get; set; }

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
    /// <summary>
    /// Total net amount for this line (Quantity × UnitPrice).
    /// </summary>
    public double LineTotal => Math.Round(Quantity * UnitPrice, 2);
}
