using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// InvoiceLineMapperBase
/// </summary>
public abstract class InvoiceLineMapperBase<T> where T : IInvoiceLineBaseDto, new()
{
    /// <summary>
    /// Map XmlInvoiceLine to IInvoiceLineBaseDto
    /// </summary>
    public virtual IInvoiceLineBaseDto FromXml(XmlInvoiceLine xmlLine)
    {
        ArgumentNullException.ThrowIfNull(xmlLine);
        var dto = new T
        {
            Id = xmlLine.Id.Content,
            Note = InvoiceMapperUtils.GetNullableString(xmlLine.Note),
            Quantity = (double)xmlLine.InvoicedQuantity.Value,
            QuantityCode = xmlLine.InvoicedQuantity.UnitCode,
            UnitPrice = (double)xmlLine.PriceDetails.PriceAmount.Value,
            StartDate = xmlLine.InvoicePeriod == null ? null
                : InvoiceMapperUtils.GetDateTime(xmlLine.InvoicePeriod.StartDate?.Value,
                    xmlLine.InvoicePeriod.StartTime?.Value),
            EndDate = xmlLine.InvoicePeriod == null ? null
                : InvoiceMapperUtils.GetDateTime(xmlLine.InvoicePeriod.EndDate?.Value,
                    xmlLine.InvoicePeriod.EndTime?.Value),
            Description = InvoiceMapperUtils.GetNullableString(xmlLine.Item.Description),
            Name = xmlLine.Item.Name,
        };
        return dto;
    }

    /// <summary>
    /// Map IInvoiceLineBaseDto to XmlInvoiceLine
    /// </summary>
    public virtual XmlInvoiceLine ToXml(
        IInvoiceLineBaseDto dtoLine,
        string currencyId,
        string taxCategory,
        string taxScheme,
        double tax)
    {
        ArgumentNullException.ThrowIfNull(dtoLine);
        var lineTotal = InvoiceMapperUtils.RoundAmount(InvoiceMapperUtils.RoundAmount(dtoLine.Quantity)
            * InvoiceMapperUtils.RoundAmount(dtoLine.UnitPrice));

        return new XmlInvoiceLine
        {
            Id = new() { Content = dtoLine.Id },
            Note = InvoiceMapperUtils.GetNullableString(dtoLine.Note),
            InvoicedQuantity = new() { Value = InvoiceMapperUtils.RoundAmount(dtoLine.Quantity), UnitCode = dtoLine.QuantityCode },
            LineExtensionAmount = new() { Value = lineTotal, CurrencyID = currencyId },
            PriceDetails = new() { PriceAmount = new() { Value = InvoiceMapperUtils.RoundAmount(dtoLine.UnitPrice), CurrencyID = currencyId } },
            InvoicePeriod = InvoiceMapperUtils.GetXmlPeriod(dtoLine.StartDate, dtoLine.EndDate),
            Item = new()
            {
                Description = InvoiceMapperUtils.GetNullableString(dtoLine.Description),
                Name = dtoLine.Name,
                ClassifiedTaxCategory = new()
                {
                    Id = new() { Content = taxCategory },
                    Percent = InvoiceMapperUtils.RoundAmount(tax),
                    TaxScheme = new() { Id = new() { Content = taxScheme } }
                }
            }
        };
    }


}
