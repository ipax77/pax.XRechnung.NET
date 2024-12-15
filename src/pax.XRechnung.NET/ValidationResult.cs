using System.Text.Json.Serialization;
using System.Xml.Schema;

namespace pax.XRechnung.NET;

/// <summary>
/// ValidationResult
/// </summary>
public sealed class ValidationResult
{
    /// <summary>
    /// JsonConstructor
    /// </summary>
    [JsonConstructor]
    public ValidationResult() { }
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="validationEventArgs"></param>
    public ValidationResult(ICollection<ValidationEventArgs> validationEventArgs)
    {
        Validations = validationEventArgs;
        if (Validations.Count == 0)
        {
            IsValid = true;
        }
    }
    /// <summary>
    /// True if no valdation errors.
    /// </summary>
    public bool IsValid { get; }
    /// <summary>
    /// Validations
    /// </summary>
    public ICollection<ValidationEventArgs> Validations { get; } = [];
    /// <summary>
    /// Error during validation
    /// </summary>
    public string? Error { get; set; }
}

