namespace pax.XRechnung.NET.XmlModels;

using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

/// <summary>
/// PartyIdentificationType
/// </summary>
public class XmlPartyIdentificationType
{
    /// <summary>
    /// Id
    /// </summary>
    [XmlElement("ID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-84")]
    public Identifier Id { get; set; } = new();
}