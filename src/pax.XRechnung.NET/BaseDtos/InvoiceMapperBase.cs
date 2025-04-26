using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// InvoiceMapper abstraction
/// </summary>
public abstract class InvoiceMapperBase<T> : IInvoiceMapper<T> where T : IInvoiceBaseDto, new()
{
    /// <summary>
    /// DefaultPaymentMeansTypeCode
    /// </summary>
    protected const string DefaultPaymentMeansTypeCode = "30";
    /// <summary>
    /// DefaultTax
    /// </summary>
    protected const decimal DefaultTax = 19.0m;
    /// <summary>
    /// DefaultTaxScheme
    /// </summary>
    protected const string DefaultTaxScheme = "VAT";
    /// <summary>
    /// DefaultTaxCategory
    /// </summary>
    protected const string DefaultTaxCategory = "S";

    /// <summary>
    /// Map xmlInvoice to T
    /// </summary>
    /// <param name="xmlInvoice"></param>
    /// <returns></returns>
    public virtual T FromXml(XmlInvoice xmlInvoice)
    {
        ArgumentNullException.ThrowIfNull(xmlInvoice);

        var xmlTaxCategory = xmlInvoice.TaxTotal.TaxSubTotal.FirstOrDefault()?.TaxCategory;
        var tax = xmlTaxCategory?.Percent ?? DefaultTax;
        var taxScheme = xmlTaxCategory?.TaxScheme.Id.Content ?? DefaultTaxScheme;
        var taxCategory = xmlTaxCategory?.Id.Content ?? DefaultTaxCategory;

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
            SellerParty = SellerPartyToDto(xmlInvoice.SellerParty.Party),
            BuyerParty = BuyerPartyToDto(xmlInvoice.BuyerParty.Party),
            PaymentMeans = new PaymentMeansBaseDto()
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
            SellerParty = new() { Party = SellerPartyToXml(dto.SellerParty, dto) },
            BuyerParty = new() { Party = BuyerPartyToXml(dto.BuyerParty) },
            PaymentMeans = new()
            {
                PaymentMeansTypeCode = dto.PaymentMeansTypeCode ?? DefaultPaymentMeansTypeCode,
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

    /// <summary>
    /// Map SellerParty to Dto
    /// </summary>
    /// <param name="xmlParty"></param>
    /// <returns></returns>
    protected virtual IPartyBaseDto SellerPartyToDto(XmlParty xmlParty)
    {
        ArgumentNullException.ThrowIfNull(xmlParty);
        return new PartyBaseDto()
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

    /// <summary>
    /// Map SellerParty to Dto
    /// </summary>
    /// <param name="xmlParty"></param>
    /// <returns></returns>
    protected virtual IPartyBaseDto BuyerPartyToDto(XmlParty xmlParty)
    {
        ArgumentNullException.ThrowIfNull(xmlParty);
        return new PartyBaseDto()
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

    /// <summary>
    /// Map SellerPartyToXml
    /// </summary>
    /// <param name="partyBaseDto"></param>
    /// <param name="invoiceBaseDto"></param>
    /// <returns></returns>
    protected virtual XmlParty SellerPartyToXml(IPartyBaseDto partyBaseDto, IInvoiceBaseDto invoiceBaseDto)
    {
        ArgumentNullException.ThrowIfNull(partyBaseDto);
        ArgumentNullException.ThrowIfNull(invoiceBaseDto);
        return new()
        {
            Website = partyBaseDto.Website,
            LogoReferenceId = partyBaseDto.LogoReferenceId,
            EndpointId = new() { SchemeId = "EM", Content = partyBaseDto.RegistrationName },
            PartyName = new() { Name = partyBaseDto.Name },
            PostalAddress = new()
            {
                StreetName = partyBaseDto.StreetName,
                City = partyBaseDto.City,
                PostCode = partyBaseDto.PostCode,
                Country = new() { IdentificationCode = partyBaseDto.CountryCode },
            },
            PartyLegalEntity = new() { RegistrationName = partyBaseDto.RegistrationName },
            PartyTaxScheme = new()
            {
                CompanyId = partyBaseDto.TaxId,
                TaxScheme = new()
                {
                    Id = new() { Content = invoiceBaseDto.GlobalTaxScheme }
                }
            },
            Contact = new()
            {
                Name = partyBaseDto.Name,
                Email = partyBaseDto.Email,
                Telephone = partyBaseDto.Telefone,
            }
        };
    }

    /// <summary>
    /// Map BuyerPartyToXml
    /// </summary>
    /// <param name="partyBaseDto"></param>
    /// <returns></returns>
    protected virtual XmlParty BuyerPartyToXml(IPartyBaseDto partyBaseDto)
    {
        ArgumentNullException.ThrowIfNull(partyBaseDto);
        return new()
        {
            Website = partyBaseDto.Website,
            LogoReferenceId = partyBaseDto.LogoReferenceId,
            EndpointId = new() { SchemeId = "EM", Content = partyBaseDto.RegistrationName },
            PartyName = new() { Name = partyBaseDto.Name },
            PostalAddress = new()
            {
                StreetName = partyBaseDto.StreetName,
                City = partyBaseDto.City,
                PostCode = partyBaseDto.PostCode,
                Country = new() { IdentificationCode = partyBaseDto.CountryCode },
            },
            PartyLegalEntity = new() { RegistrationName = partyBaseDto.RegistrationName },
            Contact = new()
            {
                Name = partyBaseDto.Name,
                Email = partyBaseDto.Email,
                Telephone = partyBaseDto.Telefone,
            }
        };
    }

    /// <summary>
    /// Map LineToDto
    /// </summary>
    /// <param name="xmlLine"></param>
    /// <returns></returns>
    protected virtual IInvoiceLineBaseDto LineToDto(XmlInvoiceLine xmlLine)
    {
        ArgumentNullException.ThrowIfNull(xmlLine);
        return new InvoiceLineBaseDto()
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

    /// <summary>
    /// Map LineToXml
    /// </summary>
    /// <param name="dtoLine"></param>
    /// <param name="currencyId"></param>
    /// <param name="taxCategory"></param>
    /// <param name="taxScheme"></param>
    /// <param name="tax"></param>
    /// <returns></returns>
    protected virtual XmlInvoiceLine LineToXml(IInvoiceLineBaseDto dtoLine,
                                            string currencyId,
                                            string taxCategory,
                                            string taxScheme,
                                            double tax)
    {
        ArgumentNullException.ThrowIfNull(dtoLine);
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
