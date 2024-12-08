using System.Xml.Serialization;

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
    public Identifier Id { get; set; } = new();

    /// <summary>
    /// Der Prozentsatz der Umsatzsteuer, der für den in Rechnung gestellten Posten gilt
    /// </summary>
    [XmlElement("Percent", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public decimal Percent { get; set; } = 19;
    /// <summary>
    /// TaxScheme
    /// </summary>
    [XmlElement("TaxScheme", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlTaxScheme TaxScheme { get; set; } = new();
}

/// <summary>
/// TaxScheme
/// </summary>
public class XmlTaxScheme
{
    /// <summary>
    /// ID
    /// </summary>
    [XmlElement("ID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Identifier Id { get; set; } = new();
}