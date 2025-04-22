using System.Xml;
using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Das Wurzelelement INVOICE
/// </summary>
[XmlRoot(ElementName = "Invoice", Namespace = XmlInvoiceWriter.InvoiceSchema)]
public class XmlInvoice
{
    /// <summary>
    /// CustomizationID
    /// </summary>
    [XmlElement("CustomizationID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string CustomizationId { get; set; } = "urn:cen.eu:en16931:2017#compliant#urn:xeinkauf.de:kosit:xrechnung_3.0";
    /// <summary>
    /// ProfileID
    /// </summary>
    [XmlElement("ProfileID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string ProfileId { get; set; } = "urn:fdc:peppol.eu:2017:poacc:billing:01:1.0";
    /// <summary>
    ///  Eine eindeutige Kennung der Rechnung, die diese im System des Verkäufers identifiziert.
    /// </summary>
    [XmlElement("ID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-1")]
    public Identifier Id { get; set; } = new();
    /// <summary>
    /// Das Datum, an dem die Rechnung ausgestellt wurde
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-2")]
    public XmlDate IssueDate { get; set; } = new();
    /// <summary>
    /// Das Fälligkeitsdatum des Rechnungsbetrages
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-9")]
    public XmlDate? DueDate { get; set; }
    /// <summary>
    /// Ein Code, der den Funktionstyp der Rechnung angibt.
    ///   Anmerkung: Der Rechnungstyp muss gemäß UNTDID 1001, spezifiziert werden.
    ///   Folgende Codes aus der Codeliste sollen verwendet werden:
    ///     • 326 (Partial invoice)
    ///     • 380 (Commercial invoice)
    ///     • 384 (Corrected invoice)
    ///     • 389 (Self-billed invoice)
    ///     • 381 (Credit note)
    ///     • 875 (Partial construction invoice)
    ///     • 876 (Partial final construction invoice)
    ///     • 877 (Final construction invoice)
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-3")]
    [CodeList("UNTDID_1001")]
    public string InvoiceTypeCode { get; set; } = "380";
    /// <summary>
    ///  Eine Gruppe von Informationselementen für rechnungsrelevante Erläuterungen mit Hinweisen auf den 
    ///  Rechnungsbetreff.
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BG-1")]
    public string? Note { get; set; }
    /// <summary>
    /// Die Währung, in der alle Rechnungsbeträge angegeben werden, ausgenommen ist der Umsatzsteuer-Gesamtbetrag,
    /// der in der Abrechnungswährung anzugeben ist
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-5")]
    [CodeList("Currency_Codes")]
    public string DocumentCurrencyCode { get; set; } = string.Empty;
    /// <summary>
    /// Ein vom Erwerber zugewiesener und für interne Lenkungszwecke benutzter Bezeichner
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-10")]
    public string BuyerReference { get; set; } = string.Empty;

    /// <summary>
    ///  Eine Gruppe von Informationselementen mit Informationen über rechnungsbegründende Unterlagen, 
    ///  die Belege für die in der Rechnung gestellten Ansprüche enthalten.
    /// </summary>
    [XmlElement("AdditionalDocumentReference", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-24")]
    public List<XmlAdditionalDocumentReference> AdditionalDocumentReferences { get; set; } = [];
    /// <summary>
    /// Eine Gruppe von Informationselementen, die Informationen über den Verkäufer enthalten.
    /// </summary>
    [XmlElement("AccountingSupplierParty", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-4")]
    public XmlSellerParty SellerParty { get; set; } = new();
    /// <summary>
    /// Eine Gruppe von Informationselementen, die Informationen über den Erwerber enthalten.
    /// </summary>
    [XmlElement("AccountingCustomerParty", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-7")]
    public XmlBuyerParty BuyerParty { get; set; } = new();
    /// <summary>
    /// Eine Gruppe von Informationselementen, die Informationen darüber enthalten, wie die Zahlung erfolgen soll.
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-16")]
    public XmlPaymentMeans PaymentMeans { get; set; } = new();
    /// <summary>
    /// Payment terms
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlPaymentTerms? PaymentTerms { get; set; }
    /// <summary>
    /// Tax total
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-23")]
    public XmlVatBreakdown TaxTotal { get; set; } = new();
    /// <summary>
    /// Eine Gruppe von Informationselementen, die die monetären Gesamtbeträge der Rechnung enthalten.
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-22")]
    public XmlMonetaryTotal LegalMonetaryTotal { get; set; } = new();
    /// <summary>
    ///  Eine Gruppe von Informationselementen, die Informationen über einzelne Rechnungspositionen enthalten.
    /// </summary>
    [XmlElement("InvoiceLine", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-25")]
    public List<XmlInvoiceLine> InvoiceLines { get; set; } = [];
}

/// <summary>
/// Payment terms
/// </summary>
public class XmlPaymentTerms
{
    /// <summary>
    /// Note
    /// </summary>
    [XmlElement(Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public string Note { get; set; } = string.Empty;
}