using System.Xml;
using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
///  Eine Gruppe von Informationselementen, die Informationen über den Preis für die in der betreffenden
///  Rechnungsposition in Rechnung gestellten Waren und Dienstleistungen enthalten.
/// </summary>
public class XmlInvoiceLinePriceDetails
{
    /// <summary>
    /// Der Preis eines Postens, ohne Umsatzsteuer, nach Abzug des für diese Rechnungsposition geltenden Rabatts.
    /// </summary>
    [XmlElement("PriceAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-146")]
    public Amount PriceAmount { get; set; } = new();

    /// <summary>
    /// Der gesamte zur Berechnung des Netto-Postenpreises vom Brutto-Postenpreis subtrahierte Rabatt.
    /// </summary>
    [XmlElement("PriceDiscount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-147")]
    public Amount? PriceDiscount { get; set; }

    /// <summary>
    /// Der Postenpreis ohne Umsatzsteuer vor Abzug des Postenpreisrabatts.
    /// </summary>
    [XmlElement("GrossPrice")]
    [SpecificationId("BT-148")]
    public Amount? GrossPrice { get; set; }

    /// <summary>
    /// Die Anzahl von Einheiten, für die der Postenpreis gilt.
    /// </summary>
    [XmlElement("PriceBaseQuantity")]
    [SpecificationId("BT-149")]
    public Quantity? PriceBaseQuantity { get; set; }

    /// <summary>
    /// Der Code der zu Grunde gelegten Maßeinheit.
    /// </summary>
    [XmlElement("PriceBaseQuantityUnitOfMeasureCode")]
    [SpecificationId("BT-150")]
    [CodeList("UN_ECE_Recommendation_N_20")]
    public string? PriceBaseQuantityUnitOfMeasureCode { get; set; }
}