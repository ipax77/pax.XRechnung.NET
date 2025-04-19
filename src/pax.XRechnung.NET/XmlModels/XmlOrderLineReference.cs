using System.Xml;
using System.Xml.Serialization;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// OrderLineReference
/// </summary>
public class XmlOrderLineReference
{
    /// <summary>
    /// LineId
    /// </summary>
    [XmlElement("LineId", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Identifier LineId { get; set; } = new();
    /// <summary>
    /// UUID
    /// </summary>
    [XmlElement("UUID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? Uuid { get; set; }
}