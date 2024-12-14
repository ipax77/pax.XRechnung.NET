namespace pax.XRechnung.NET.XmlModels;

using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

/// <summary>
/// Untergruppe für die Verkäuferanschrift.
/// </summary>
public class XmlPostalAddress
{
    /// <summary>
    ///  Die Hauptzeile in einer Anschrift. Üblicherweise ist dies entweder Strasse und Hausnummer oder der 
    ///  Text „Postfach“ gefolgt von der Postfachnummer
    /// </summary>
    [XmlElement("StreetName", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-35")]
    public string? StreetName { get; set; }
    /// <summary>
    /// Eine zusätzliche Adresszeile in einer Anschrift, die verwendet werden kann, um weitere Einzelheiten 
    /// in Ergänzung zur Hauptzeile anzugeben.
    /// </summary>
    [XmlElement("AdditionalStreetName", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-36")]
    public string? AdditionalStreetName { get; set; }
    /// <summary>
    /// Eine zusätzliche Adresszeile in einer Anschrift, die verwendet werden kann, um weitere Einzelheiten 
    /// in Ergänzung zur Hauptzeile anzugeben.
    /// </summary>
    [XmlElement("BlockName", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-162")]
    public string? BlockName { get; set; }
    /// <summary>
    ///  Die Bezeichnung der Stadt oder Gemeinde, in der sich die Verkäuferanschrift befindet.
    /// </summary>
    [XmlElement("CityName", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-37")]
    public string City { get; set; } = string.Empty;
    /// <summary>
    ///  Die Postleitzahl
    /// </summary>
    [XmlElement("PostalZone", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-38")]
    public string PostCode { get; set; } = string.Empty;
    /// <summary>
    ///  Ein Code, mit dem das Land bezeichnet wird
    /// </summary>
    [XmlElement("Country", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BT-40")]
    public XmlCountry Country { get; set; } = new();
}
