using System.Xml;
using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Eine Gruppe von Informationselementen, die Informationen über Kosten, Zuschläge, und Steuern – ausgenommen die 
/// Umsatzsteuer – enthalten, die für die jeweilige Rechnungsposition gelten.
/// </summary>
public class XmlInvoiceLineCharges
{
    /// <summary>
    /// Betrag der Abgabe ohne Umsatzsteuer (erforderlich).
    /// </summary>
    [XmlElement("ChargeAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-141")]
    public Amount ChargeAmount { get; set; } = new();

    /// <summary>
    /// Grundbetrag, der für die Berechnung des Abgabenbetrags verwendet werden kann (optional).
    /// </summary>
    [XmlElement("ChargeBaseAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-142")]
    public Amount? ChargeBaseAmount { get; set; }

    /// <summary>
    /// Prozentsatz für die Berechnung des Abgabenbetrags (optional).
    /// </summary>
    [XmlElement("ChargePercentage", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-143")]
    public decimal? ChargePercentage { get; set; }

    /// <summary>
    /// Textbeschreibung des Grundes für die Abgabe (optional).
    /// </summary>
    [XmlElement("ChargeReason", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-144")]
    public string? ChargeReason { get; set; }

    /// <summary>
    /// Code für den Grund der Abgabe (optional).
    /// </summary>
    [XmlElement("ChargeReasonCode", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-145")]
    [CodeList("UNTDID_7161")]
    public string? ChargeReasonCode { get; set; }
}