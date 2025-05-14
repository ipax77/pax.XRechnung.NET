using pax.XRechnung.NET.Attributes;
using System.Xml;
using System.Xml.Serialization;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
///  Eine Gruppe von Informationselementen mit Informationen über rechnungsbegründende Unterlagen,
///  die Belege für die in der Rechnung gestellten Ansprüche enthalten.
/// </summary>
public class XmlAdditionalDocumentReference
{
    /// <summary>
    /// Eine Kennung der rechnungsbegründenden Unterlage
    /// </summary>
    [XmlElement("ID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-122")]
    public Identifier Id { get; set; } = new();
    /// <summary>
    /// Eine Beschreibung der rechnungsbegründenden Unterlage
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-123")]
    public string? DocumentDescription { get; set; }
    /// <summary>
    ///  Ein als Binärobjekt eingebettetes Anhangsdokument
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BT-125")]
    public BinaryObject? Attachment { get; set; }
}