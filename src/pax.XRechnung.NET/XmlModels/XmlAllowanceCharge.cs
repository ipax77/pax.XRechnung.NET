using System.Xml;
using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
///  Eine Gruppe von Informationselementen, die Informationen über die für die betreffende Rechnungsposition
///  geltenden Nachlässe enthalten.
/// </summary>
public class XmlAllowanceCharge
{
    /// <summary>
    /// Der Nachlassbetrag ohne Umsatzsteuer (erforderlich).
    /// </summary>
    [XmlElement("AllowanceAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-136")]
    public Amount AllowanceAmount { get; set; } = new();

    /// <summary>
    /// Der Grundbetrag für die Berechnung des Nachlassbetrags (optional).
    /// </summary>
    [XmlElement("AllowanceBaseAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-137")]
    public Amount? AllowanceBaseAmount { get; set; }

    /// <summary>
    /// Der Prozentsatz zur Berechnung des Nachlassbetrags (optional).
    /// </summary>
    [XmlElement("AllowancePercentage", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-138")]
    public decimal? AllowancePercentage { get; set; }

    /// <summary>
    /// Textbeschreibung des Grundes für den Nachlass (optional).
    /// </summary>
    [XmlElement("AllowanceReason", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-139")]
    public string? AllowanceReason { get; set; }

    /// <summary>
    /// Code für den Grund des Nachlasses (optional).
    /// </summary>
    [XmlElement("AllowanceReasonCode", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-140")]
    [CodeList("UNTDID_5189")]
    public string? AllowanceReasonCode { get; set; }
}