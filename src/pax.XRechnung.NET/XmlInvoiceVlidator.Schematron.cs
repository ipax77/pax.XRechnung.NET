
using pax.XRechnung.NET.KositValidator;
using pax.XRechnung.NET.XmlModels;
using System.Xml.Schema;
using System.Diagnostics.CodeAnalysis;

namespace pax.XRechnung.NET;

public static partial class XmlInvoiceValidator
{
    /// <summary>
    /// Validate xml string against Schmatrons
    /// Runs locally unless an explicit KoSIT server URI is supplied.
    /// </summary>
    /// <param name="invoice">XmlInvoice</param>
    /// <param name="kositUri">Optional URI to an external KoSIT validator.</param>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "The existing result-based API reports serialization and validation failures through Error.")]
    public static async Task<InvoiceValidationResult> ValidateSchematron(XmlInvoice invoice, Uri? kositUri = null)
    {
        try
        {
            var xml = XmlInvoiceWriter.Serialize(invoice);
            return await ValidateSchematron(xml, kositUri).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return new InvoiceValidationResult
            {
                IsValid = false,
                Error = ex.Message
            };
        }
    }
    /// <summary>
    /// Validate xml string against Schmatrons
    /// Runs locally unless an explicit KoSIT server URI is supplied.
    /// </summary>
    /// <param name="xml">xml text</param>
    /// <param name="kositUri">Optional URI to an external KoSIT validator.</param>
    [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "The existing result-based API reports validation failures through Error.")]
    public static async Task<InvoiceValidationResult> ValidateSchematron(string xml, Uri? kositUri = null)
    {
        try
        {
            if (kositUri is null)
            {
                return InvoiceLocalValidator.Validate(xml);
            }
            var validationResult = await InvoiceKositValidator.Validate(xml, kositUri)
                .ConfigureAwait(false);
            return MapToValidationResult(validationResult);
        }
        catch (Exception ex)
        {
            return new InvoiceValidationResult
            {
                IsValid = false,
                Error = ex.Message
            };
        }
    }

    private static InvoiceValidationResult MapToValidationResult(SchematronValidationResult kositResult)
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
            validationEvents.Add(new(exception, message, severity)
            {
                Code = msg.Code,
                Path = msg.Path,
                RuleSeverity = msg.Severity.ToUpperInvariant() switch
                {
                    "INFORMATION" or "INFO" => "information",
                    "WARNING" => "warning",
                    _ => "error"
                }
            });
        }

        if (!string.IsNullOrEmpty(kositResult.Error) ||
            (!kositResult.IsValid && !validationEvents.Any(m => m.Severity == XmlSeverityType.Error)))
        {
            validationEvents.Add(new(new XmlSchemaException("unknown"), kositResult.Error ?? kositResult.HttpStatusCode.ToString(), XmlSeverityType.Error));
        }

        var result = new InvoiceValidationResult(validationEvents)
        {
            HttpStatusCode = kositResult.HttpStatusCode.ToString(),
            Evaluation = kositResult.Evaluation,
            Conformity = kositResult.Conformity,
            Error = kositResult.Error,
            IsValid = kositResult.IsValid && string.IsNullOrEmpty(kositResult.Error)
        };
        return result;
    }
}

