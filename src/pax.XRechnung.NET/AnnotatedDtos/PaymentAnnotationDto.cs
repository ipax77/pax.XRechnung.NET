using pax.XRechnung.NET.BaseDtos;
using System.ComponentModel.DataAnnotations;

namespace pax.XRechnung.NET.AnnotatedDtos;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class PaymentAnnotationDto : IPaymentMeansBaseDto
{
    [Required]
    public string Iban { get; set; } = string.Empty;
    [Required]
    public string Bic { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    [Required]
    [ValidCode(CodeListType.UNTDID_4461_3)]
    public string PaymentMeansTypeCode { get; set; } = "30";
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
