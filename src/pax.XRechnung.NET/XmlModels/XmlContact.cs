namespace pax.XRechnung.NET.XmlModels;

using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

/// <summary>
/// Untergruppe für Käufer-/Verkäuferkontakte.
/// </summary>
public class XmlContact
{
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
