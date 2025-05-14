using pax.XRechnung.NET.Attributes;
using System.Xml;
using System.Xml.Serialization;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
///  Eine Gruppe von Informationselementen, die Informationen über die für die betreffende Rechnungsposition
///  geltenden Nachlässe enthalten.
/// </summary>
public class XmlAllowanceCharge
{
    /// <summary>
    /// Charge Indicator
    /// </summary>
    [XmlElement("ChargeIndicator", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public bool ChargeIndicator { get; set; }
    /// <summary>
    /// Code für den Grund des Nachlasses (optional).
    /// </summary>
    [XmlElement("AllowanceChargeReasonCode", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-140")]
    [CodeList("UNTDID_5189")]
    public string? AllowanceReasonCode { get; set; }
    /// <summary>
    /// Textbeschreibung des Grundes für den Nachlass (optional).
    /// </summary>
    [XmlElement("AllowanceChargeReason", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-139")]
    public string? AllowanceReason { get; set; }
    /// <summary>
    /// Der Prozentsatz zur Berechnung des Nachlassbetrags (optional).
    /// </summary>
    [XmlElement("MultiplierFactorNumeric", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-138")]
    public decimal? AllowancePercentage { get; set; }
    /// <summary>
    /// Der Nachlassbetrag ohne Umsatzsteuer (erforderlich).
    /// </summary>
    [XmlElement("Amount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-136")]
    public Amount AllowanceAmount { get; set; } = new();
    /// <summary>
    /// Der Grundbetrag für die Berechnung des Nachlassbetrags (optional).
    /// </summary>
    [XmlElement("BaseAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-137")]
    public Amount? AllowanceBaseAmount { get; set; }


}