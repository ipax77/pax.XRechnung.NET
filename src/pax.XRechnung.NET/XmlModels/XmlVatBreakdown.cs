using pax.XRechnung.NET.Attributes;
using System.Xml;
using System.Xml.Serialization;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Represents the VAT breakdown for different categories, rates, and exemptions.
/// </summary>
public class XmlVatBreakdown
{
    /// <summary>
    /// Summe aller zu versteuernden Beträge, für die ein bestimmter Code der Umsatzsteuerkategorie und ein
    /// bestimmter Umsatzsteuersatz gelten (falls ein kategoriespezifischer Umsatzsteuersatz gilt).
    /// </summary>
    [XmlElement("TaxAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BG-116")]
    public Amount TaxAmount { get; set; } = new();
    /// <summary>
    ///  Der für die betreffende Umsatzsteuerkategorie zu entrichtende Gesamtbetrag.
    /// </summary>
    [XmlElement("TaxSubtotal", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-23")]
    public List<XmlTaxSubTotal> TaxSubTotal { get; set; } = [];
}
