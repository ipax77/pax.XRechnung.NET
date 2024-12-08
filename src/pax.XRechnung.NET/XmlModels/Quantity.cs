
using System.Xml.Serialization;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Quantity ISO 15000-5:2014
/// </summary>
public class Quantity
{
    /// <summary>
    /// Content
    /// </summary>
    [XmlText]
    public decimal Value { get; set; }

    /// <summary>
    /// Maßeinheit
    /// </summary>
    [XmlAttribute("unitCode")]
    public string UnitCode { get; set; } = string.Empty;
}