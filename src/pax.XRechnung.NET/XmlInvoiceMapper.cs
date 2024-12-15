
using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET;

/// <summary>
/// XmlInvoiceMapper
/// </summary>
public static class XmlInvoiceMapper
{
    /// <summary>
    /// Map XmlInvoice to InvoiceDto
    /// </summary>
    /// <param name="xmlInvoice"></param>
    /// <returns></returns>
    public static InvoiceDto GetInvoiceDto(XmlInvoice xmlInvoice)
    {
        ArgumentNullException.ThrowIfNull(xmlInvoice);

        return new InvoiceDto
        {
            Id = xmlInvoice.Id.Content,
            InvoiceTypeCode = xmlInvoice.InvoiceTypeCode,
            BuyerReference = xmlInvoice.BuyerReference,
            DocumentCurrencyCode = xmlInvoice.DocumentCurrencyCode,
            IssueDate = xmlInvoice.IssueDate,
            DueDate = xmlInvoice.DueDate,
            Note = xmlInvoice.Note ?? "",
            SellerParty = MapPartyToDto(xmlInvoice.SellerParty.Party),
            BuyerParty = MapPartyToDto(xmlInvoice.BuyerParty.Party),
            PaymentMeans = new PaymentMeansDto
            {
                PaymentMeansTypeCode = xmlInvoice.PaymentMeans.PaymentMeansTypeCode,
                PayeeFinancialAccount = xmlInvoice.PaymentMeans.PayeeFinancialAccount?
                    .Select(account => new FinancialAccountDto
                    {
                        Id = account.Id?.Content ?? "",
                        Name = account.Name ?? ""
                    }).ToList() ?? []
            },
            LegalMonetaryTotal = new MonetaryTotalDto
            {
                LineExtensionAmount = MapAmountToDto(xmlInvoice.LegalMonetaryTotal.LineExtensionAmount),
                TaxExclusiveAmount = MapAmountToDto(xmlInvoice.LegalMonetaryTotal.TaxExclusiveAmount),
                TaxInclusiveAmount = MapAmountToDto(xmlInvoice.LegalMonetaryTotal.TaxInclusiveAmount),
                PayableAmount = MapAmountToDto(xmlInvoice.LegalMonetaryTotal.PayableAmount)
            },
            TaxTotal = new TaxTotalDto
            {
                TaxAmount = MapAmountToDto(xmlInvoice.TaxTotal.TaxAmount),
                TaxSubTotal = xmlInvoice.TaxTotal.TaxSubTotal.Select(taxSubTotal => new TaxSubTotalDto
                {
                    TaxableAmount = MapAmountToDto(taxSubTotal.TaxableAmount),
                    TaxAmount = MapAmountToDto(taxSubTotal.TaxAmount),
                    TaxCategory = new TaxCategoryDto
                    {
                        Id = taxSubTotal.TaxCategory.Id?.Content ?? "",
                        Percent = taxSubTotal.TaxCategory.Percent,
                        TaxScheme = new TaxSchemeDto
                        {
                            CompanyId = "",
                            TaxSchemeId = taxSubTotal.TaxCategory.TaxScheme.Id?.Content ?? ""
                        }
                    }
                }).ToList()
            }
        };
    }

    private static AmountDto MapAmountToDto(Amount amount)
    {
        return new AmountDto
        {
            CurrencyID = amount.CurrencyID,
            Value = amount.Value
        };
    }

    private static PartyDto MapPartyToDto(XmlSeller party)
    {
        return new PartyDto
        {
            EndpointId = party.EndpointId.Content,
            PartyName = party.PartyName.Name,
            PostalAddress = new AddressDto
            {
                StreetName = party.PostalAddress.StreetName ?? "",
                City = party.PostalAddress.City,
                PostCode = party.PostalAddress.PostCode
            },
            PartyTaxScheme = new TaxSchemeDto
            {
                CompanyId = party.PartyTaxScheme?.CompanyId ?? "",
                TaxSchemeId = party.PartyTaxScheme?.TaxScheme.Id.Content ?? ""
            },
            PartyLegalEntity = new LegalEntityDto
            {
                RegistrationName = party.PartyLegalEntity?.RegistrationName ?? ""
            },
            Contact = new ContactDto
            {
                Name = party.Contact?.Name ?? "",
                Telephone = party.Contact?.Telephone ?? "",
                Email = party.Contact?.Email ?? ""
            }
        };
    }

    private static PartyDto MapPartyToDto(XmlBuyer party)
    {
        return new PartyDto
        {
            EndpointId = party.EndpointId.Content,
            PartyName = party.PartyName.Name,
            PostalAddress = new AddressDto
            {
                StreetName = party.PostalAddress?.StreetName ?? "",
                City = party.PostalAddress?.City ?? "",
                PostCode = party.PostalAddress?.PostCode ?? ""
            },
            PartyTaxScheme = new TaxSchemeDto
            {
                CompanyId = party.PartyTaxScheme?.CompanyId ?? "",
                TaxSchemeId = party.PartyTaxScheme?.TaxScheme.Id?.Content ?? ""
            },
            PartyLegalEntity = new LegalEntityDto
            {
                RegistrationName = party.PartyLegalEntity.RegistrationName
            },
            Contact = new ContactDto
            {
                Name = party.Contact?.Name ?? "",
                Telephone = party.Contact?.Telephone ?? "",
                Email = party.Contact?.Email ?? ""
            }
        };
    }

