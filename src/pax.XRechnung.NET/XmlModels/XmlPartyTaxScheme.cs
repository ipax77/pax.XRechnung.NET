namespace pax.XRechnung.NET.XmlModels;

using pax.XRechnung.NET.Attributes;
using System.Xml.Serialization;

/// <summary>
/// PartyTaxScheme
/// </summary>
public class XmlPartyTaxScheme
{
    /// <summary>
    ///  Eine örtliche steuerrechtliche Kennung des Verkäufers (bestimmt durch dessen Adresse) oder ein Verweis auf 
    ///  seinen eingetragenen Steuerstatus. (Hier ist ggf. die Angabe „Steuerschuldnerschaft des Leistungsempfängers“ 
    ///  oder die USt-Befreiung des Rechnungsstellers einzutragen.)
    /// </summary>
    [XmlElement("RegistrationName", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-32")]
    public string? RegistrationName { get; set; }
    /// <summary>
    ///  Die Umsatzsteuer-Identifikationsnummer des Verkäufers. Verfügt der Verkäufer über eine solche, ist sie hier 
    ///  anzugeben, sofern nicht Angaben zum "SELLER TAX REPRESENTATIVE PARTY" (BG-11) gemacht werden.
    /// </summary>
    [SpecificationId("BT-31")]
    [XmlElement("CompanyID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string CompanyId { get; set; } = string.Empty;
    /// <summary>
    /// TaxScheme
    /// </summary>
    [XmlElement("TaxScheme", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlTaxScheme TaxScheme { get; set; } = new();
}