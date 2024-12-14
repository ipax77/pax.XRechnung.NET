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
    /// TaxScheme
    /// </summary>
    [XmlElement("TaxScheme", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlTaxScheme TaxScheme { get; set; } = new();
}
