using System.Xml;
using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
///  Eine Gruppe von Informationselementen, die Informationen über die in Rechnung gestelltenWaren und 
///  Dienstleistungen enthalten.
/// </summary>
public class XmlItemInformation
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
    /// Verkäufer-Kennung des Postens. (Optional)
    /// </summary>
    [XmlElement("SellersIdentifier", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-155")]
    public Identifier? SellersIdentifier { get; set; }

    /// <summary>
    /// Käufer-Kennung des Postens. (Optional)
    /// </summary>
    [XmlElement("BuyersIdentifier", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-156")]
    public Identifier? BuyersIdentifier { get; set; }

    /// <summary>
    /// Standard-Kennung des Postens. (Optional)
    /// </summary>
    [XmlElement("StandardIdentifier", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-157")]
    public Identifier? StandardIdentifier { get; set; }

    /// <summary>
    /// Klassifizierungskennungen des Postens (0..*).
    /// </summary>
    [XmlElement("ClassificationIdentifier", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BT-158")]
    public List<Identifier> ClassificationIdentifiers { get; set; } = [];

    /// <summary>
    /// Ursprungsland des Postens. (Optional)
    /// </summary>
    [XmlElement("CountryOfOrigin", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-159")]
    [CodeList("Country_Codes")]
    public string? CountryOfOrigin { get; set; }
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
