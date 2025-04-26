namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// IPartyBaseDto used for mapping
/// </summary>
public interface IPartyBaseDto
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    string? Website { get; set; }
    string? LogoReferenceId { get; set; }
    string Name { get; set; }
    string? StreetName { get; set; }
    string City { get; set; }
    string PostCode { get; set; }
    string CountryCode { get; set; }
    string Telefone { get; set; }
    string Email { get; set; }
    string RegistrationName { get; set; }
    string TaxId { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}

/// <summary>
/// Seller / Buyer Party
/// </summary>
public class PartyBaseDto : IPartyBaseDto
{
    /// <summary>
    /// Website
    /// </summary>
    public string? Website { get; set; }
    /// <summary>
    /// Logo Id referencing a AdditionalDocument
    /// </summary>
    public string? LogoReferenceId { get; set; }
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// StreetName
    /// </summary>
    public string? StreetName { get; set; }
    /// <summary>
    /// City
    /// </summary>
    public string City { get; set; } = string.Empty;
    /// <summary>
    /// PostCode
    /// </summary>
    public string PostCode { get; set; } = string.Empty;
    /// <summary>
    /// CountryCode
    /// </summary>
    public string CountryCode { get; set; } = "DE";
    /// <summary>
    /// Telefone
    /// </summary>
    public string Telefone { get; set; } = string.Empty;
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Registration Name
    /// </summary>
    public string RegistrationName { get; set; } = string.Empty;
    /// <summary>
    /// VAT number or company tax ID.
    /// </summary>
    public string TaxId { get; set; } = string.Empty;
}