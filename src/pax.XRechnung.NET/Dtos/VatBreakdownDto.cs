namespace pax.XRechnung.NET.Dtos;

/// <summary>
/// Represents the VAT breakdown for different categories, rates, and exemptions.
/// </summary>
public record VatBreakdownDto
{
    /// <summary>
    /// Summe aller zu versteuernden Beträge, für die ein bestimmter Code der Umsatzsteuerkategorie und ein
    /// bestimmter Umsatzsteuersatz gelten (falls ein kategoriespezifischer Umsatzsteuersatz gilt).
    /// </summary>
    public decimal TaxAmount { get; set; }
    /// <summary>
    /// Summe aller zu versteuernden Beträge, für die ein bestimmter Code der Umsatzsteuerkategorie und ein
    /// bestimmter Umsatzsteuersatz gelten (falls ein kategoriespezifischer Umsatzsteuersatz gilt).
    /// </summary>
    public decimal TaxableAmount { get; set; }
    /// <summary>
    /// Codierte Bezeichnung einer Umsatzsteuerkategorie.
    ///   Anmerkung: Folgende Codes aus der Codeliste UNTDID 5305 müssen verwendet werden:
    ///   • S (Standard rate)
    ///   • Z (Zero rated goods)
    ///   • E (Exempt from tax)
    ///   • AE (VAT Reverse Charge)
    ///   • K (VAT exempt for EEA intra-community supply of goods and services)
    ///   • G (Free export item, tax not charged)
    ///   • O (Services outside scope of tax)
    ///   • L (Canary Islands general indirect tax)
    ///   • M (Tax for production, services and importation in Ceuta and Melilla)
    /// </summary>
    /// <summary>
    ///  In Textform angegebener Grund für die Ausnahme des Betrages von der Umsatzsteuerpflicht.
    ///  Sofern die Umsatzsteuerkategorie AE für die Rechnung gilt, ist hier der Text
    /// „Umkehrung der Steuerschuldnerschaft“ oder der entsprechende Normtext in der für die Rechnung gewählten
    /// Sprache anzugeben.
    /// </summary>
    public string? VatExemptionReasonText { get; set; }

    /// <summary>
    ///  Ein Code für den Grund für die Ausnahme des Betrages von der Umsatzsteuerpflicht. Die Codeliste VATEX 
    ///  „VATexemption reason code list“ wird von der Connecting Europe Facility gepflegt und herausgegeben.
    /// </summary>
    public string? VatExemptionReasonCode { get; set; }
    /// <summary>
    /// Der Code der für den in Rechnung gestellten Posten geltenden Umsatzsteuerkategorie.
    /// </summary>
    public string TaxCategoryId { get; set; } = "S";
    /// <summary>
    /// Der Prozentsatz der Umsatzsteuer, der für den in Rechnung gestellten Posten gilt
    /// </summary>
    public decimal Percent { get; set; } = 19;
    /// <summary>
    /// TaxScheme
    /// </summary>
    public string TaxScheme { get; set; } = "VAT";
}