using System.ComponentModel.DataAnnotations;
using pax.XRechnung.NET.BaseDtos;

namespace pax.XRechnung.NET.AnnotatedDtos;

public class PaymentAnnotationDto : IPaymentMeansBaseDto
{
    [Required]
    public string Iban { get; set; } = string.Empty;
    [Required]
    public string Bic { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
