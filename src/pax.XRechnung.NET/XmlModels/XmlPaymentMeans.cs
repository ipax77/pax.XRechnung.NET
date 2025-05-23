using pax.XRechnung.NET.Attributes;
using System.Xml.Serialization;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Represents payment instructions for the invoice.
/// </summary>
public class XmlPaymentMeans
{
    /// <summary>
    /// Eine Kennung des Zahlungsmittels
    /// </summary>
    [XmlElement("ID", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    public Identifier? Id { get; set; }
    /// <summary>
    /// Expected or used payment means as a code. UNTDID 4461
    /// </summary>
    [XmlElement("PaymentMeansCode", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-81")]
    [CodeList("UNTDID_4461")]
    public string PaymentMeansTypeCode { get; set; } = "30";

    /// <summary>
    /// Expected or used payment means as text.
    /// </summary>
    [XmlElement("InstructionNote", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-82")]
    public string? PaymentMeansText { get; set; }

    /// <summary>
    /// Information about the payment card used.
    /// </summary>
    [XmlElement("CardAccount", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-18")]
    public XmlCardAccount? PaymentCardInformation { get; set; }

    /// <summary>
    /// Information about bank accounts for credit transfers.
    /// </summary>
    [XmlElement("PayeeFinancialAccount", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-17")]
    public XmlFinancialAccount? PayeeFinancialAccount { get; set; }
}
