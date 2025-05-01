namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// Additional Document Reference
/// </summary>
public interface IDocumentReferenceBaseDto
{
    /// <summary>
    /// Id
    /// </summary>
    string Id { get; set; }
    /// <summary>
    /// Document Description
    /// </summary>
    string DocumentDescription { get; set; }
    /// <summary>
    /// Mime Code - e.g. application/pdf
    /// </summary>
    string MimeCode { get; set; }
    /// <summary>
    /// File Name
    /// </summary>
    string FileName { get; set; }
    /// <summary>
    /// Content as base64 encoded string
    /// </summary>
    string Content { get; set; }
}

/// <summary>
/// Additional Document Reference
/// </summary>
public class DocumentReferenceBaseDto : IDocumentReferenceBaseDto
{
    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// Document Description
    /// </summary>
    public string DocumentDescription { get; set; } = string.Empty;
    /// <summary>
    /// Mime Code - e.g. application/pdf
    /// </summary>
    public string MimeCode { get; set; } = string.Empty;
    /// <summary>
    /// FileName
    /// </summary>
    public string FileName { get; set; } = string.Empty;
    /// <summary>
    /// Content as base64 encoded string
    /// </summary>
    public string Content { get; set; } = string.Empty;
}