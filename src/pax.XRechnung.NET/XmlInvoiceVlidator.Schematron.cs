
using System.Xml.Schema;
using pax.XRechnung.NET.Validator;

namespace pax.XRechnung.NET;

public static partial class XmlInvoiceValidator
{
    /// <summary>
    /// Validate xml string against Schmatrons
    /// Requires a running Kosit validation Server. See Readme for details.
    /// </summary>
    /// <param name="xml">xml text</param>
    /// <param name="kositUri">optional uri to the kosit validator, default is http://localhost:8080</param>
    public static async Task<ValidationResult> ValidateSchematron(string xml, Uri? kositUri = null)
    {
        try
        {
            var validationResult = await KositValidator.Validate(xml, kositUri)
                .ConfigureAwait(false);
            return MapToValidationResult(validationResult);
        }
        catch (Exception ex)
        {
            return new ValidationResult
            {
                IsValid = false,
                Error = ex.Message
            };
            throw;
        }
    }

    private static ValidationResult MapToValidationResult(SchematronValidationResult kositResult)
    {
        var validationEvents = new List<ValidationMessage>();

        foreach (var msg in kositResult.Messages)
        {
            var severity = msg.Severity.ToUpperInvariant() switch
            {
                "ERROR" => XmlSeverityType.Error,
                "WARNING" => XmlSeverityType.Warning,
                _ => XmlSeverityType.Warning
            };

            var message = $"{msg.Text}";
            if (!string.IsNullOrWhiteSpace(msg.Path))
            {
                message += $" (Pfad: {msg.Path})";
            }

            var exception = new XmlSchemaException(message);
            validationEvents.Add(new(exception, message, severity));
        }

        return new ValidationResult(validationEvents);
    }
}

