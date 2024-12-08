using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Represents a credit transfer payment method.
/// </summary>
public class XmlCreditTransfer
{
    /// <summary>
    /// Die Kennung des Kontos, auf das die Zahlung erfolgen soll: IBAN für Zahlungen im SEPA-Raum, Kontonummer
    /// oder IBAN im Falle von Auslandszahlungen.
    /// </summary>
    [XmlElement("ID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-84")]
    public Identifier Id { get; set; } = new();
    /// <summary>
    /// Name des Kontos bei einem Zahlungsdienstleister, auf das die Zahlung erfolgen soll. (z. B. Kontoinhaber)
    /// </summary>
    [XmlElement("Name", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-85")]
    public string? Name { get; set; }
    /// <summary>
    ///  Die Kennung des Konto führenden Zahlungsdienstleisters. Diese Kennung ergibt sich bei Zahlungen im SEPA 
    ///  Raum im Regelfall aus der IBAN.
    /// Für alle Auslandszahlungen,
    /// • außerhalb des SEPA-Raumes wird der Bank Identifier Code (BIC) benötigt. Dies wird durch Code 30 (Credit
    ///   transfer (non-SEPA) in "Payment means type code" (BT-81) gekennzeichnet.
    /// • innerhalb des SEPA-Raumes wird der Bank Identifier Code (BIC) nicht benötigt. Es reicht die Angabe der
    ///   IBAN. Dies wird durch Code 58 „SCT“ in "Payment means type code" (BT-81) gekennzeichnet. Ausgenommen
    ///   sind Zahlungen an Bankverbindungen z. B. in San Marino, Monaco, Schweiz, Saint Pierre und Miquelon: Hier
    ///   ist die Angabe des BIC zwingend erforderlich.
    /// </summary>
    [XmlElement("Identifier", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-86")]
    public Identifier? Identifier { get; set; }
}