    /// <summary>
    /// Map InvoiceDto to XmlInvoice
    /// </summary>
    /// <param name="invoiceDto"></param>
    /// <returns></returns>
    public static XmlInvoice GetXmlInvoice(InvoiceDto invoiceDto)
    {
        ArgumentNullException.ThrowIfNull(invoiceDto);
        return new XmlInvoice
        {
            Id = new Identifier { Content = invoiceDto.Id },
            InvoiceTypeCode = invoiceDto.InvoiceTypeCode,
            BuyerReference = invoiceDto.BuyerReference,
            DocumentCurrencyCode = invoiceDto.DocumentCurrencyCode,
            IssueDate = invoiceDto.IssueDate,
            DueDate = invoiceDto.DueDate,
            Note = invoiceDto.Note,
            SellerParty = MapSellerParty(invoiceDto.SellerParty),
            BuyerParty = MapBuyerParty(invoiceDto.BuyerParty),
            PaymentMeans = new XmlPaymentInstructions
            {
                PaymentMeansTypeCode = invoiceDto.PaymentMeans.PaymentMeansTypeCode,
                PayeeFinancialAccount = invoiceDto.PaymentMeans.PayeeFinancialAccount
                    .Select(account => new XmlCreditTransfer
                    {
                        Id = new Identifier { Content = account.Id },
                        Name = account.Name
                    }).ToList()
            },
            LegalMonetaryTotal = new XmlDocumentTotals
            {
                LineExtensionAmount = MapAmount(invoiceDto.LegalMonetaryTotal.LineExtensionAmount),
                TaxExclusiveAmount = MapAmount(invoiceDto.LegalMonetaryTotal.TaxExclusiveAmount),
                TaxInclusiveAmount = MapAmount(invoiceDto.LegalMonetaryTotal.TaxInclusiveAmount),
                PayableAmount = MapAmount(invoiceDto.LegalMonetaryTotal.PayableAmount)
            },
            TaxTotal = new XmlVatBreakdown
            {
                TaxAmount = MapAmount(invoiceDto.TaxTotal.TaxAmount),
                TaxSubTotal = invoiceDto.TaxTotal.TaxSubTotal.Select(taxSubTotal => new XmlTaxSubTotal
                {
                    TaxableAmount = MapAmount(taxSubTotal.TaxableAmount),
                    TaxAmount = MapAmount(taxSubTotal.TaxAmount),
                    TaxCategory = new XmlTaxCategory
                    {
                        Id = new Identifier { Content = taxSubTotal.TaxCategory.Id },
                        Percent = taxSubTotal.TaxCategory.Percent,
                        TaxScheme = new XmlTaxScheme
                        {
                            Id = new Identifier { Content = taxSubTotal.TaxCategory.TaxScheme.TaxSchemeId }
                        }
                    }
                }).ToList()
            }
        };
    }

    private static Amount MapAmount(AmountDto amountDto)
    {
        return new Amount()
        {
            CurrencyID = amountDto.CurrencyID,
            Value = amountDto.Value
        };
    }

    private static XmlSellerParty MapSellerParty(PartyDto partyDto)
    {
        return new XmlSellerParty()
        {
            Party = new XmlSeller()
            {
                EndpointId = new XmlEndpointId() { Content = partyDto.EndpointId },
                PartyName = new XmlPartyName() { Name = partyDto.PartyName },
                PostalAddress = new XmlPostalAddress()
                {
                    StreetName = partyDto.PostalAddress.StreetName,
                    City = partyDto.PostalAddress.City,
                    PostCode = partyDto.PostalAddress.PostCode,
                },
                PartyTaxScheme = new XmlPartyTaxScheme()
                {
                    CompanyId = partyDto.PartyTaxScheme.CompanyId,
                    TaxScheme = new XmlTaxScheme()
                    {
                        Id = new Identifier() { Content = partyDto.PartyTaxScheme.TaxSchemeId }
                    }
                },
                PartyLegalEntity = new XmlPartyLegalEntity()
                {
                    RegistrationName = partyDto.PartyLegalEntity.RegistrationName
                },
                Contact = new XmlContact()
                {
                    Name = partyDto.Contact.Name,
                    Telephone = partyDto.Contact.Telephone,
                    Email = partyDto.Contact.Email
                },
            }
        };
    }

    private static XmlBuyerParty MapBuyerParty(PartyDto partyDto)
    {
        return new XmlBuyerParty()
        {
            Party = new XmlBuyer()
            {
                EndpointId = new XmlEndpointId() { Content = partyDto.EndpointId },
                PartyName = new XmlPartyName() { Name = partyDto.PartyName },
                PostalAddress = new XmlPostalAddress()
                {
                    StreetName = partyDto.PostalAddress.StreetName,
                    City = partyDto.PostalAddress.City,
                    PostCode = partyDto.PostalAddress.PostCode,
                },
                PartyTaxScheme = new XmlPartyTaxScheme()
                {
                    CompanyId = partyDto.PartyTaxScheme.CompanyId,
                    TaxScheme = new XmlTaxScheme()
                    {
                        Id = new Identifier() { Content = partyDto.PartyTaxScheme.TaxSchemeId }
                    }
                },
                PartyLegalEntity = new XmlPartyLegalEntity()
                {
                    RegistrationName = partyDto.PartyLegalEntity.RegistrationName
                },
                Contact = new XmlContact()
                {
                    Name = partyDto.Contact.Name,
                    Telephone = partyDto.Contact.Telephone,
                    Email = partyDto.Contact.Email
                },
            }
        };
    }
}