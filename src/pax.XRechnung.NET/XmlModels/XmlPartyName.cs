namespace pax.XRechnung.NET.XmlModels;

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
    public string Name { get; set; } = string.Empty;
}
