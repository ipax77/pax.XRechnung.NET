using System.Xml;
using System.Xml.Serialization;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Represents the monetary totals of the invoice.
/// </summary>
public class XmlDocumentTotals
{
    /// <summary>
    /// Sum of all net amounts for invoice lines.
    /// </summary>
    [XmlElement("LineExtensionAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Amount LineExtensionAmount { get; set; } = new();

    /// <summary>
    /// Sum of all allowances at the document level.
    /// </summary>
    [XmlElement("AllowanceTotalAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Amount? SumOfAllowancesOnDocumentLevel { get; set; }

    /// <summary>
    /// Sum of all charges at the document level.
    /// </summary>
    [XmlElement("ChargeTotalAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Amount? ChargeTotalAmount { get; set; }

    /// <summary>
    /// Total invoice amount without VAT.
    /// </summary>
    [XmlElement("TaxExclusiveAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Amount TaxExclusiveAmount { get; set; } = new();

    /// <summary>
    /// Total VAT amount for the invoice.
    /// </summary>
    [XmlElement("TaxTotalAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Amount? InvoiceTotalVATAmount { get; set; }

    /// <summary>
    /// Total VAT amount in the accounting currency.
    /// </summary>
    [XmlElement("TaxTotalAmountInAccountingCurrency", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Amount? InvoiceTotalVATAmountInAccountingCurrency { get; set; }

    /// <summary>
    /// Total invoice amount with VAT.
    /// </summary>
    [XmlElement("TaxInclusiveAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Amount TaxInclusiveAmount { get; set; } = new();

    /// <summary>
    /// Amount already paid.
    /// </summary>
    [XmlElement("PrepaidAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Amount? PrepaidAmount { get; set; }

    /// <summary>
    /// Rounding amount for the invoice.
    /// </summary>
    [XmlElement("PayableRoundingAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Amount? PayableRoundingAmount { get; set; }

    /// <summary>
    /// Total amount due for payment.
    /// </summary>
    [XmlElement("PayableAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Amount PayableAmount { get; set; } = new();
}
