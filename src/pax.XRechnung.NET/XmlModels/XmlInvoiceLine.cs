using System.Xml;
using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
///  Eine Gruppe von Informationselementen, die Informationen über einzelne Rechnungspositionen enthalten.
/// </summary>
public class XmlInvoiceLine
{
    /// <summary>
    /// Eindeutige Bezeichnung für die betreffende Rechnungsposition.
    /// </summary>
    [XmlElement("ID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-126")]
    public Identifier Id { get; set; } = new();

    /// <summary>
    /// Ein Textvermerk, der unstrukturierte Informationen enthält, die für die Rechnungsposition maßgeblich sind.
    /// </summary>
    [XmlElement("Note", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-127")]
    public string? Note { get; set; }

    /// <summary>
    /// Eine vom Verkäufer angegebene Kennung für ein Objekt, auf das sich die Rechnungsposition bezieht.
    /// </summary>
    [XmlElement("ObjectIdentifier", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-128")]
    public Identifier? ObjectIdentifier { get; set; }

    /// <summary>
    /// Die Menge zu dem in der betreffenden Zeile in Rechnung gestellten Einzelposten (Waren oder Dienstleistungen).
    /// </summary>
    [XmlElement("InvoicedQuantity", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-129")]
    public Quantity InvoicedQuantity { get; set; } = new();
    /// <summary>
    ///  Der Gesamtbetrag der Rechnungsposition. Dies ist der Betrag ohne Umsatzsteuer, aber einschließlich aller für 
    ///  die Rechnungsposition geltenden Nachlässe und Abgaben sowie sonstiger anfallender Steuern.
    /// </summary>
    [XmlElement("LineExtensionAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-131")]
    public Amount LineExtensionAmount { get; set; } = new();
    /// <summary>
    /// Eine vom Erwerber ausgegebene Kennung für eine referenzierte Position einer Bestellung/eines Auftrags.
    /// </summary>
    [XmlElement("ReferencedPurchaseOrderLineReference", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-132")]
    public string? ReferencedPurchaseOrderLineReference { get; set; }
    /// <summary>
    /// Ein Textwert, der angibt, an welcher Stelle die betreffenden Daten in den Finanzkonten des Erwerbers zu buchen sind.
    /// </summary>
    [XmlElement("BuyerAccountingReference", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-133")]
    public string? BuyerAccountingReference { get; set; }
    /// <summary>
    ///  Eine Gruppe von Informationselementen, die Informationen über die in Rechnung gestellten Waren und
    ///  Dienstleistungen enthalten.
    /// </summary>
    [XmlElement("Item", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-30")]
    public XmlItemInformation Item { get; set; } = new();
    /// <summary>
    /// Eine Gruppe von Informationselementen, die Informationen über die für die betreffende Rechnungsposition geltenden Nachlässe enthalten.
    /// </summary>
    [XmlElement("Allowances", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-27")]
    public List<XmlInvoiceLineAllowances> Allowances { get; set; } = [];

    /// <summary>
    /// Eine Gruppe von Informationselementen, die Informationen über die für die betreffende Rechnungsposition geltenden Zuschläge und Kosten enthalten.
    /// </summary>
    [XmlElement("Charges", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-28")]
    public List<XmlInvoiceLineCharges> Charges { get; set; } = [];

    ///// <summary>
    ///// Eine Gruppe von Informationselementen, die Informationen über den für die Rechnungsposition maßgeblichen Abrechnungszeitraum enthalten.
    ///// </summary>
    //[XmlElement("InvoicePeriod", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    //public XmlInvoiceLinePeriod? InvoicePeriod { get; set; }

    /// <summary>
    /// Eine Gruppe von Informationselementen, die Informationen über den Preis der betreffenden Rechnungsposition enthalten.
    /// </summary>
    [XmlElement("Price", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-29")]
    public XmlInvoiceLinePriceDetails PriceDetails { get; set; } = new();

    /// <summary>
    /// Weitere untergeordnete Rechnungszeilen.
    /// </summary>
    [XmlElement("InvoiceLine", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public List<XmlInvoiceLine> InvoiceLines { get; set; } = [];
}
