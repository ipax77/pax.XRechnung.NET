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
    public XmlParty Party { get; set; } = new();
}
