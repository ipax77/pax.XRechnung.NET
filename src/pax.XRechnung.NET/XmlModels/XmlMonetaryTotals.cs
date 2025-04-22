using System.Xml;
using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Represents the monetary totals of the invoice. BG-22
/// </summary>
public class XmlMonetaryTotal
{
    /// <summary>
    /// Sum of all net amounts for invoice lines.
    /// </summary>
    [XmlElement("LineExtensionAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BG-106")]
    public Amount LineExtensionAmount { get; set; } = new();

    /// <summary>
    /// Total invoice amount without VAT.
    /// </summary>
    [XmlElement("TaxExclusiveAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BG-109")]
    public Amount TaxExclusiveAmount { get; set; } = new();

    /// <summary>
    /// Total invoice amount with VAT.
    /// </summary>
    [XmlElement("TaxInclusiveAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Amount TaxInclusiveAmount { get; set; } = new();

    /// <summary>
    /// Sum of all allowances at the document level.
    /// </summary>
    [XmlElement("AllowanceTotalAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BG-107")]
    public Amount? SumOfAllowancesOnDocumentLevel { get; set; }

    /// <summary>
    /// Sum of all charges at the document level.
    /// </summary>
    [XmlElement("ChargeTotalAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BG-108")]
    public Amount? ChargeTotalAmount { get; set; }

    /// <summary>
    /// Amount already paid.
    /// </summary>
    [XmlElement("PrepaidAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BG-113")]
    public Amount? PrepaidAmount { get; set; }

    /// <summary>
    /// Der Betrag, um den der Rechnungsbetrag gerundet wurde.
    /// </summary>
    [XmlElement("PayableRoundingAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BG-114")]
    public Amount? PayableRoundingAmount { get; set; }

    /// <summary>
    /// Total amount due for payment.
    /// </summary>
    [XmlElement("PayableAmount", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BG-115")]
    public Amount PayableAmount { get; set; } = new();
}
