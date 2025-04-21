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
    /// UUID. Eine vom Verkäufer angegebene Kennung für ein Objekt, auf das sich die Rechnungsposition bezieht.
    /// </summary>
    [XmlElement("UUID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-128")]
    public string? ObjectIdentifier { get; set; }

    /// <summary>
    /// Ein Textvermerk, der unstrukturierte Informationen enthält, die für die Rechnungsposition maßgeblich sind.
    /// </summary>
    [XmlElement("Note", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-127")]
    public string? Note { get; set; }

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
    /// Ein Textwert, der angibt, an welcher Stelle die betreffenden Daten in den Finanzkonten des Erwerbers zu buchen sind.
    /// </summary>
    [XmlElement("AccountingCost", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-133")]
    public string? BuyerAccountingReference { get; set; }
    /// <summary>
    /// Eine Gruppe von Informationselementen, die Informationen über den für die Rechnungsposition maßgeblichen Abrechnungszeitraum enthalten.
    /// </summary>
    [XmlElement("InvoicePeriod", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public XmlPeriod? InvoicePeriod { get; set; }
    /// <summary>
    /// Eine vom Erwerber ausgegebene Kennung für eine referenzierte Position einer Bestellung/eines Auftrags.
    /// </summary>
    [XmlElement("OrderLineReference", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BT-132")]
    public XmlOrderLineReference? OrderLineReference { get; set; }

    /// <summary>
    /// Eine Gruppe von Informationselementen, die Informationen über die für die betreffende Rechnungsposition geltenden Nachlässe enthalten.
    /// </summary>
    [XmlElement("AllowanceCharge", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-27")]
    public List<XmlAllowanceCharge> Allowances { get; set; } = [];
    /// <summary>
    /// TaxTotal
    /// </summary>
    [XmlElement("TaxTotal", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-30")]
    public XmlTaxTotal? TaxTotal { get; set; }
    /// <summary>
    ///  Eine Gruppe von Informationselementen, die Informationen über die in Rechnung gestellten Waren und
    ///  Dienstleistungen enthalten.
    /// </summary>
    [XmlElement("Item", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-31")]
    public XmlItem Item { get; set; } = new();
    /// <summary>
    /// Eine Gruppe von Informationselementen, die Informationen über den Preis der betreffenden Rechnungsposition enthalten.
    /// </summary>
    [XmlElement("Price", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-29")]
    public XmlPrice PriceDetails { get; set; } = new();

    /// <summary>
    /// Weitere untergeordnete Rechnungszeilen.
    /// </summary>
    [XmlElement("SubInvoiceLine", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    public List<XmlInvoiceLine> InvoiceLines { get; set; } = [];
}
