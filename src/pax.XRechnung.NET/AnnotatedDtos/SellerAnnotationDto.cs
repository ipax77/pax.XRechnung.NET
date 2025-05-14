using pax.XRechnung.NET.BaseDtos;
using System.ComponentModel.DataAnnotations;

namespace pax.XRechnung.NET.AnnotatedDtos;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class SellerAnnotationDto : IPartyBaseDto
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
    [Required]
    public string RegistrationName { get; set; } = string.Empty;
    [Required]
    public string TaxId { get; set; } = string.Empty;
    public string? CompanyId { get; set; }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
