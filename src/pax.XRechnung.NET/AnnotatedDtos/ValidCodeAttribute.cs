
using System.ComponentModel.DataAnnotations;

namespace pax.XRechnung.NET.AnnotatatedDtos;

/// <summary>
/// Code List Validation Attribute
/// </summary>
/// <remarks>
/// ValidCodeAttribute
/// </remarks>
/// <param name="listType"></param>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class ValidCodeAttribute(CodeListType listType) : ValidationAttribute
{
    /// <summary>
    /// CodeListType
    /// </summary>
    public CodeListType ListType { get; } = listType;

    /// <summary>
    /// Validate CodeList
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string stringValue && !string.IsNullOrEmpty(stringValue))
        {
            var listId = ListType.ToString();
            var isValid = CodeListRepository.IsValidCode(listId, stringValue);

            if (!isValid)
            {
                return new ValidationResult(
                    $"The code '{stringValue}' is not valid for list '{listId}'."
                );
            }
        }

        return ValidationResult.Success;
    }
}

