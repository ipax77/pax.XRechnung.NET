namespace pax.XRechnung.NET.XmlModels;

using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

/// <summary>
/// Country
/// </summary>
public class XmlCountry
{
    /// <summary>
    /// IdentificationCode
    /// </summary>
    [XmlElement("IdentificationCode", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-40")]
    [CodeList("Country_Codes")]
    public string IdentificationCode { get; set; } = "DE";
    /// <summary>
    /// Name
    /// </summary>
    [XmlElement("Name", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? Name { get; set; }
}
