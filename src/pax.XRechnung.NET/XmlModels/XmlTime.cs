using System.Globalization;
using System.Xml.Serialization;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// XmlTime for correct UBL time format (HH:mm:ss)
/// </summary>
public class XmlTime
{
    /// <summary>
    /// The time value
    /// </summary>
    [XmlIgnore]
    public TimeOnly Value { get; set; }

    /// <summary>
    /// ValueString
    /// </summary>
    [XmlText]
    public string ValueString
    {
        get => Value.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
        set
        {
            if (TimeOnly.TryParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out var timeOnly))
            {
                Value = timeOnly;
            }
        }
    }

    /// <summary>
    /// Xml-conform string representation (HH:mm:ss)
    /// </summary>
    /// <returns>Formatted time</returns>
    public override string ToString() => Value.ToString("HH:mm:ss", CultureInfo.InvariantCulture);

    /// <summary>
    /// Implicit conversion to DateTime
    /// </summary>
    /// <param name="t">XmlTime</param>
    public static implicit operator TimeOnly(XmlTime t)
    {
        ArgumentNullException.ThrowIfNull(t);
        return t.Value;
    }

    /// <summary>
    /// Implicit conversion from DateTime
    /// </summary>
    /// <param name="d">DateTime</param>
    public static implicit operator XmlTime(TimeOnly d) => new() { Value = d };

    /// <summary>
    /// Returns value as DateTime
    /// </summary>
    public TimeOnly ToTimeOnly() => Value;

    /// <summary>
    /// Returns value as XmlTime
    /// </summary>
    public XmlTime ToXmlTime() => new() { Value = this.Value };
}
