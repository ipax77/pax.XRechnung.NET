using System.Xml.Serialization;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// TaxScheme
/// </summary>
public class XmlTaxScheme
{
    /// <summary>
    /// ID
    /// </summary>
    [XmlElement("ID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Identifier Id { get; set; } = new();
}