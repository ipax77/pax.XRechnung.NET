using System.Text.Json.Serialization;
using System.Xml.Schema;

namespace pax.XRechnung.NET;

/// <summary>
/// ValidationResult
/// </summary>
public sealed class InvoiceValidationResult
{
    /// <summary>
    /// JsonConstructor
    /// </summary>
    [JsonConstructor]
    public InvoiceValidationResult() { }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="validationEventArgs"></param>
    public InvoiceValidationResult(ICollection<ValidationEventArgs> validationEventArgs)
    {
        Validations = validationEventArgs.Select(s => new ValidationMessage(s))
            .ToList();
        if (Validations.Count == 0)
        {
            IsValid = true;
        }
    }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="validationMessages"></param>
    public InvoiceValidationResult(ICollection<ValidationMessage> validationMessages)
    {
        Validations = validationMessages;
        if (!Validations.Where(x => x.Severity == XmlSeverityType.Error).Any())
        {
            IsValid = true;
        }
    }
    /// <summary>
    /// True if no valdation errors.
    /// </summary>
    public bool IsValid { get; set; }
    /// <summary>
    /// Validations
    /// </summary>
    public ICollection<ValidationMessage> Validations { get; } = [];
    /// <summary>
    /// Error during validation
    /// </summary>
    public string? Error { get; set; }
}

/// <summary>
/// ValidationMessage
/// </summary>
public sealed class ValidationMessage
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="message"></param>
    /// <param name="severity"></param>
    public ValidationMessage(XmlSchemaException exception, string message, XmlSeverityType severity)
    {
        Exception = exception;
        Message = message;
        Severity = severity;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="e"></param>
    public ValidationMessage(ValidationEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(e);
        Exception = e.Exception;
        Message = e.Message;
        Severity = e.Severity;
    }

    /// <summary>
    /// Exception
    /// </summary>
    public XmlSchemaException Exception { get; }
    /// <summary>
    /// Message
    /// </summary>
    public string Message { get; }
    /// <summary>
    /// Severity
    /// </summary>
    public XmlSeverityType Severity { get; }

}
