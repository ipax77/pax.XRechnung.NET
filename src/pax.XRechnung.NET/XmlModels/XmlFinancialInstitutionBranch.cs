using System.Xml.Serialization;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Financial Institution Branch
/// </summary>
public class XmlFinancialInstitutionBranch
{
    /// <summary>
    /// BIC
    /// </summary>
    [XmlElement("ID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Identifier Id { get; set; } = new();
    /// <summary>
    /// Name der Bank
    /// </summary>
    [XmlElement("Name", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? Name { get; set; }
}