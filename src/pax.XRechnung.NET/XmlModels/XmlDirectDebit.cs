using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Represents direct debit information.
/// </summary>
public class XmlDirectDebit
{
    /// <summary>
    /// Eindeutige Kennung, die vom Zahlungsempfänger zur Referenzierung der Einzugsermächtigung zugewiesen wird 
    /// (Mandatsreferenznummer).
    /// </summary>
    [XmlElement("MandateIdentifier", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-89")]
    public Identifier MandateIdentifier { get; set; } = new();
    /// <summary>
    ///  Die eindeutige Kennung des Verkäufers (Seller) oder des Zahlungsempfängers (Payee), 
    ///  um am SEPA-Lastschriftverfahren teilnehmen zu können (Gläubiger-ID).
    /// </summary>
    [XmlElement("BankIdentifier", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-90")]
    public Identifier BankIdentifier { get; set; } = new();
    /// <summary>
    /// Die Kennung des Kontos, von dem die Lastschrift erfolgen soll: IBAN für Zahlungen im SEPA-Raum, 
    /// Kontonummer oder IBAN im Falle von Auslandszahlungen.
    /// </summary>
    [XmlElement("AccountIdentifier", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-91")]
    public Identifier DebitedAccountIdentifier { get; set; } = new();
}
