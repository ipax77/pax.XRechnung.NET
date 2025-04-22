using System.Globalization;
using System.Xml.Serialization;

namespace pax.XRechnung.NET.XmlModels;

/// <summary>
/// XmlDate for correct UBL date format
/// </summary>
public class XmlDate
{
    /// <summary>
    /// Value
    /// </summary>
    [XmlIgnore]
    public DateOnly Value { get; set; }

    /// <summary>
    /// ValueString
    /// </summary>
    [XmlText]
    public string ValueString
    {
        get => Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        set
        {
            if (DateOnly.TryParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
            {
                Value = dateTime;
            }
        }
    }

    /// <summary>
    /// Xml conform string representation
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    /// <summary>
    /// implicit operator
    /// </summary>
    /// <param name="d"></param>
    public static implicit operator DateOnly(XmlDate d)
    {
        ArgumentNullException.ThrowIfNull(d);
        return d.Value;
    }

    /// <summary>
    /// implicit operator
    /// </summary>
    /// <param name="d"></param>
    public static implicit operator XmlDate(DateOnly d) => new() { Value = d };

    /// <summary>
    /// ToDateTime
    /// </summary>
    /// <returns>DateTime</returns>
    public DateOnly ToDateOnly()
    {
        return Value;
    }
    /// <summary>
    /// ToXmlDate
    /// </summary>
    /// <returns>XmlDate</returns>
    public XmlDate ToXmlDate()
    {
        return new() { Value = this.Value };
    }
}
