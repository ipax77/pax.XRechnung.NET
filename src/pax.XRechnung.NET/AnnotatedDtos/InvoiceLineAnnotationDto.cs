using System.ComponentModel.DataAnnotations;
using pax.XRechnung.NET.BaseDtos;

namespace pax.XRechnung.NET.AnnotatedDtos;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class InvoiceLineAnnotationDto : IInvoiceLineBaseDto
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

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
