using pax.XRechnung.NET.XmlModels;

namespace pax.XRechnung.NET.BaseDtos;

/// <summary>
/// InvoiceMapper abstraction
/// </summary>
public abstract class InvoiceMapperBase<TInvoiceDto, TDocumentReferenceDto, TSellerPartyDto, TBuyerPartyDto, TPayment, TLineDto> :
    IInvoiceMapper<TInvoiceDto, TDocumentReferenceDto, TSellerPartyDto, TBuyerPartyDto, TPayment, TLineDto>
    where TInvoiceDto : IInvoiceBaseDto, new()
    where TDocumentReferenceDto : IDocumentReferenceBaseDto, new()
    where TSellerPartyDto : IPartyBaseDto, new()
    where TBuyerPartyDto : IPartyBaseDto, new()
    where TPayment : IPaymentMeansBaseDto, new()
    where TLineDto : IInvoiceLineBaseDto, new()
{
#pragma warning disable CA1051 // Do not declare visible instance fields
    /// <summary>
    /// DocumentReferenceMapper
    /// </summary>
    protected readonly DocumentReferenceMapperBase<TDocumentReferenceDto> DocumentReferenceMapper;
    /// <summary>
    /// SellerPartyMapper
    /// </summary>
    protected readonly InvoiceSellerPartyMapperBase<TSellerPartyDto> SellerPartyMapper;
    /// <summary>
    /// BuyerPartyMapper
    /// </summary>
    protected readonly InvoiceBuyerPartyMapperBase<TBuyerPartyDto> BuyerPartyMapper;
    /// <summary>
    /// PaymentMeansMapper
    /// </summary>
    protected readonly PaymentMeansMapperBase<TPayment> PaymentMeansMapper;
    /// <summary>
    /// InvoiceLineMapper
    /// </summary>
    protected readonly InvoiceLineMapperBase<TLineDto> InvoiceLineMapper;
#pragma warning restore CA1051 // Do not declare visible instance fields


    /// <summary>
    /// InvoiceMapperBase
    /// </summary>
    /// <param name="documentReferenceMapperBase"></param>
    /// <param name="sellerPartyMapper"></param>
    /// <param name="buyerPartyMapper"></param>
    /// <param name="paymentMeansMapper"></param>
    /// <param name="invoiceLineMapper"></param>
    protected InvoiceMapperBase(
        DocumentReferenceMapperBase<TDocumentReferenceDto> documentReferenceMapperBase,
        InvoiceSellerPartyMapperBase<TSellerPartyDto> sellerPartyMapper,
        InvoiceBuyerPartyMapperBase<TBuyerPartyDto> buyerPartyMapper,
        PaymentMeansMapperBase<TPayment> paymentMeansMapper,
        InvoiceLineMapperBase<TLineDto> invoiceLineMapper)
    {
        DocumentReferenceMapper = documentReferenceMapperBase;
        SellerPartyMapper = sellerPartyMapper;
        BuyerPartyMapper = buyerPartyMapper;
        PaymentMeansMapper = paymentMeansMapper;
        InvoiceLineMapper = invoiceLineMapper;
    }

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
    /// smallBusiness Tax Category § 19UStG
    /// </summary>
    protected const string DefaultSmallBusinessTaxCategory = "E";
    /// <summary>
    /// smallBusinessText § 19UStG
    /// </summary>
    protected const string smallBusinessText = "Kein Ausweis von Umsatzsteuer, da Kleinunternehmer gemäß § 19UStG";

    /// <summary>
    /// Map xmlInvoice to T
    /// </summary>
    /// <param name="xmlInvoice"></param>
    /// <returns></returns>
    public virtual TInvoiceDto FromXml(XmlInvoice xmlInvoice)
    {
        ArgumentNullException.ThrowIfNull(xmlInvoice);

        var xmlTaxCategory = xmlInvoice.TaxTotal.TaxSubTotal.FirstOrDefault()?.TaxCategory;
        var tax = xmlTaxCategory?.Percent ?? DefaultTax;
        var taxScheme = xmlTaxCategory?.TaxScheme.Id.Content ?? DefaultTaxScheme;
        var taxCategory = xmlTaxCategory?.Id.Content ??
            (tax == 0 ? DefaultSmallBusinessTaxCategory : DefaultTaxCategory);

        return new()
        {
            GlobalTaxCategory = taxCategory,
            GlobalTaxScheme = taxScheme,
            GlobalTax = (double)tax,
            Id = xmlInvoice.Id.Content,
            IssueDate = InvoiceMapperUtils.GetDateTime(xmlInvoice.IssueDate),
            DueDate = InvoiceMapperUtils.GetDateTime(xmlInvoice.DueDate?.Value),
            InvoiceTypeCode = xmlInvoice.InvoiceTypeCode,
            DocumentCurrencyCode = xmlInvoice.DocumentCurrencyCode,
            BuyerReference = xmlInvoice.BuyerReference,
            AdditionalDocumentReferences = xmlInvoice.AdditionalDocumentReferences
                .Select(s => AdditionalDocumentToDto(s)).ToList(),
            SellerParty = SellerPartyToDto(xmlInvoice.SellerParty.Party),
            BuyerParty = BuyerPartyToDto(xmlInvoice.BuyerParty.Party),
            PaymentMeans = PaymentMeansMapper.FromXml(xmlInvoice.PaymentMeans),
            PaymentMeansTypeCode = xmlInvoice.PaymentMeans.PaymentMeansTypeCode,
            PaymentTermsNote = xmlInvoice.PaymentTerms?.Note ?? string.Empty,
            PayableAmount = (double)(xmlInvoice.LegalMonetaryTotal.PayableAmount?.Value ?? 0),
            InvoiceLines = xmlInvoice.InvoiceLines.Select(s => LineToDto(s)).ToList(),

        };
    }
    /// <summary>
    /// Map T to XmlInvoice
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public virtual XmlInvoice ToXml(TInvoiceDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        decimal taxRate = InvoiceMapperUtils.RoundAmount(dto.GlobalTax / 100.0);
        decimal taxExclusiveAmount = InvoiceMapperUtils.RoundAmount(dto.InvoiceLines
            .Sum(s => InvoiceMapperUtils
                .RoundAmount(InvoiceMapperUtils.RoundAmount(s.Quantity)
                    * InvoiceMapperUtils.RoundAmount(s.UnitPrice))));

        decimal payableAmount = InvoiceMapperUtils.RoundAmount(taxExclusiveAmount + taxExclusiveAmount * taxRate);
        bool isSmallBusiness = taxRate == 0; // keine Umsatzsteuer nach § 19 UStG
        decimal taxAmount = isSmallBusiness ? 0 : InvoiceMapperUtils.RoundAmount(payableAmount - taxExclusiveAmount);

        if (isSmallBusiness)
        {
            dto.GlobalTaxCategory = DefaultSmallBusinessTaxCategory;
        }

        var xml = new XmlInvoice()
        {
            Id = new() { Content = dto.Id },
            IssueDate = new DateOnly(dto.IssueDate.Year, dto.IssueDate.Month, dto.IssueDate.Day),
            InvoiceTypeCode = dto.InvoiceTypeCode,
            DocumentCurrencyCode = dto.DocumentCurrencyCode,
            BuyerReference = dto.BuyerReference,
            AdditionalDocumentReferences = dto.AdditionalDocumentReferences.Select(s => AdditionalDocumentToXml(s))
                .ToList(),

            SellerParty = new() { Party = SellerPartyToXml(dto.SellerParty, dto) },
            BuyerParty = new() { Party = BuyerPartyToXml(dto.BuyerParty) },
            PaymentMeans = PaymentMeansMapper.ToXml(dto.PaymentMeans, string.IsNullOrEmpty(dto.PaymentMeansTypeCode)
                 ? DefaultPaymentMeansTypeCode : dto.PaymentMeansTypeCode),
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
                            Percent = InvoiceMapperUtils.RoundAmount(dto.GlobalTax),
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
            InvoiceLines = dto.InvoiceLines
                .Select(s =>
                    LineToXml(s, dto.DocumentCurrencyCode, dto.GlobalTaxCategory, dto.GlobalTaxScheme, dto.GlobalTax))
                .ToList(),
        };

        if (isSmallBusiness)
        {
            if (xml.SellerParty.Party.PartyTaxScheme is null)
            {
                xml.SellerParty.Party.PartyTaxScheme = new();
            }
            xml.SellerParty.Party.PartyLegalEntity.CompanyLegalForm = smallBusinessText;
            xml.TaxTotal.TaxSubTotal[0].TaxCategory.ExemptionReason = smallBusinessText;
            xml.TaxTotal.TaxSubTotal[0].TaxCategory.Id.Content = "E";

        }

        return xml;
    }

    /// <summary>
    /// Map SellerParty to Dto
    /// </summary>
    /// <param name="xmlParty"></param>
    /// <returns></returns>
    protected virtual IPartyBaseDto SellerPartyToDto(XmlParty xmlParty) => SellerPartyMapper.FromXml(xmlParty);

    /// <summary>
    /// Map SellerParty to Dto
    /// </summary>
    /// <param name="xmlParty"></param>
    /// <returns></returns>
    protected virtual IPartyBaseDto BuyerPartyToDto(XmlParty xmlParty) => BuyerPartyMapper.FromXml(xmlParty);

    /// <summary>
    /// Map SellerPartyToXml
    /// </summary>
    /// <param name="partyBaseDto"></param>
    /// <param name="invoiceBaseDto"></param>
    /// <returns></returns>
    protected virtual XmlParty SellerPartyToXml(IPartyBaseDto partyBaseDto, IInvoiceBaseDto invoiceBaseDto)
        => SellerPartyMapper.ToXml(partyBaseDto, invoiceBaseDto);

    /// <summary>
    /// Map BuyerPartyToXml
    /// </summary>
    /// <param name="partyBaseDto"></param>
    /// <returns></returns>
    protected virtual XmlParty BuyerPartyToXml(IPartyBaseDto partyBaseDto) => BuyerPartyMapper.ToXml(partyBaseDto);

    /// <summary>
    /// Map LineToDto
    /// </summary>
    /// <param name="xmlLine"></param>
    /// <returns></returns>
    protected virtual IInvoiceLineBaseDto LineToDto(XmlInvoiceLine xmlLine) => InvoiceLineMapper.FromXml(xmlLine);

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
        => InvoiceLineMapper.ToXml(dtoLine, currencyId, taxCategory, taxScheme, tax);

    /// <summary>
    /// Map AdditionalDocumentReference to DTO
    /// </summary>
    /// <param name="xmlDoc"></param>
    /// <returns></returns>
    protected virtual IDocumentReferenceBaseDto AdditionalDocumentToDto(XmlAdditionalDocumentReference xmlDoc)
        => DocumentReferenceMapper.FromXml(xmlDoc);

    /// <summary>
    /// Map AdditionalDocumentReference to Xml
    /// </summary>
    /// <param name="docDto"></param>
    /// <returns></returns>
    protected virtual XmlAdditionalDocumentReference AdditionalDocumentToXml(IDocumentReferenceBaseDto docDto)
        => DocumentReferenceMapper.ToXml(docDto);

}
