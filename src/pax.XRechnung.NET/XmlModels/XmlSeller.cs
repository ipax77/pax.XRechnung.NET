namespace pax.XRechnung.NET.XmlModels;

using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

/// <summary>
/// AccountingSupplierParty
/// </summary>
public class XmlSellerParty
{
    /// <summary>
    /// Party
    /// </summary>
    [XmlElement("Party", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlSeller Party { get; set; } = new();
}

/// <summary>
/// Seller
/// </summary>
public class XmlSeller
{
    /// <summary>
    /// Website
    /// </summary>
    [XmlElement("WebsiteURI", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? Website { get; set; }
    /// <summary>
    /// LogoReferenceId
    /// </summary>
    [XmlElement("LogoReferenceID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? LogoReferenceId { get; set; }
    /// <summary>
    /// EndpointID
    /// </summary>
    [XmlElement("EndpointID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public XmlEndpointId EndpointId { get; set; } = new();
    /// <summary>
    ///  Eine von einer offiziellen Registrierstelle ausgegebene Kennung, die den Verkäufer als Rechtsträger oder 
    ///  juristische Person identifiziert.
    /// </summary>
    [XmlElement("IndustryClassificationCode", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-30")]
    [CodeList("ICD")]
    public Identifier? LegalRegistrationIdentifier { get; set; }
    /// <summary>
    /// Eine (i. d. R. vom Erwerber vergebene) Kennung des Verkäufers, wie z. B. die Kreditorennummer für das
    /// Mittelbewirtschaftungsverfahren oder die Lieferantennummer für das Bestellsystem.
    /// </summary>
    [XmlElement("PartyIdentification", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BT-29")]
    public List<Identifier> Identifiers { get; set; } = [];
    /// <summary>
    ///  Der vollständige Name, unter dem der Verkäufer im nationalen Register für juristische Personen oder als 
    ///  steuerpflichtige Person eingetragen ist oder anderweitig als Person(en) handelt (Firma).
    /// </summary>
    [XmlElement("PartyName", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BT-27")]
    public XmlPartyName PartyName { get; set; } = new();
    /// <summary>
    /// Weitere rechtliche Informationen, die für den Verkäufer maßgeblich sind (wie z. B. Grundkapital).
    /// </summary>
    [XmlElement("LegalInformation", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-33")]
    public string? LegalInformation { get; set; }
    /// <summary>
    /// Eine Gruppe von Informationselementen, die Informationen über die Verkäuferanschrift enthalten.
    /// </summary>
    [XmlElement("PostalAddress", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-5")]
    public XmlPostalAddress PostalAddress { get; set; } = new XmlPostalAddress();
    /// <summary>
    /// PartyTaxScheme
    /// </summary>
    [XmlElement("PartyTaxScheme", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlPartyTaxScheme? PartyTaxScheme { get; set; }
    /// <summary>
    /// Zusätzliche rechtliche Informationen über den Verkäufer
    /// </summary>
    [XmlElement("PartyLegalEntity", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlPartyLegalEntity PartyLegalEntity { get; set; } = new();
    /// <summary>
    /// Kontaktinformationen des Verkäufers (erforderlich).
    /// </summary>
    [XmlElement("Contact", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-6")]
    public XmlContact Contact { get; set; } = new();
}
