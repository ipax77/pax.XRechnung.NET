using System.Xml;
using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
///  Eine Gruppe von Informationselementen, die Informationen über die in Rechnung gestelltenWaren und 
///  Dienstleistungen enthalten. BG-31
/// </summary>
public class XmlItem
{
    /// <summary>
    /// Beschreibung des Postens. (Optional)
    /// </summary>
    [XmlElement("Description", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-154")]
    public string? Description { get; set; }
    /// <summary>
    /// Name des Postens. (Erforderlich)
    /// </summary>
    [XmlElement("Name", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-153")]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Käufer-Kennung des Postens. (Optional)
    /// </summary>
    [XmlElement("BuyersItemIdentification", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-156")]
    public XmlItemIdentification? BuyersIdentifier { get; set; }
    /// <summary>
    /// Verkäufer-Kennung des Postens. (Optional)
    /// </summary>
    [XmlElement("SellersItemIdentification", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-155")]
    public XmlItemIdentification? SellersIdentifier { get; set; }
    /// <summary>
    /// Standard-Kennung des Postens. (Optional)
    /// </summary>
    [XmlElement("StandardItemIdentification", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-157")]
    public XmlItemIdentification? StandardIdentifier { get; set; }

    /// <summary>
    /// Klassifizierungskennungen des Postens (0..*).
    /// </summary>
    [XmlElement("CatalogueItemIdentification", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BT-158")]
    public List<XmlItemIdentification> ClassificationIdentifiers { get; set; } = [];

    /// <summary>
    /// Ursprungsland des Postens. (Optional)
    /// </summary>
    [XmlElement("OriginCountry", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BT-159")]
    public XmlCountry? CountryOfOrigin { get; set; }
    /// <summary>
    /// Eine Gruppe von Informationselementen, die Informationen über die für die betreffende Rechnungsposition
    /// geltende Umsatzsteuer enthalten.
    /// </summary>
    [XmlElement("ClassifiedTaxCategory", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlTaxCategory ClassifiedTaxCategory { get; set; } = new();
    /// <summary>
    /// Liste von Attributen für die Unterposition (0..*).
    /// </summary>
    [XmlElement("AdditionalItemProperty", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-32")]
    public List<XmlItemAttributes> Attributes { get; set; } = [];
}

/// <summary>
/// ItemIdentification
/// </summary>
public class XmlItemIdentification
{
    /// <summary>
    /// Id
    /// </summary>
    [XmlElement("ID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Identifier Id { get; set; } = new();
    /// <summary>
    /// Id
    /// </summary>
    [XmlElement("ExtendedID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Identifier? ExtendedId { get; set; }
    /// <summary>
    /// Id
    /// </summary>
    [XmlElement("BarcodeSymbologyID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Identifier? BarcodeSymbologyId { get; set; }
}