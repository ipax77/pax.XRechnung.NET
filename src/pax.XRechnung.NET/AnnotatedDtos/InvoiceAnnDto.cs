using System.ComponentModel.DataAnnotations;
using pax.XRechnung.NET.BaseDtos;

namespace pax.XRechnung.NET.AnnotatatedDtos;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Annotated Invoice DTO
/// </summary>
public class InvoiceAnnDto : IInvoiceBaseDto
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
    public IPartyBaseDto SellerParty { get; set; } = new SellerAnnDto();
    public IPartyBaseDto BuyerParty { get; set; } = new BuyerAnnDto();
    public IPaymentMeansBaseDto PaymentMeans { get; set; } = new PaymentAnnDto();
    [Required]
    [ValidCode(CodeListType.UNTDID_4461_3)]
    public string PaymentMeansTypeCode { get; set; } = "30";
    public string PaymentTermsNote { get; set; } = string.Empty;
    public double PayableAmount { get; set; }
    public List<IInvoiceLineBaseDto> InvoiceLines { get; set; } = [];
}

public class InvoiceLineAnnDto : IInvoiceLineBaseDto
{
    public string Id { get; set; } = string.Empty;
    public string? Note { get; set; } = string.Empty;
    [Required]
    public double Quantity { get; set; }
    [Required]
    [ValidCode(CodeListType.UN_ECE_Recommendation_N_20_3)]
    public string QuantityCode { get; set; } = "HUR";
    [Required]
    public double UnitPrice { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    public double LineTotal => Math.Round(Math.Round(Quantity, 2) * Math.Round(UnitPrice, 2), 2);
}

public class PaymentAnnDto : IPaymentMeansBaseDto
{
    [Required]
    public string Iban { get; set; } = string.Empty;
    [Required]
    public string Bic { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class SellerAnnDto : IPartyBaseDto
{
    public string? Website { get; set; }
    public string? LogoReferenceId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string? StreetName { get; set; }
    [Required]
    public string City { get; set; } = string.Empty;
    [Required]
    public string PostCode { get; set; } = string.Empty;
    [Required]
    [ValidCode(CodeListType.Country_Codes_8)]
    public string CountryCode { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    public string RegistrationName { get; set; } = string.Empty;
    [Required]
    public string TaxId { get; set; } = string.Empty;
}

public class BuyerAnnDto : IPartyBaseDto
{
    public string? Website { get; set; }
    public string? LogoReferenceId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string? StreetName { get; set; }
    [Required]
    public string City { get; set; } = string.Empty;
    [Required]
    public string PostCode { get; set; } = string.Empty;
    [Required]
    [ValidCode(CodeListType.Country_Codes_8)]
    public string CountryCode { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    public string RegistrationName { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
