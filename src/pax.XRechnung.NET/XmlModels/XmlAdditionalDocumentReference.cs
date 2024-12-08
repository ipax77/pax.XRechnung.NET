using System.Xml;
using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

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
    /// Die Internetadresse bzw. URL (Uniform Resource Locator), unter der das externe Dokument verfügbar ist
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-124")]
    public string? DocumentLocation { get; set; }
    /// <summary>
    ///  Ein als Binärobjekt eingebettetes Anhangsdokument
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BT-125")]
    public BinaryObject? Attachment { get; set; }
}