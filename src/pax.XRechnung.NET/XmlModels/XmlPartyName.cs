namespace pax.XRechnung.NET.XmlModels;

using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

/// <summary>
/// PartyName
/// </summary>
public class XmlPartyName
{
    /// <summary>
    /// Der vollständige Name des Käufers/Verkäufers (erforderlich).
    /// </summary>
    [XmlElement("Name", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-44")]
    public string Name { get; set; } = string.Empty;
}
