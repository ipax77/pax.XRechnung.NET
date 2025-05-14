namespace pax.XRechnung.NET.XmlModels;

using pax.XRechnung.NET.Attributes;
using System.Xml.Serialization;

/// <summary>
/// EndpointId
/// </summary>
public class XmlEndpointId
{
    /// <summary>
    /// Eine von einer offiziellen Registrierstelle ausgegebene Kennung, die den Verkäufer als Rechtsträger oder 
    /// juristische Person identifiziert.
    /// </summary>
    [XmlText]
    [SpecificationId("BT-34")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// schemeID
    /// </summary>
    [XmlAttribute("schemeID")]
    [SpecificationId("BT-34")]
    [CodeList("ICD")]
    public string? SchemeId { get; set; }

}