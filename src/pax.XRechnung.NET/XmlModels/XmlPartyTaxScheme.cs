namespace pax.XRechnung.NET.XmlModels;

using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

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
    public Identifier? RegistrationName { get; set; }
    /// <summary>
    /// CompanyId
    /// </summary>
    [XmlElement("CompanyID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string CompanyId { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    [XmlElement("TaxScheme", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlTaxScheme TaxScheme { get; set; } = new();
}