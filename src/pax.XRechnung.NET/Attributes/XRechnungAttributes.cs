namespace pax.XRechnung.NET.Attributes;

/// <summary>
/// XRechnungAttribute
/// </summary>
public abstract class XRechnungAttribute : Attribute
{
}

/// <summary>
/// SpecificationIdAttribute
/// </summary>
/// <param name="id"></param>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class SpecificationIdAttribute(string id) : XRechnungAttribute
{
    /// <summary>
    /// Property ID
    /// </summary>
    public string Id { get; } = id;
}

/// <summary>
/// CodeListAttribute
/// </summary>
/// <param name="id"></param>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class CodeListAttribute(string id) : XRechnungAttribute
{
    /// <summary>
    /// CodelList ID
    /// </summary>
    public string Id { get; } = id;
}

/// <summary>
/// XRechnungAttributeExtensions
/// </summary>
public static class XRechnungAttributeExtensions
{
    /// <summary>
    /// Get XRechung Attribute
    /// </summary>
    /// <typeparam name="TC"></typeparam>
    /// <typeparam name="TA"></typeparam>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static TA? GetAttributeFrom<TC, TA>(string propertyName) where TA : XRechnungAttribute
    {
        return (TA?)typeof(TC).GetProperty(propertyName)?
            .GetCustomAttributes(typeof(TA), false).SingleOrDefault();
    }
}