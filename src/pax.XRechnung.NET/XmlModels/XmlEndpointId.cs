namespace pax.XRechnung.NET.XmlModels;

using System.Xml.Serialization;

/// <summary>
/// EndpointId
/// </summary>
public class XmlEndpointId
{
    /// <summary>
    /// E-Mail
    /// </summary>
    [XmlText]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// schemeID
    /// </summary>
    [XmlAttribute("schemeID")]
    public string? SchemeId { get; set; } = "EM";

}