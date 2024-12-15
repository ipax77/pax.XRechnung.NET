using System.Xml;
using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Eine Gruppe von Informationselementen für rechnungsrelevante Erläuterungen mit Hinweisen auf den Rechnungsbetreff.
/// </summary>
public class XmlInvoiceNote
{
    /// <summary>
    /// Der Betreff für den nachfolgenden Textvermerk zur Rechnung.
    /// </summary>
    [XmlElement("SubjectCode", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-21")]
    [CodeList("UNTDID_4451")]
    public string? SubjectCode { get; set; }
    /// <summary>
    /// Ein Textvermerk, der unstrukturierte Informationen enthält, die für die Rechnung als Ganzes maßgeblich sind. 
    /// Erforderlichenfalls können Angaben zur Aufbewahrungspflicht gem. § 14 Abs. 4 Nr. 9 UStG hier aufgenommen werden.
    ///   Anmerkung: Im Falle einer bereits fakturierten Rechnung kann hier z. B. der Grund der Korrektur angegeben 
    ///   werden.
    /// </summary>
    [XmlElement("Note", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-21")]
    public string Note { get; set; } = string.Empty;
}