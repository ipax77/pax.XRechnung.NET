namespace pax.XRechnung.NET.XmlModels;

using System.Xml.Serialization;

/// <summary>
/// PartyTaxScheme
/// </summary>
public class XmlPartyTaxScheme
{
    /// <summary>
    /// CompanyId
    /// </summary>
    [XmlElement("CompanyID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string CompanyId { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    [XmlElement("TaxScheme", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlTaxScheme TaxScheme { get; set; } = new();
}