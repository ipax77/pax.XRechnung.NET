namespace pax.XRechnung.NET.Dtos;

/// <summary>
/// InvoiceDtoExtensions
/// </summary>
public static class InvoiceDtoExtensions
{
    /// <summary>
    /// Calculates the document totals based on the InvoiceLines.
    /// TaxTotal.Percent needs to be greater than zero.
    /// In this simplified dto abstraction the VAT percentage for all lines must be the same as the VAT total percentage.
    /// </summary>
    /// <param name="invoice"></param>
    public static void CalculateTotals(this InvoiceDto invoice)
    {
        ArgumentNullException.ThrowIfNull(invoice);

        if (invoice.TaxTotal.Percent <= 0)
        {
            throw new ArgumentException("TaxTotal.Percent must be greater than zero.");
        }

        if (invoice.InvoiceLines.Count == 0)
        {
            throw new InvalidOperationException("Invoice must have at least one invoice line.");
        }

        decimal totalNetAmount = 0;
        decimal vatPercent = invoice.TaxTotal.Percent / 100m;

        foreach (var line in invoice.InvoiceLines)
        {
            if (line.TaxPercent != invoice.TaxTotal.Percent)
            {
                throw new InvalidOperationException("The VAT percentage for all lines must be the same as the VAT total percentage.");
            }
            totalNetAmount += line.LineExtensionAmount;
        }

        decimal taxAmount = totalNetAmount * vatPercent;
        decimal totalGrossAmount = totalNetAmount + taxAmount;

        totalNetAmount = Math.Round(totalNetAmount, 2, MidpointRounding.AwayFromZero);
        taxAmount = Math.Round(taxAmount, 2, MidpointRounding.AwayFromZero);
        totalGrossAmount = Math.Round(totalGrossAmount, 2, MidpointRounding.AwayFromZero);

        invoice.LegalMonetaryTotal.LineExtensionAmount = totalNetAmount;
        invoice.LegalMonetaryTotal.TaxExclusiveAmount = totalNetAmount;
        invoice.TaxTotal.TaxableAmount = totalNetAmount;
        invoice.TaxTotal.TaxAmount = taxAmount;
        invoice.LegalMonetaryTotal.TaxInclusiveAmount = totalGrossAmount;
        invoice.LegalMonetaryTotal.PayableAmount = totalGrossAmount;
    }
}