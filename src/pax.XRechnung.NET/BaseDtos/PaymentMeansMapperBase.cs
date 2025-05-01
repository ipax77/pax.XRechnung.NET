using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// PaymentMeans Mapper
/// </summary>
public abstract class PaymentMeansMapperBase<T> where T : IPaymentMeansBaseDto, new()
{
    /// <summary>
    /// Map XmlParty to IPartyBaseDto
    /// </summary>
    public virtual T FromXml(XmlPaymentMeans xmlPayment)
    {
        ArgumentNullException.ThrowIfNull(xmlPayment);
        var dto = new T
        {
            Iban = xmlPayment.PayeeFinancialAccount?.Id.Content ?? string.Empty,
            Bic = xmlPayment.PayeeFinancialAccount?.FinancialInstitutionBranch?.Id?.Content ?? string.Empty,
            Name = xmlPayment.PayeeFinancialAccount?.Name ?? string.Empty,
        };
        return dto;
    }

    /// <summary>
    /// Map IPartyBaseDto to XmlParty
    /// </summary>
    public virtual XmlPaymentMeans ToXml(IPaymentMeansBaseDto dto, string paymentMeansTypeCode)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new()
        {
            PaymentMeansTypeCode = paymentMeansTypeCode,
            PayeeFinancialAccount = new()
            {
                Id = new() { Content = dto.Iban },
                Name = dto.Name,
                FinancialInstitutionBranch = new()
                {
                    Id = new() { Content = dto.Bic }
                }
            },
        };
    }
}
