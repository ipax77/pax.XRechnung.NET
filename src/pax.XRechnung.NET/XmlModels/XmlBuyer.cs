namespace pax.XRechnung.NET.XmlModels;

using System.Xml.Serialization;

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
public class XmlBuyer : XmlInvoiceParticipant
{
    /// <summary>
    /// Kontaktinformationen des Käufers (optional).
    /// </summary>
    [XmlElement("Contact", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlContact? Contact { get; set; }
}

