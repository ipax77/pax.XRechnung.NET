using System.Net;

namespace pax.XRechnung.NET.KositValidator;

/// <summary>
/// Schematron Validation Result
/// </summary>
public class SchematronValidationResult
{
    /// <summary>
    /// HttpStatusCode
    /// </summary>
    public HttpStatusCode HttpStatusCode { get; init; }
    /// <summary>
    /// Konformitätsprüfung
    /// </summary>
    public string? Conformity { get; set; }
    /// <summary>
    /// Evaluation
    /// </summary>
    public string? Evaluation { get; set; }
    /// <summary>
    /// Validation Steps
    /// </summary>
    public List<ValidationStep> Steps { get; init; } = [];
    /// <summary>
    /// Validation Messages
    /// </summary>
    public List<ValidationMessage> Messages { get; init; } = [];
    /// <summary>
    /// Error
    /// </summary>
    public string? Error { get; set; }
    /// <summary>
    /// Validation is valid
    /// </summary>
    public bool IsValid { get; set; }
}

/// <summary>
/// Validation Step
/// </summary>
public class ValidationStep
{
    /// <summary>
    /// Step information
    /// </summary>
    public string Step { get; set; } = string.Empty;
    /// <summary>
    /// Errros
    /// </summary>
    public int Errors { get; set; }
    /// <summary>
    /// Warnings
    /// </summary>
    public int Warnings { get; set; }
    /// <summary>
    /// Infos
    /// </summary>
    public int Infos { get; set; }
}

/// <summary>
/// Validation Message
/// </summary>
public class ValidationMessage
{
    /// <summary>
    /// Position
    /// </summary>
    public string Position { get; set; } = string.Empty;
    /// <summary>
    /// Code
    /// </summary>
    public string Code { get; set; } = string.Empty;
    /// <summary>
    ///  Severity
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    /// <summary>
    /// Text
    /// </summary>
    public string Text { get; set; } = string.Empty;
    /// <summary>
    /// Path
    /// </summary>
    public string Path { get; set; } = string.Empty;
}