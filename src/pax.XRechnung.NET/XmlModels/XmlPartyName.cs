namespace pax.XRechnung.NET.XmlModels;

using pax.XRechnung.NET.Attributes;
using System.Xml.Serialization;

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
