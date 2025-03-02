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
    public XmlParty Party { get; set; } = new();
}


