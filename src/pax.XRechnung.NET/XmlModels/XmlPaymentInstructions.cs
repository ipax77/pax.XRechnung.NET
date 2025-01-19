using System.Xml.Serialization;
using pax.XRechnung.NET.Attributes;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// Represents payment instructions for the invoice.
/// </summary>
public class XmlPaymentInstructions
{
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
    [XmlElement("PaymentMeansText", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-82")]
    public string? PaymentMeansText { get; set; }

    /// <summary>
    /// Text for linking payment to the issued invoice.
    /// </summary>
    [XmlElement("AliasName", Namespace = XmlInvoiceWriter.CommonBasicComponents)]
    [SpecificationId("BT-83")]
    public string? RemittanceInformation { get; set; }

    /// <summary>
    /// Information about bank accounts for credit transfers.
    /// </summary>
    [XmlElement("PayeeFinancialAccount", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-17")]
    public List<XmlCreditTransfer> PayeeFinancialAccount { get; set; } = [];

    /// <summary>
    /// Information about the payment card used.
    /// </summary>
    [XmlElement("PaymentCardInformation", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-18")]
    public XmlPaymentCardInformation? PaymentCardInformation { get; set; }

    /// <summary>
    /// Information about direct debit.
    /// </summary>
    [XmlElement("DirectDebit", Namespace = XmlInvoiceWriter.CommonAggregateComponents)]
    [SpecificationId("BG-19")]
    public XmlDirectDebit? DirectDebit { get; set; }
}
