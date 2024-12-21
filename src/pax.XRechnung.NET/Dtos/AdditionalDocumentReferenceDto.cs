namespace pax.XRechnung.NET.Dtos;

/// <summary>
///  Eine Gruppe von Informationselementen mit Informationen über rechnungsbegründende Unterlagen,
///  die Belege für die in der Rechnung gestellten Ansprüche enthalten.
/// </summary>
public record AdditionalDocumentReferenceDto
{
    /// <summary>
    /// Eine Kennung der rechnungsbegründenden Unterlage
    /// </summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// Eine Beschreibung der rechnungsbegründenden Unterlage
    /// </summary>
    public string? DocumentDescription { get; set; }
    /// <summary>
    /// Die Internetadresse bzw. URL (Uniform Resource Locator), unter der das externe Dokument verfügbar ist
    /// </summary>
    public string? DocumentLocation { get; set; }
    /// <summary>
    /// MimeCode (e.g."application/pdf")
    /// </summary>
    public string MimeCode { get; set; } = string.Empty;
    /// <summary>
    /// FileName
    /// </summary>
    public string FileName { get; set; } = string.Empty;
    /// <summary>
    /// Binary content
    /// </summary>
    public string? Content { get; set; }
}