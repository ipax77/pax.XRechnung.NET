using pax.XRechnung.NET.BaseDtos;
using System.ComponentModel.DataAnnotations;

namespace pax.XRechnung.NET.AnnotatedDtos;

/// <summary>
/// Additional Document Reference
/// </summary>
public class DocumentReferenceAnnotationDto : IDocumentReferenceBaseDto
{
    /// <summary>
    /// Id
    /// </summary>
    [Required]
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// Document Description
    /// </summary>
    public string DocumentDescription { get; set; } = string.Empty;
    /// <summary>
    /// Mime Code - e.g. application/pdf
    /// </summary>
    [Required]
    public string MimeCode { get; set; } = string.Empty;
    /// <summary>
    /// FileName
    /// </summary>
    [Required]
    public string FileName { get; set; } = string.Empty;
    /// <summary>
    /// Content as base64 encoded string
    /// </summary>
    [Required]
    public string Content { get; set; } = string.Empty;
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
