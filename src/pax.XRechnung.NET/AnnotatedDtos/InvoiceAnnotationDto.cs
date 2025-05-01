using System.ComponentModel.DataAnnotations;
using pax.XRechnung.NET.BaseDtos;

namespace pax.XRechnung.NET.AnnotatedDtos;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Annotated Invoice DTO
/// </summary>
public class InvoiceAnnotationDto : IInvoiceBaseDto
{
    /// <summary>
    /// Global tax category (e.g., "S" for Standard rate).
    /// Applied to all invoice lines.
    /// </summary>
    [Required]
    [ValidCode(CodeListType.UNTDID_5305_3)]
    public string GlobalTaxCategory { get; set; } = "S";
    /// <summary>
    /// Global tax scheme (e.g., "VAT").
    /// Applied to all invoice lines.
    /// </summary>
    [Required]
    public string GlobalTaxScheme { get; set; } = "VAT";
    /// <summary>
    /// Global tax used for all lines
    /// </summary>
    [Required]
    public double GlobalTax { get; set; } = 19.0;
    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public DateTime IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    [Required]
    public string InvoiceTypeCode { get; set; } = string.Empty;
    public string? Note { get; set; } = string.Empty;
    [Required]
    [ValidCode(CodeListType.Currency_Codes_3)]
    public string DocumentCurrencyCode { get; set; } = "EUR";
    [Required]
    public string BuyerReference { get; set; } = string.Empty;
    public List<IDocumentReferenceBaseDto> AdditionalDocumentReferences { get; set; } = [];
    public IPartyBaseDto SellerParty { get; set; } = new SellerAnnotationDto();
    public IPartyBaseDto BuyerParty { get; set; } = new BuyerAnnotationDto();
    public IPaymentMeansBaseDto PaymentMeans { get; set; } = new PaymentAnnotationDto();
    [Required]
    [ValidCode(CodeListType.UNTDID_4461_3)]
    public string PaymentMeansTypeCode { get; set; } = "30";
    public string PaymentTermsNote { get; set; } = string.Empty;
    public double PayableAmount { get; set; }
    public List<IInvoiceLineBaseDto> InvoiceLines { get; set; } = [];
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
