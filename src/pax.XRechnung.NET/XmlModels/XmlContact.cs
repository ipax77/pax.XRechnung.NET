namespace pax.XRechnung.NET.XmlModels;

using pax.XRechnung.NET.Attributes;
using System.Xml.Serialization;

/// <summary>
/// Untergruppe für Käufer-/Verkäuferkontakte.
/// </summary>
public class XmlContact
{
    /// <summary>
    /// Eine Kennung des Kontakts
    /// </summary>
    [XmlElement("ID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-122")]
    public Identifier? Id { get; set; }
    /// <summary>
    /// Angaben zu Ansprechpartner oder Kontaktstelle (wie z. B. Name einer Person, Abteilungs- oder Bürobezeichnung)
    /// </summary>
    [XmlElement("Name", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-41")]
    public string? Name { get; set; }
    /// <summary>
    ///  Telefonnummer des Ansprechpartners oder der Kontaktstelle
    /// </summary>
    [XmlElement("Telephone", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-42")]
    public string? Telephone { get; set; }
    /// <summary>
    /// Eine E-Mail-Adresse des Ansprechpartners oder der Kontaktstelle
    /// </summary>
    [XmlElement("ElectronicMail", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-43")]
    public string? Email { get; set; } = string.Empty;
}
