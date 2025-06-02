// using System.Xml.Serialization;
// XmlSerializer serializer = new XmlSerializer(typeof(CrossIndustryInvoice));
// using (StringReader reader = new StringReader(xml))
// {
//    var test = (CrossIndustryInvoice)serializer.Deserialize(reader);
// }

using System.Xml.Serialization;

namespace pax.XRechnung.NET.ZUGFeRDModels;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
[XmlRoot(ElementName = "BusinessProcessSpecifiedDocumentContextParameter", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class BusinessProcessSpecifiedDocumentContextParameter
{

    [XmlElement(ElementName = "ID", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public string ID { get; set; } = "urn:fdc:peppol.eu:2017:poacc:billing:01:1.0";
}

[XmlRoot(ElementName = "GuidelineSpecifiedDocumentContextParameter", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class GuidelineSpecifiedDocumentContextParameter
{

    [XmlElement(ElementName = "ID", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public string ID { get; set; } = "urn:zugferd:2.1:EN16931";
}

[XmlRoot(ElementName = "ExchangedDocumentContext", Namespace = ZUGFeRDWriter.CrossIndustryInvoice)]
public class ExchangedDocumentContext
{

    [XmlElement(ElementName = "BusinessProcessSpecifiedDocumentContextParameter", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public BusinessProcessSpecifiedDocumentContextParameter BusinessProcessSpecifiedDocumentContextParameter { get; set; } = new();

    [XmlElement(ElementName = "GuidelineSpecifiedDocumentContextParameter", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public GuidelineSpecifiedDocumentContextParameter GuidelineSpecifiedDocumentContextParameter { get; set; } = new();
}

[XmlRoot(ElementName = "DateTimeString", Namespace = ZUGFeRDWriter.UnqualifiedDataType)]
public class DateTimeString
{

    [XmlAttribute(AttributeName = "format", Namespace = "")]
    public int Format { get; set; }

    [XmlText]
    public int Text { get; set; }
}

[XmlRoot(ElementName = "IssueDateTime", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class IssueDateTime
{

    [XmlElement(ElementName = "DateTimeString", Namespace = ZUGFeRDWriter.UnqualifiedDataType)]
    public DateTimeString DateTimeString { get; set; } = new();
}

[XmlRoot(ElementName = "ExchangedDocument", Namespace = ZUGFeRDWriter.CrossIndustryInvoice)]
public class ExchangedDocument
{

    [XmlElement(ElementName = "ID", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public int ID { get; set; }

    [XmlElement(ElementName = "TypeCode", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public int TypeCode { get; set; }

    [XmlElement(ElementName = "IssueDateTime", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public IssueDateTime IssueDateTime { get; set; } = new();
}

[XmlRoot(ElementName = "AssociatedDocumentLineDocument", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class AssociatedDocumentLineDocument
{

    [XmlElement(ElementName = "LineID", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public int LineID { get; set; }
}

[XmlRoot(ElementName = "SpecifiedTradeProduct", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class SpecifiedTradeProduct
{

    [XmlElement(ElementName = "Name", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public string Name { get; set; } = string.Empty;
}

[XmlRoot(ElementName = "NetPriceProductTradePrice", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class NetPriceProductTradePrice
{

    [XmlElement(ElementName = "ChargeAmount", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public double ChargeAmount { get; set; }
}

[XmlRoot(ElementName = "SpecifiedLineTradeAgreement", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class SpecifiedLineTradeAgreement
{

    [XmlElement(ElementName = "NetPriceProductTradePrice", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public NetPriceProductTradePrice NetPriceProductTradePrice { get; set; } = new();
}

[XmlRoot(ElementName = "BilledQuantity", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class BilledQuantity
{

    [XmlAttribute(AttributeName = "unitCode", Namespace = "")]
    public string UnitCode { get; set; } = string.Empty;

    [XmlText]
    public double Text { get; set; }
}

[XmlRoot(ElementName = "SpecifiedLineTradeDelivery", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class SpecifiedLineTradeDelivery
{

    [XmlElement(ElementName = "BilledQuantity", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public BilledQuantity BilledQuantity { get; set; } = new();
}

[XmlRoot(ElementName = "ApplicableTradeTax", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class ApplicableTradeTax
{

    [XmlElement(ElementName = "TypeCode", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public string TypeCode { get; set; } = string.Empty;

    [XmlElement(ElementName = "CategoryCode", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public string CategoryCode { get; set; } = string.Empty;

    [XmlElement(ElementName = "RateApplicablePercent", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public int RateApplicablePercent { get; set; }

    [XmlElement(ElementName = "CalculatedAmount", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public double CalculatedAmount { get; set; }

    [XmlElement(ElementName = "BasisAmount", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public double BasisAmount { get; set; }
}

[XmlRoot(ElementName = "SpecifiedTradeSettlementLineMonetarySummation", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class SpecifiedTradeSettlementLineMonetarySummation
{

    [XmlElement(ElementName = "LineTotalAmount", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public double LineTotalAmount { get; set; }
}

[XmlRoot(ElementName = "SpecifiedLineTradeSettlement", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class SpecifiedLineTradeSettlement
{

    [XmlElement(ElementName = "ApplicableTradeTax", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public ApplicableTradeTax ApplicableTradeTax { get; set; } = new();

    [XmlElement(ElementName = "SpecifiedTradeSettlementLineMonetarySummation", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public SpecifiedTradeSettlementLineMonetarySummation SpecifiedTradeSettlementLineMonetarySummation { get; set; } = new();
}

[XmlRoot(ElementName = "IncludedSupplyChainTradeLineItem", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class IncludedSupplyChainTradeLineItem
{

    [XmlElement(ElementName = "AssociatedDocumentLineDocument", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public AssociatedDocumentLineDocument AssociatedDocumentLineDocument { get; set; } = new();

    [XmlElement(ElementName = "SpecifiedTradeProduct", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public SpecifiedTradeProduct SpecifiedTradeProduct { get; set; } = new();

    [XmlElement(ElementName = "SpecifiedLineTradeAgreement", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public SpecifiedLineTradeAgreement SpecifiedLineTradeAgreement { get; set; } = new();

    [XmlElement(ElementName = "SpecifiedLineTradeDelivery", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public SpecifiedLineTradeDelivery SpecifiedLineTradeDelivery { get; set; } = new();

    [XmlElement(ElementName = "SpecifiedLineTradeSettlement", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public SpecifiedLineTradeSettlement SpecifiedLineTradeSettlement { get; set; } = new();
}

[XmlRoot(ElementName = "TelephoneUniversalCommunication", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class TelephoneUniversalCommunication
{

    [XmlElement(ElementName = "CompleteNumber", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public string CompleteNumber { get; set; } = string.Empty;
}

[XmlRoot(ElementName = "EmailURIUniversalCommunication", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class EmailURIUniversalCommunication
{

    [XmlElement(ElementName = "URIID", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public string URIID { get; set; } = string.Empty;
}

[XmlRoot(ElementName = "DefinedTradeContact", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class DefinedTradeContact
{

    [XmlElement(ElementName = "PersonName", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public string PersonName { get; set; } = string.Empty;

    [XmlElement(ElementName = "TelephoneUniversalCommunication", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public TelephoneUniversalCommunication TelephoneUniversalCommunication { get; set; } = new();

    [XmlElement(ElementName = "EmailURIUniversalCommunication", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public EmailURIUniversalCommunication EmailURIUniversalCommunication { get; set; } = new();
}

[XmlRoot(ElementName = "PostalTradeAddress", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class PostalTradeAddress
{

    [XmlElement(ElementName = "PostcodeCode", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public int PostcodeCode { get; set; }

    [XmlElement(ElementName = "CityName", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public string CityName { get; set; } = string.Empty;

    [XmlElement(ElementName = "CountryID", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public string CountryID { get; set; } = string.Empty;
}

[XmlRoot(ElementName = "URIID", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class URIID
{

    [XmlAttribute(AttributeName = "schemeID", Namespace = "")]
    public string SchemeID { get; set; } = string.Empty;

    [XmlText]
    public string Text { get; set; } = string.Empty;
}

[XmlRoot(ElementName = "URIUniversalCommunication", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class URIUniversalCommunication
{

    [XmlElement(ElementName = "URIID", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public URIID URIID { get; set; } = new();
}

[XmlRoot(ElementName = "ID", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class ID
{

    [XmlAttribute(AttributeName = "schemeID", Namespace = "")]
    public string SchemeID { get; set; } = string.Empty;

    [XmlText]
    public string Text { get; set; } = string.Empty;
}

[XmlRoot(ElementName = "SpecifiedTaxRegistration", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class SpecifiedTaxRegistration
{

    [XmlElement(ElementName = "ID", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public ID ID { get; set; } = new();
}

[XmlRoot(ElementName = "SellerTradeParty", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class SellerTradeParty
{

    [XmlElement(ElementName = "ID", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public double ID { get; set; }

    [XmlElement(ElementName = "Name", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public string Name { get; set; } = string.Empty;

    [XmlElement(ElementName = "DefinedTradeContact", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public DefinedTradeContact DefinedTradeContact { get; set; } = new();

    [XmlElement(ElementName = "PostalTradeAddress", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public PostalTradeAddress PostalTradeAddress { get; set; } = new();

    [XmlElement(ElementName = "URIUniversalCommunication", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public URIUniversalCommunication URIUniversalCommunication { get; set; } = new();

    [XmlElement(ElementName = "SpecifiedTaxRegistration", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public SpecifiedTaxRegistration SpecifiedTaxRegistration { get; set; } = new();
}

[XmlRoot(ElementName = "BuyerTradeParty", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class BuyerTradeParty
{

    [XmlElement(ElementName = "Name", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public string Name { get; set; } = string.Empty;

    [XmlElement(ElementName = "PostalTradeAddress", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public PostalTradeAddress PostalTradeAddress { get; set; } = new();

    [XmlElement(ElementName = "URIUniversalCommunication", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public URIUniversalCommunication URIUniversalCommunication { get; set; } = new();
}

[XmlRoot(ElementName = "ApplicableHeaderTradeAgreement", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class ApplicableHeaderTradeAgreement
{

    [XmlElement(ElementName = "BuyerReference", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public string BuyerReference { get; set; } = string.Empty;

    [XmlElement(ElementName = "SellerTradeParty", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public SellerTradeParty SellerTradeParty { get; set; } = new();

    [XmlElement(ElementName = "BuyerTradeParty", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public BuyerTradeParty BuyerTradeParty { get; set; } = new();
}

[XmlRoot(ElementName = "PayeePartyCreditorFinancialAccount", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class PayeePartyCreditorFinancialAccount
{

    [XmlElement(ElementName = "IBANID", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public string IBANID { get; set; } = string.Empty;
}

[XmlRoot(ElementName = "SpecifiedTradeSettlementPaymentMeans", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class SpecifiedTradeSettlementPaymentMeans
{

    [XmlElement(ElementName = "TypeCode", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public int TypeCode { get; set; }

    [XmlElement(ElementName = "PayeePartyCreditorFinancialAccount", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public PayeePartyCreditorFinancialAccount PayeePartyCreditorFinancialAccount { get; set; } = new();
}

[XmlRoot(ElementName = "DueDateDateTime", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class DueDateDateTime
{

    [XmlElement(ElementName = "DateTimeString", Namespace = ZUGFeRDWriter.UnqualifiedDataType)]
    public DateTimeString DateTimeString { get; set; } = new();
}

[XmlRoot(ElementName = "SpecifiedTradePaymentTerms", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class SpecifiedTradePaymentTerms
{

    [XmlElement(ElementName = "DueDateDateTime", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public DueDateDateTime DueDateDateTime { get; set; } = new();
}

[XmlRoot(ElementName = "TaxTotalAmount", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class TaxTotalAmount
{

    [XmlAttribute(AttributeName = "currencyID", Namespace = "")]
    public string CurrencyID { get; set; } = string.Empty;

    [XmlText]
    public double Text { get; set; }
}

[XmlRoot(ElementName = "SpecifiedTradeSettlementHeaderMonetarySummation", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class SpecifiedTradeSettlementHeaderMonetarySummation
{

    [XmlElement(ElementName = "LineTotalAmount", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public double LineTotalAmount { get; set; }

    [XmlElement(ElementName = "TaxBasisTotalAmount", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public double TaxBasisTotalAmount { get; set; }

    [XmlElement(ElementName = "TaxTotalAmount", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public TaxTotalAmount TaxTotalAmount { get; set; } = new();

    [XmlElement(ElementName = "GrandTotalAmount", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public double GrandTotalAmount { get; set; }

    [XmlElement(ElementName = "DuePayableAmount", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public double DuePayableAmount { get; set; }
}

[XmlRoot(ElementName = "ApplicableHeaderTradeSettlement", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
public class ApplicableHeaderTradeSettlement
{

    [XmlElement(ElementName = "InvoiceCurrencyCode", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public string InvoiceCurrencyCode { get; set; } = string.Empty;

    [XmlElement(ElementName = "SpecifiedTradeSettlementPaymentMeans", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public SpecifiedTradeSettlementPaymentMeans SpecifiedTradeSettlementPaymentMeans { get; set; } = new();

    [XmlElement(ElementName = "ApplicableTradeTax", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public ApplicableTradeTax ApplicableTradeTax { get; set; } = new();

    [XmlElement(ElementName = "SpecifiedTradePaymentTerms", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public SpecifiedTradePaymentTerms SpecifiedTradePaymentTerms { get; set; } = new();

    [XmlElement(ElementName = "SpecifiedTradeSettlementHeaderMonetarySummation", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public SpecifiedTradeSettlementHeaderMonetarySummation SpecifiedTradeSettlementHeaderMonetarySummation { get; set; } = new();
}

[XmlRoot(ElementName = "SupplyChainTradeTransaction", Namespace = ZUGFeRDWriter.CrossIndustryInvoice)]
public class SupplyChainTradeTransaction
{

    [XmlElement(ElementName = "IncludedSupplyChainTradeLineItem", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public IncludedSupplyChainTradeLineItem IncludedSupplyChainTradeLineItem { get; set; } = new();

    [XmlElement(ElementName = "ApplicableHeaderTradeAgreement", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public ApplicableHeaderTradeAgreement ApplicableHeaderTradeAgreement { get; set; } = new();

    [XmlElement(ElementName = "ApplicableHeaderTradeDelivery", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public object ApplicableHeaderTradeDelivery { get; set; } = new();

    [XmlElement(ElementName = "ApplicableHeaderTradeSettlement", Namespace = ZUGFeRDWriter.ReusableAggregateBusinessInformationEntity)]
    public ApplicableHeaderTradeSettlement ApplicableHeaderTradeSettlement { get; set; } = new();
}

[XmlRoot(ElementName = "CrossIndustryInvoice", Namespace = ZUGFeRDWriter.CrossIndustryInvoice)]
public class CrossIndustryInvoice
{

    [XmlElement(ElementName = "ExchangedDocumentContext", Namespace = ZUGFeRDWriter.CrossIndustryInvoice)]
    public ExchangedDocumentContext ExchangedDocumentContext { get; set; } = new();

    [XmlElement(ElementName = "ExchangedDocument", Namespace = ZUGFeRDWriter.CrossIndustryInvoice)]
    public ExchangedDocument ExchangedDocument { get; set; } = new();

    [XmlElement(ElementName = "SupplyChainTradeTransaction", Namespace = ZUGFeRDWriter.CrossIndustryInvoice)]
    public SupplyChainTradeTransaction SupplyChainTradeTransaction { get; set; } = new();

    [XmlAttribute(AttributeName = "rsm", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Rsm { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "ram", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Ram { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "qdt", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Qdt { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "udt", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Udt { get; set; } = string.Empty;

    [XmlText]
    public string Text { get; set; } = string.Empty;
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
