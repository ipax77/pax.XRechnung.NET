using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Represents payment card information.
/// </summary>
public class XmlPaymentCardInformation
{
    /// <summary>
    /// Die Nummer der Kreditkarte, die für die Zahlung genutzt wurde.
    ///   Anmerkung: In Übereinstimmung mit den für Kreditkarten geltenden Sicherheitsstandards darf eine Rechnung
    ///              nicht die vollständige Kartennummer enthalten.
    /// </summary>
    [XmlElement("Number", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-87")]
    public string Number { get; set; } = string.Empty;
    /// <summary>
    ///  Name des Karteninhabers
    /// </summary>
    [XmlElement("Name", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-88")]
    public string? Name { get; set; }
}
