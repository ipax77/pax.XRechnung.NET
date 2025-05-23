using pax.XRechnung.NET.BaseDtos;
using System.ComponentModel.DataAnnotations;

namespace pax.XRechnung.NET.AnnotatedDtos;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Annotated Invoice DTO
/// </summary>
public class InvoiceAnnotationDto : IInvoiceBaseDto
{
    [Required]
    [ValidCode(CodeListType.UNTDID_5305_3)]
    public string GlobalTaxCategory { get; set; } = "S";
    [Required]
    public string GlobalTaxScheme { get; set; } = "VAT";
    [Required]
    public double GlobalTax { get; set; } = 19.0;
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public DateTime IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    [Required]
    [ValidCode(CodeListType.UNTDID_1001_4)]
    public string InvoiceTypeCode { get; set; } = string.Empty;
    public string? Note { get; set; } = string.Empty;
    [Required]
    [ValidCode(CodeListType.Currency_Codes_3)]
    public string DocumentCurrencyCode { get; set; } = "EUR";
    public List<DocumentReferenceAnnotationDto> AdditionalDocumentReferences { get; set; } = [];
    public SellerAnnotationDto SellerParty { get; set; } = new SellerAnnotationDto();
    public BuyerAnnotationDto BuyerParty { get; set; } = new BuyerAnnotationDto();
    public PaymentAnnotationDto PaymentMeans { get; set; } = new PaymentAnnotationDto();

    public string PaymentTermsNote { get; set; } = string.Empty;
    public double PayableAmount { get; set; }
    public List<InvoiceLineAnnotationDto> InvoiceLines { get; set; } = [];

    IPartyBaseDto IInvoiceBaseDto.SellerParty { get => SellerParty; set => SellerParty = (SellerAnnotationDto)value; }
    IPartyBaseDto IInvoiceBaseDto.BuyerParty { get => BuyerParty; set => BuyerParty = (BuyerAnnotationDto)value; }
    IPaymentMeansBaseDto IInvoiceBaseDto.PaymentMeans { get => PaymentMeans; set => PaymentMeans = (PaymentAnnotationDto)value; }
    List<IInvoiceLineBaseDto> IInvoiceBaseDto.InvoiceLines
    {
        get => InvoiceLines.Cast<IInvoiceLineBaseDto>().ToList();
        set => InvoiceLines = value.Cast<InvoiceLineAnnotationDto>().ToList();
    }
    List<IDocumentReferenceBaseDto> IInvoiceBaseDto.AdditionalDocumentReferences
    {
        get => AdditionalDocumentReferences.Cast<IDocumentReferenceBaseDto>().ToList();
        set => AdditionalDocumentReferences = value.Cast<DocumentReferenceAnnotationDto>().ToList();
    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
