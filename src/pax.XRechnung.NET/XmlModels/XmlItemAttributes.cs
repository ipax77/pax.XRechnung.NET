using System.Xml;
using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Eine Gruppe von Informationselementen, die Informationen über die Eigenschaften der inRechnung gestellten Waren
/// und Dienstleistungen enthalten.
/// </summary>
public class XmlItemAttributes
{
    /// <summary>
    ///  Der Name der Eigenschaft des Postens, wie z. B. „Farbe“.
    /// </summary>
    [XmlElement("Name", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-160")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Der Wert der Eigenschaft des Postens, wie z. B. „rot“.
    /// </summary>
    [XmlElement("Value", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-161")]
    public string Value { get; set; } = string.Empty;
}