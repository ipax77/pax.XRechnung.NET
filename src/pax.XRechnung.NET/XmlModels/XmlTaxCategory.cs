using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// TaxCategory
/// </summary>
public class XmlTaxCategory
{
    /// <summary>
    /// Der Code der für den in Rechnung gestellten Posten geltenden Umsatzsteuerkategorie.
    /// </summary>
    [XmlElement("ID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-118")]
    public Identifier Id { get; set; } = new();

    /// <summary>
    /// Der Prozentsatz der Umsatzsteuer, der für den in Rechnung gestellten Posten gilt
    /// </summary>
    [XmlElement("Percent", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-119")]
    public decimal Percent { get; set; } = 19;
        /// <summary>
    /// Exemption Reason Code
    /// </summary>
    [XmlElement("TaxExemptionReasonCode", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Code? ExemptionReasonCode { get; set; }
    /// <summary>
    /// Exemption Reason (e.g. „Kein Ausweis von Umsatzsteuer, da Kleinunternehmer gemäß § 19UStG“)
    /// </summary>
    [XmlElement("TaxExemptionReason", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-120")]
    public string? ExemptionReason { get; set; } 
    /// <summary>
    /// TaxScheme
    /// </summary>
    [XmlElement("TaxScheme", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlTaxScheme TaxScheme { get; set; } = new();
}
