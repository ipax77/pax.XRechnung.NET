
using System.Xml.Serialization;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Represents an Identifier with optional scheme details, based on ISO 15000-5:2014 Annex B.
/// </summary>
[XmlType("Identifier")]
public class Identifier
{
    /// <summary>
    /// The content of the identifier (required).
    /// Example: "abc:123-DEF"
    /// </summary>
    [XmlText]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// The identifier of the scheme (optional).
    /// Example: "GLN"
    /// </summary>
    [XmlAttribute("schemeID")]
    public string? SchemeIdentifier { get; set; }

    // /// <summary>
    // /// The version of the scheme (optional).
    // /// Example: "1.0"
    // /// </summary>
    // [XmlAttribute("schemeVersionID")]
    // public string? SchemeVersionIdentifier { get; set; }
}
