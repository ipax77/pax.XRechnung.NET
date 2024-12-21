namespace pax.XRechnung.NET.XmlModels;

using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

/// <summary>
/// AccountingCustomerParty
/// </summary>
public class XmlBuyerParty
{
    /// <summary>
    /// Party
    /// </summary>
    [XmlElement("Party", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlBuyer Party { get; set; } = new();
}

/// <summary>
/// Buyer Reference
/// </summary>
public class XmlBuyer
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
    [SpecificationId("BT-44")]
    public XmlPartyName PartyName { get; set; } = new();

    /// <summary>
    /// Ein Name, unter dem der Erwerber bekannt ist, sofern abweichend vom Namen des Erwerbers.
    /// </summary>
    [XmlElement("TradingName", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-45")]
    public string? TradingName { get; set; }

    /// <summary>
    ///  Eine (i. d. R. vom Verkäufer vergebene) Kennung des Erwerbers, wie z. B. die Debitorennummer für die
    ///  Buchhaltung oder die Kundennnummer für die Auftragsverwaltung.
    /// </summary>
    [XmlElement("Identifier", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BT-46")]
    [CodeList("ICD")]
    public Identifier? Identifier { get; set; }

    /// <summary>
    /// Rechtliche Registrierungskennung des Käufers/Verkäufers (optional).
    /// </summary>
    [XmlElement("LegalRegistrationIdentifier", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-47")]
    public Identifier? LegalRegistrationIdentifier { get; set; }
    /// <summary>
    /// Die Umsatzsteuer-Identifikationsnummer des Erwerbers.
    /// </summary>
    [XmlElement("VATIdentifier", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-48")]
    [CodeList("Country_Codes")]
    public Identifier? VATIdentifier { get; set; }
    /// <summary>
    /// Käufer-/Verkäuferanschrift (erforderlich).
    /// </summary>
    [XmlElement("PostalAddress", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-8")]
    public XmlPostalAddress PostalAddress { get; set; } = new XmlPostalAddress();
    /// <summary>
    /// Zusätzliche rechtliche Informationen über den Verkäufer
    /// </summary>
    [XmlElement("PartyLegalEntity", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlPartyLegalEntity PartyLegalEntity { get; set; } = new();
    /// <summary>
    /// Kontaktinformationen des Käufers (optional).
    /// </summary>
    [XmlElement("Contact", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-9")]
    public XmlContact? Contact { get; set; }
}

