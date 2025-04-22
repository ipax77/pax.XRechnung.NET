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

        var xmlTaxCategory = xmlInvoice.TaxTotal.TaxSubTotal.FirstOrDefault()?.TaxCategory;
        var tax = xmlTaxCategory?.Percent ?? 19.0m;
        var taxScheme = xmlTaxCategory?.TaxScheme.Id.Content ?? "VAT";
        var taxCategory = xmlTaxCategory?.Id.Content ?? "S";

        return new()
        {
            GlobalTaxCategory = taxCategory,
            GlobalTaxScheme = taxScheme,
            GlobalTax = (double)tax,
            Id = xmlInvoice.Id.Content,
            IssueDate = GetDateTime(xmlInvoice.IssueDate),
            DueDate = GetDateTime(xmlInvoice.DueDate?.Value),
            InvoiceTypeCode = xmlInvoice.InvoiceTypeCode,
            DocumentCurrencyCode = xmlInvoice.DocumentCurrencyCode,
            BuyerReference = xmlInvoice.BuyerReference,
            InvoiceLines = xmlInvoice.InvoiceLines.Select(s => LineToDto(s)).ToList(),
            SellerParty = PartyToDto(xmlInvoice.SellerParty.Party),
            BuyerParty = PartyToDto(xmlInvoice.BuyerParty.Party),
            PaymentMeans = new()
            {
                Iban = xmlInvoice.PaymentMeans.PayeeFinancialAccount?.Id.Content ?? string.Empty,
                Bic = xmlInvoice.PaymentMeans.PayeeFinancialAccount?.FinancialInstitutionBranch?.Id?.Content ?? string.Empty,
                Name = xmlInvoice.PaymentMeans.PayeeFinancialAccount?.Name ?? string.Empty,
            },
            PaymentMeansTypeCode = xmlInvoice.PaymentMeans.PaymentMeansTypeCode,
            PaymentTermsNote = xmlInvoice.PaymentTerms?.Note ?? string.Empty,
            PayableAmount = (double)(xmlInvoice.LegalMonetaryTotal.PayableAmount?.Value ?? 0),

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

        decimal taxRate = RoundAmount(dto.GlobalTax / 100.0);
        decimal taxExclusiveAmount = RoundAmount(dto.InvoiceLines
            .Sum(s => RoundAmount(RoundAmount(s.Quantity) * RoundAmount(s.UnitPrice))));

        decimal payableAmount = RoundAmount(taxExclusiveAmount + taxExclusiveAmount * taxRate);
        decimal taxAmount = RoundAmount(payableAmount - taxExclusiveAmount);

        return new()
        {
            Id = new() { Content = dto.Id },
            IssueDate = new DateOnly(dto.IssueDate.Year, dto.IssueDate.Month, dto.IssueDate.Day),
            InvoiceTypeCode = dto.InvoiceTypeCode,
            DocumentCurrencyCode = dto.DocumentCurrencyCode,
            BuyerReference = dto.BuyerReference,
            InvoiceLines = dto.InvoiceLines
                .Select(s =>
                    LineToXml(s, dto.DocumentCurrencyCode, dto.GlobalTaxCategory, dto.GlobalTaxScheme, dto.GlobalTax))
                .ToList(),
            SellerParty = new() { Party = PartyToXml(dto.SellerParty, dto) },
            BuyerParty = new() { Party = PartyToXml(dto.BuyerParty, null) },
            PaymentMeans = new()
            {
                PaymentMeansTypeCode = dto.PaymentMeansTypeCode ?? "30",
                PayeeFinancialAccount = new()
                {
                    Id = new() { Content = dto.PaymentMeans.Iban },
                    Name = dto.PaymentMeans.Name,
                    FinancialInstitutionBranch = new()
                    {
                        Id = new() { Content = dto.PaymentMeans.Bic }
                    }
                },
            },
            PaymentTerms = string.IsNullOrEmpty(dto.PaymentTermsNote) ? null : new() { Note = dto.PaymentTermsNote },
            TaxTotal = new()
            {
                TaxAmount = new()
                {
                    Value = taxAmount,
                    CurrencyID = dto.DocumentCurrencyCode
                },
                TaxSubTotal =
                [
                    new()
                    {
                        TaxableAmount = new() { Value = taxExclusiveAmount,
                            CurrencyID = dto.DocumentCurrencyCode },
                        TaxAmount = new() { Value = taxAmount,
                            CurrencyID = dto.DocumentCurrencyCode },
                        TaxCategory = new()
                        {
                            Id = new() { Content = dto.GlobalTaxCategory },
                            Percent = RoundAmount(dto.GlobalTax),
                            TaxScheme = new() { Id = new() { Content = dto.GlobalTaxScheme } }
                        }
                    }
                ]
            },
            LegalMonetaryTotal = new()
            {
                LineExtensionAmount = new()
                {
                    Value = taxExclusiveAmount,
                    CurrencyID = dto.DocumentCurrencyCode
                },
                TaxExclusiveAmount = new()
                {
                    Value = taxExclusiveAmount,
                    CurrencyID = dto.DocumentCurrencyCode
                },
                TaxInclusiveAmount = new() { Value = payableAmount, CurrencyID = dto.DocumentCurrencyCode },
                PayableAmount = new() { Value = payableAmount, CurrencyID = dto.DocumentCurrencyCode },
            },
        };
    }

    private static PartyBaseDto PartyToDto(XmlParty xmlParty)
    {
        return new()
        {
            Website = xmlParty.Website,
            LogoReferenceId = xmlParty.LogoReferenceId,
            Name = xmlParty.PartyName.Name,
            StreetName = xmlParty.PostalAddress.StreetName,
            City = xmlParty.PostalAddress.City,
            PostCode = xmlParty.PostalAddress.PostCode,
            CountryCode = xmlParty.PostalAddress.Country.IdentificationCode,
            RegistrationName = xmlParty.PartyLegalEntity.RegistrationName,
            TaxId = xmlParty.PartyTaxScheme?.CompanyId ?? string.Empty,
            Telefone = xmlParty.Contact?.Telephone ?? string.Empty,
            Email = xmlParty.Contact?.Email ?? string.Empty,
        };
    }

    private static XmlParty PartyToXml(PartyBaseDto dtoParty, InvoiceBaseDto? invoiceBaseDto)
    {
        return new()
        {
            Website = dtoParty.Website,
            LogoReferenceId = dtoParty.LogoReferenceId,
            EndpointId = new() { SchemeId = "EM", Content = dtoParty.RegistrationName },
            PartyName = new() { Name = dtoParty.Name },
            PostalAddress = new()
            {
                StreetName = dtoParty.StreetName,
                City = dtoParty.City,
                PostCode = dtoParty.PostCode,
                Country = new() { IdentificationCode = dtoParty.CountryCode },
            },
            PartyLegalEntity = new() { RegistrationName = dtoParty.RegistrationName },
            PartyTaxScheme = invoiceBaseDto == null ? null : new()
            {
                CompanyId = dtoParty.TaxId,
                TaxScheme = new()
                {
                    Id = new() { Content = invoiceBaseDto.GlobalTaxScheme }
                }
            },
            Contact = new()
            {
                Name = dtoParty.Name,
                Email = dtoParty.Email,
                Telephone = dtoParty.Telefone,
            }
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
            UnitPrice = (double)xmlLine.PriceDetails.PriceAmount.Value,
            StartDate = xmlLine.InvoicePeriod == null ? null
                : GetDateTime(xmlLine.InvoicePeriod.StartDate?.Value, xmlLine.InvoicePeriod.StartTime?.Value),
            EndDate = xmlLine.InvoicePeriod == null ? null
                : GetDateTime(xmlLine.InvoicePeriod.EndDate?.Value, xmlLine.InvoicePeriod.EndTime?.Value),
            Description = xmlLine.Item.Description,
            Name = xmlLine.Item.Name,
        };
    }

    private static XmlInvoiceLine LineToXml(InvoiceLineBaseDto dtoLine,
                                            string currencyId,
                                            string taxCategory,
                                            string taxScheme,
                                            double tax)
    {
        var lineTotal = RoundAmount(RoundAmount(dtoLine.Quantity) * RoundAmount(dtoLine.UnitPrice));

        return new()
        {
            Id = new() { Content = dtoLine.Id },
            Note = dtoLine.Note,
            InvoicedQuantity = new() { Value = RoundAmount(dtoLine.Quantity), UnitCode = dtoLine.QuantityCode },
            LineExtensionAmount = new() { Value = lineTotal, CurrencyID = currencyId },
            PriceDetails = new() { PriceAmount = new() { Value = RoundAmount(dtoLine.UnitPrice), CurrencyID = currencyId } },
            InvoicePeriod = GetXmlPeriod(dtoLine.StartDate, dtoLine.EndDate),
            Item = new()
            {
                Description = dtoLine.Description,
                Name = dtoLine.Name,
                ClassifiedTaxCategory = new()
                {
                    Id = new() { Content = taxCategory },
                    Percent = RoundAmount(tax),
                    TaxScheme = new() { Id = new() { Content = taxScheme } }
                }
            }
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

    private static decimal RoundAmount(double value) => Math.Round((decimal)value, 2, MidpointRounding.AwayFromZero);
    private static decimal RoundAmount(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);
}
