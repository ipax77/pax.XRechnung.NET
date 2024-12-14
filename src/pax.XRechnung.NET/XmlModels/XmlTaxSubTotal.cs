using System.Xml;
using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
///  Eine Gruppe von Informationselementen, die Informationen über die Umsatzsteueraufschlüsselung 
///  nach verschiedenen Kategorien, Steuersätzen und Ausnahmegründen enthalten.
/// </summary>
public class XmlTaxSubTotal
{
    /// <summary>
    /// Summe aller zu versteuernden Beträge, für die ein bestimmter Code der Umsatzsteuerkategorie und ein
    /// bestimmter Umsatzsteuersatz gelten (falls ein kategoriespezifischer Umsatzsteuersatz gilt).
    /// </summary>
    [XmlElement("TaxableAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-116")]
    public Amount TaxableAmount { get; set; } = new();
    /// <summary>
    /// Der für die betreffende Umsatzsteuerkategorie zu entrichtende Gesamtbetrag.
    /// </summary>
    [XmlElement("TaxAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-117")]
    public Amount TaxAmount { get; set; } = new();
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
    [XmlElement("TaxCategory", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlTaxCategory TaxCategory { get; set; } = new();
    /// <summary>
    ///  In Textform angegebener Grund für die Ausnahme des Betrages von der Umsatzsteuerpflicht.
    ///  Sofern die Umsatzsteuerkategorie AE für die Rechnung gilt, ist hier der Text
    /// „Umkehrung der Steuerschuldnerschaft“ oder der entsprechende Normtext in der für die Rechnung gewählten
    /// Sprache anzugeben.
    /// </summary>
    [XmlElement("ExemptionReason", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-120")]
    public string? VatExemptionReasonText { get; set; }

    /// <summary>
    ///  Ein Code für den Grund für die Ausnahme des Betrages von der Umsatzsteuerpflicht. Die Codeliste VATEX 
    ///  „VATexemption reason code list“ wird von der Connecting Europe Facility gepflegt und herausgegeben.
    /// </summary>
    [XmlElement("ExemptionReasonCode", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-121")]
    [CodeList("VATEX")]
    public string? VatExemptionReasonCode { get; set; }
}