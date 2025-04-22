using System.Xml;
using System.Xml.Serialization;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Tax Total
/// </summary>
public class XmlTaxTotal
{
    /// <summary>
    /// Amount
    /// </summary>
    [XmlElement("TaxAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Amount TaxAmount { get; set; } = new();
    /// <summary>
    /// RoundingAmount
    /// </summary>
    [XmlElement("RoundingAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Amount? RoundingAmount { get; set; }
}