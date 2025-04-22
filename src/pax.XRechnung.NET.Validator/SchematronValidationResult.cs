using System.Net;

namespace pax.XRechnung.NET.Validator;

/// <summary>
/// Schematron Validation Result
/// </summary>
public class SchematronValidationResult
{
    public HttpStatusCode HttpStatusCode { get; init; }
    public List<ValidationStep> Steps { get; init; } = [];
    public List<ValidationMessage> Messages { get; init; } = [];
    public string? Error { get; set; }
    public bool IsValid { get; set; }
}

public class ValidationStep
{
    public string Step { get; set; } = string.Empty;
    public int Errors { get; set; }
    public int Warnings { get; set; }
    public int Infos { get; set; }
}

public class ValidationMessage
{
    public string Position { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
}