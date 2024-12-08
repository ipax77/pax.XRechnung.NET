namespace pax.XRechnung.NET.XmlModels;

using System.Collections.Generic;
using System.Xml.Serialization;

/// <summary>
/// Buyer/Seller Participant
/// </summary>
[XmlInclude(typeof(XmlBuyer))]
[XmlInclude(typeof(XmlSeller))]
public class XmlInvoiceParticipant
{
    /// <summary>
    /// EndpointID
    /// </summary>
    [XmlElement("EndpointID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public XmlEndpointId EndpointId { get; set; } = new();
    /// <summary>
    /// PartyName
    /// </summary>
    [XmlElement("PartyName", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlPartyName PartyName { get; set; } = new();

    /// <summary>
    /// Handelsname des Käufers/Verkäufers (optional).
    /// </summary>
    [XmlElement("TradingName", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? TradingName { get; set; }

    /// <summary>
    /// Eine Liste von Käufer-/Verkäuferkennungen (optional, 0..*).
    /// </summary>
    [XmlElement("Identifier", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public List<Identifier> Identifiers { get; set; } = [];

    /// <summary>
    /// Rechtliche Registrierungskennung des Käufers/Verkäufers (optional).
    /// </summary>
    [XmlElement("LegalRegistrationIdentifier", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Identifier? LegalRegistrationIdentifier { get; set; }
    /// <summary>
    /// Umsatzsteuer-Identifikationsnummer des Käufers/Verkäufers (optional).
    /// </summary>
    [XmlElement("VATIdentifier", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Identifier? VATIdentifier { get; set; }
    /// <summary>
    /// Käufer-/Verkäuferanschrift (erforderlich).
    /// </summary>
    [XmlElement("PostalAddress", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
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
}
