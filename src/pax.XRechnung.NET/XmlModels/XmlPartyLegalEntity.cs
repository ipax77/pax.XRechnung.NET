namespace pax.XRechnung.NET.XmlModels;

using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

/// <summary>
/// PartyLegalEntity
/// </summary>
public class XmlPartyLegalEntity
{
    /// <summary>
    /// RegistrationName
    /// </summary>
    [XmlElement("RegistrationName", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-30")]
    public string RegistrationName { get; set; } = string.Empty;
}
