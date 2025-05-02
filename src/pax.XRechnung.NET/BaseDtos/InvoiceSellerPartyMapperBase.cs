using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// Invoice Party Mapper
/// </summary>
public abstract class InvoiceSellerPartyMapperBase<T> where T : IPartyBaseDto, new()
{
    /// <summary>
    /// Map XmlParty to IPartyBaseDto
    /// </summary>
    public virtual IPartyBaseDto FromXml(XmlParty xmlParty)
    {
        ArgumentNullException.ThrowIfNull(xmlParty);
        var dto = new T
        {
            Website = xmlParty.Website,
            LogoReferenceId = xmlParty.LogoReferenceId,
            Name = xmlParty.PartyName.Name,
            StreetName = xmlParty.PostalAddress.StreetName,
            City = xmlParty.PostalAddress.City,
            PostCode = xmlParty.PostalAddress.PostCode,
            CountryCode = xmlParty.PostalAddress.Country.IdentificationCode,
            RegistrationName = xmlParty.PartyLegalEntity.RegistrationName,
            TaxId = xmlParty.PartyTaxScheme?.CompanyId ?? string.Empty,
            Telefone = xmlParty.Contact?.Telephone ?? string.Empty,
            Email = xmlParty.Contact?.Email ?? string.Empty,
        };
        return dto;
    }

    /// <summary>
    /// Map IPartyBaseDto to XmlParty
    /// </summary>
    public virtual XmlParty ToXml(IPartyBaseDto partyBaseDto, IInvoiceBaseDto invoiceBaseDto)
    {
        ArgumentNullException.ThrowIfNull(partyBaseDto);
        ArgumentNullException.ThrowIfNull(invoiceBaseDto);
        return new()
        {
            Website = partyBaseDto.Website,
            LogoReferenceId = InvoiceMapperUtils.GetNullableString(partyBaseDto.LogoReferenceId),
            EndpointId = new() { SchemeId = "EM", Content = partyBaseDto.RegistrationName },
            PartyName = new() { Name = partyBaseDto.Name },
            PostalAddress = new()
            {
                StreetName = InvoiceMapperUtils.GetNullableString(partyBaseDto.StreetName),
                City = partyBaseDto.City,
                PostCode = partyBaseDto.PostCode,
                Country = new() { IdentificationCode = partyBaseDto.CountryCode },
            },
            PartyLegalEntity = new() { RegistrationName = partyBaseDto.RegistrationName },
            PartyTaxScheme = new()
            {
                CompanyId = partyBaseDto.TaxId,
                TaxScheme = new()
                {
                    Id = new() { Content = invoiceBaseDto.GlobalTaxScheme }
                }
            },
            Contact = new()
            {
                Name = InvoiceMapperUtils.GetNullableString(partyBaseDto.Name),
                Email = InvoiceMapperUtils.GetNullableString(partyBaseDto.Email),
                Telephone = InvoiceMapperUtils.GetNullableString(partyBaseDto.Telefone),
            }
        };
    }
}
