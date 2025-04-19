using System.Xml;
using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
///  Eine Gruppe von Informationselementen, die Informationen über den Preis für die in der betreffenden
///  Rechnungsposition in Rechnung gestellten Waren und Dienstleistungen enthalten. BG-29
/// </summary>
public class XmlPrice
{
    /// <summary>
    /// Der Preis eines Postens, ohne Umsatzsteuer, nach Abzug des für diese Rechnungsposition geltenden Rabatts.
    /// </summary>
    [XmlElement("PriceAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-146")]
    public Amount PriceAmount { get; set; } = new();
    /// <summary>
    /// Die Anzahl von Einheiten, für die der Postenpreis gilt.
    /// </summary>
    [XmlElement("BaseQuantity", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-149")]
    public Quantity? PriceBaseQuantity { get; set; }
}