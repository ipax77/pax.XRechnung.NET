namespace pax.XRechnung.NET.XmlModels;

using pax.XRechnung.NET.Attributes;
using System.Xml.Serialization;

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
    /// <summary>
    ///  Die Umsatzsteuer-Identifikationsnummer des Verkäufers. Verfügt der Verkäufer über eine solche, ist sie hier 
    ///  anzugeben, sofern nicht Angaben zum "SELLER TAX REPRESENTATIVE PARTY" (BG-11) gemacht werden.
    /// </summary>
    [SpecificationId("BT-31")]
    [XmlElement("CompanyID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? CompanyId { get; set; }
    /// <summary>
    ///  Weitere rechtliche Informationen, die für den Verkäufer maßgeblich sind
    ///  e.g. „Kein Ausweis von Umsatzsteuer, da Kleinunternehmer gemäß § 19UStG“
    /// </summary>
    [XmlElement("CompanyLegalForm", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-33")]
    public string? CompanyLegalForm { get; set; }

}
