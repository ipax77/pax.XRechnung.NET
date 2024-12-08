namespace pax.XRechnung.NET.XmlModels;

using System.Xml.Serialization;

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
public class XmlSeller : XmlInvoiceParticipant
{
    /// <summary>
    /// Steuerliche Kennung des Verkäufers (optional).
    /// </summary>
    [XmlElement("TaxRegistrationIdentifier", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Identifier? TaxRegistrationIdentifier { get; set; }

    /// <summary>
    /// Kontaktinformationen des Verkäufers (erforderlich).
    /// </summary>
    [XmlElement("Contact", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlContact Contact { get; set; } = new();
}
