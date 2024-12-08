
using System.Xml.Serialization;

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