
using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// BinaryObject ISO 15000-5:2014
/// </summary>
public class BinaryObject
{
    /// <summary>
    /// Der Inhalt der Datei im Binärformat (erforderlich).
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public EmbeddedDocumentBinaryObject EmbeddedDocumentBinaryObject { get; set; } = new();
    /// <summary>
    /// External Reference
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlExternalReference? ExternalReference { get; set; }
}

/// <summary>
/// EmbeddedDocumentBinaryObject
/// </summary>
public class EmbeddedDocumentBinaryObject
{
    /// <summary>
    /// MimeCode
    /// </summary>
    [XmlAttribute("mimeCode")]
    public string MimeCode { get; set; } = "application/pdf";
    /// <summary>
    /// FileName
    /// </summary>
    [XmlAttribute("filename")]
    public string FileName { get; set; } = string.Empty;
    /// <summary>
    /// Binary content
    /// </summary>
    [XmlText]
    public string Content { get; set; } = string.Empty;

}

/// <summary>
/// External Reference
/// </summary>
public class XmlExternalReference
{
    /// <summary>
    /// URI
    /// </summary>
    [XmlElement("URI", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
#pragma warning disable CA1056 // URI-like properties should not be strings
    public string? Uri { get; set; }
#pragma warning restore CA1056 // URI-like properties should not be strings
    /// <summary>
    /// DocumentHash
    /// </summary>
    [XmlElement("DocumentHash", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? DocumentHash { get; set; }
    /// <summary>
    /// HashAlgorithmMethod
    /// </summary>
    [XmlElement("HashAlgorithmMethod", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? HashAlgorithmMethod { get; set; }
    /// <summary>
    /// ExpiryDate
    /// </summary>
    [XmlElement("ExpiryDate", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public XmlDate? ExpiryDate { get; set; }
    /// <summary>
    /// ExpiryTime
    /// </summary>
    [XmlElement("ExpiryTime", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public XmlTime? ExpiryTime { get; set; }
    /// <summary>
    /// MimeCode
    /// </summary>
    [XmlElement("MimeCode", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? MimeCode { get; set; }
    /// <summary>
    /// FormatCode
    /// </summary>
    [XmlElement("FormatCode", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? FormatCode { get; set; }
    /// <summary>
    /// EncodingCode
    /// </summary>
    [XmlElement("EncodingCode", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? EncodingCode { get; set; }
    /// <summary>
    /// CharacterSetCode
    /// </summary>
    [XmlElement("CharacterSetCode", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? CharacterSetCode { get; set; }
    /// <summary>
    /// FileName
    /// </summary>
    [XmlElement("FileName", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string? FileName { get; set; }
    /// <summary>
    /// Description
    /// </summary>
    [XmlElement("Description", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public List<string>? Description { get; set; }
}