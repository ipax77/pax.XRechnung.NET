using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// InvoiceMapperUtils
/// </summary>
public static class InvoiceMapperUtils
{
    /// <summary>
    /// GetXmlPeriod
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public static XmlPeriod? GetXmlPeriod(DateTime? startDate, DateTime? endDate)
    {
        if (startDate is null || endDate is null)
        {
            return null;
        }
        return new()
        {
            StartDate = new DateOnly(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day),
            StartTime = new TimeOnly(startDate.Value.Hour, startDate.Value.Minute, startDate.Value.Second),
            EndDate = new DateOnly(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day),
            EndTime = new TimeOnly(endDate.Value.Hour, endDate.Value.Minute, endDate.Value.Second),
        };
    }

    /// <summary>
    /// GetDateTime
    /// </summary>
    /// <param name="dateOnly"></param>
    /// <param name="timeOnly"></param>
    /// <returns></returns>
    public static DateTime GetDateTime(DateOnly? dateOnly, TimeOnly? timeOnly = null)
    {
        if (dateOnly is null)
            return DateTime.MinValue;

        return timeOnly is null
            ? new DateTime(dateOnly.Value.Year, dateOnly.Value.Month, dateOnly.Value.Day)
            : new DateTime(dateOnly.Value.Year, dateOnly.Value.Month, dateOnly.Value.Day,
                           timeOnly.Value.Hour, timeOnly.Value.Minute, timeOnly.Value.Second);
    }

    /// <summary>
    /// RoundAmount
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static decimal RoundAmount(double value) =>
        Math.Round((decimal)value, 2, MidpointRounding.AwayFromZero);

    /// <summary>
    /// RoundAmount
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static decimal RoundAmount(decimal value) =>
        Math.Round(value, 2, MidpointRounding.AwayFromZero);
}
