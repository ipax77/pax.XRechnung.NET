namespace pax.XRechnung.NET.Dtos;

/// <summary>
/// Eine Gruppe von Informationselementen, die Informationen über die Eigenschaften der inRechnung gestellten Waren
/// und Dienstleistungen enthalten.
/// </summary>
public record ItemAttributeDto
{
    /// <summary>
    ///  Der Name der Eigenschaft des Postens, wie z. B. „Farbe“.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Der Wert der Eigenschaft des Postens, wie z. B. „rot“.
    /// </summary>
    public string Value { get; set; } = string.Empty;
}