using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// InvoiceMapper abstraction
/// </summary>
public abstract class InvoiceMapperBase<T> : IInvoiceMapper<T> where T : InvoiceBaseDto, new()
{
    /// <summary>
    /// Map xmlInvoice to T
    /// </summary>
    /// <param name="xmlInvoice"></param>
    /// <returns></returns>
    public virtual T FromXml(XmlInvoice xmlInvoice)
    {
        ArgumentNullException.ThrowIfNull(xmlInvoice);
        return new()
        {
            Id = xmlInvoice.Id.Content,
            IssueDate = GetDateTime(xmlInvoice.IssueDate),
            DocumentCurrencyCode = xmlInvoice.DocumentCurrencyCode,
            InvoiceLines = xmlInvoice.InvoiceLines.Select(s => LineToDto(s)).ToList(),
        };
    }
    /// <summary>
    /// Map T to XmlInvoice
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public virtual XmlInvoice ToXml(T dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        return new()
        {
            Id = new() { Content = dto.Id },
            IssueDate = new DateOnly(dto.IssueDate.Year, dto.IssueDate.Month, dto.IssueDate.Day),
            DocumentCurrencyCode = dto.DocumentCurrencyCode,
            InvoiceLines = dto.InvoiceLines.Select(s => LineToXml(s)).ToList(),
        };
    }

    private static InvoiceLineBaseDto LineToDto(XmlInvoiceLine xmlLine)
    {
        return new()
        {
            Id = xmlLine.Id.Content,
            Note = xmlLine.Note,
            Quantity = (double)xmlLine.InvoicedQuantity.Value,
            QuantityCode = xmlLine.InvoicedQuantity.UnitCode,
            Amount = (double)xmlLine.LineExtensionAmount.Value,
            StartDate = GetDateTime(xmlLine.InvoicePeriod?.StartDate?.Value, xmlLine.InvoicePeriod?.StartTime?.Value),
            EndDate = GetDateTime(xmlLine.InvoicePeriod?.EndDate?.Value, xmlLine.InvoicePeriod?.EndTime?.Value),
            Description = xmlLine.Item.Description,
            Name = xmlLine.Item.Name,
        };
    }

    private static XmlInvoiceLine LineToXml(InvoiceLineBaseDto dtoLine, string currencyId = "EUR")
    {
        return new()
        {
            Id = new() { Content = dtoLine.Id },
            Note = dtoLine.Note,
            InvoicedQuantity = new() { Value = (decimal)dtoLine.Quantity, UnitCode = dtoLine.QuantityCode },
            LineExtensionAmount = new() { Value = (decimal)dtoLine.Amount, CurrencyID = currencyId },
            InvoicePeriod = GetXmlPeriod(dtoLine.StartDate, dtoLine.EndDate),
            Item = new()
            {
                Description = dtoLine.Description,
                Name = dtoLine.Name,
            },
        };
    }

    private static XmlPeriod? GetXmlPeriod(DateTime? startDate, DateTime? endDate)
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

    private static DateTime GetDateTime(DateOnly? dateOnly, TimeOnly? timeOnly = null)
    {
        if (dateOnly is null)
        {
            return DateTime.MinValue;
        }
        if (timeOnly is null)
        {
            return new DateTime(dateOnly.Value.Year, dateOnly.Value.Month, dateOnly.Value.Day);
        }
        return new DateTime(dateOnly.Value.Year, dateOnly.Value.Month, dateOnly.Value.Day,
         timeOnly.Value.Hour, timeOnly.Value.Minute, timeOnly.Value.Second);
    }
}

/// <summary>
/// InvoiceMapper Interface
/// </summary>
/// <typeparam name="T">InvoiceBaseDto</typeparam>
public interface IInvoiceMapper<T> where T : InvoiceBaseDto
{
    /// <summary>
    /// Map xmlInvoice to T
    /// </summary>
    /// <param name="xmlInvoice"></param>
    /// <returns></returns>
    T FromXml(XmlInvoice xmlInvoice);
    /// <summary>
    /// Map T to XmlInvoice
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    XmlInvoice ToXml(T dto);
}

/// <summary>
/// InvoiceMapper implementation
/// </summary>
public class InvoiceMapper<T> : InvoiceMapperBase<InvoiceBaseDto>
{
}
