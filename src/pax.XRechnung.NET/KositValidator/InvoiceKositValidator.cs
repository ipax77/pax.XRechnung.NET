using System.Net;
using System.Net.Http.Headers;
using System.Text;
using HtmlAgilityPack;

namespace pax.XRechnung.NET.KositValidator;

/// <summary>
/// KositValidator
/// </summary>
public static class InvoiceKositValidator
{
    private const string validatorUri = "http://localhost:8080";

    /// <summary>
    /// Validate xml string against Schmatrons
    /// Requires a running Kosit validation Server. See Readme for details.
    /// </summary>
    /// <param name="xmlText">xml text</param>
    /// <param name="kositUri">optional uri to the kosit validator, default is http://localhost:8080</param>
    public static async Task<SchematronValidationResult> Validate(string xmlText, Uri? kositUri = null)
    {
        try
        {
            using var client = new HttpClient();
            client.BaseAddress = kositUri ?? new Uri(validatorUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));


            using var content = new StringContent(xmlText, Encoding.UTF8, "application/xml");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");

            var response = await client.PostAsync(null as Uri, content).ConfigureAwait(false);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            ArgumentNullException.ThrowIfNull(result);
            SchematronValidationResult validationResult = new()
            {
                HttpStatusCode = response.StatusCode
            };

            SetValidationInfo(result, validationResult);
            var tables = ParseValidatorResponse(result);
            SetValidationResult(tables, validationResult);

            if (validationResult.HttpStatusCode == HttpStatusCode.OK &&
                validationResult.Steps.Sum(s => s.Errors) == 0)
            {
                validationResult.IsValid = true;
            }

            return validationResult;
        }
        catch (Exception ex)
        {
            SchematronValidationResult validationResult = new()
            {
                HttpStatusCode = HttpStatusCode.InternalServerError,
                Error = ex.Message,
                IsValid = false
            };
            return validationResult;
            throw;
        }
    }

    private static void SetValidationInfo(string html, SchematronValidationResult validationResult)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Extract Referenz
        var referenz = doc.DocumentNode
            .SelectSingleNode("//dt[text()='Referenz:']/following-sibling::dd[1]")?.InnerText.Trim();

        // Extract Zeitpunkt der Prüfung
        var zeitpunkt = doc.DocumentNode
            .SelectSingleNode("//dt[text()='Zeitpunkt der Prüfung:']/following-sibling::dd[1]")?.InnerText.Trim();

        // Extract Dokumenttyp (b.error inside dd)
        var dokumenttyp = doc.DocumentNode
            .SelectSingleNode("//dt[text()='Erkannter Dokumenttyp:']/following-sibling::dd[1]/b")?.InnerText.Trim();

        // Extract Konformität text
        var konformitaet = doc.DocumentNode
            .SelectNodes("//p[@class='important']")
            ?.FirstOrDefault(p => p.InnerText.Contains("Konformitätsprüfung", StringComparison.Ordinal))?
                .InnerText.Trim();

        // Extract Bewertung (error paragraph)
        var bewertung = doc.DocumentNode
            .SelectSingleNode("//p[@class='important error']")?.InnerText.Trim();

        validationResult.Conformity = konformitaet;
        validationResult.Evaluation = bewertung;
    }

    private static List<HtmlTable> ParseValidatorResponse(string response)
    {
        var lines = response.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        string? reportLine = null;
        foreach (var line in lines)
        {
            if (line.Contains("<body>", StringComparison.Ordinal))
            {
                reportLine = line;
                break;
            }
        }
        ArgumentNullException.ThrowIfNull(reportLine);
        reportLine += "</body>";
        HtmlDocument document = new();
        document.LoadHtml(reportLine);

        var tables = document.DocumentNode.SelectNodes("//table")?
            .Where(x => x != null);

        ArgumentNullException.ThrowIfNull(tables);

        List<HtmlTable> htmlTables = [];
        foreach (var table in tables)
        {
            var htmlTable = ExtractHtmlTable(table);
            if (htmlTable is not null)
            {
                htmlTables.Add(htmlTable);
            }
        }

        return htmlTables;
    }

    private static void SetValidationResult(List<HtmlTable> htmlTables, SchematronValidationResult validationResult)
    {
        if (htmlTables.Count < 2)
            return;

        // First table: Summary of steps
        var summaryTable = htmlTables[0];
        foreach (var row in summaryTable.TableRows)
        {
            if (row.Count < 4) continue;

            validationResult.Steps.Add(new ValidationStep
            {
                Step = row[0].Trim(),
                Errors = int.TryParse(row[1], out var e) ? e : 0,
                Warnings = int.TryParse(row[2], out var w) ? w : 0,
                Infos = int.TryParse(row[3], out var i) ? i : 0
            });
        }

        // Second table: Detailed messages
        var detailTable = htmlTables[1];
        ValidationMessage? currentMessage = null;

        foreach (var row in detailTable.TableRows)
        {
            if (row.Count == 4) // main row
            {
                currentMessage = new ValidationMessage
                {
                    Position = row[0].Trim(),
                    Code = row[1].Trim(),
                    Severity = row[2].Trim(),
                    Text = row[3].Trim()
                };
            }
            else if (row.Count == 1 && currentMessage != null) // path row
            {
                currentMessage.Path = row[0].Replace("Pfad: ", "", StringComparison.OrdinalIgnoreCase).Trim();
                validationResult.Messages.Add(currentMessage);
                currentMessage = null;
            }
        }
    }

    private static HtmlTable? ExtractHtmlTable(HtmlNode tableNode)
    {
        var rows = tableNode.SelectNodes(".//tr")?
         .Where(x => x != null);

        if (rows is null)
        {
            return null;
        }

        HtmlTable table = new();

        foreach (var row in rows)
        {
            var headerCells = row.SelectNodes("./th")?
                .Where(x => x != null);
            var bodyCells = row.SelectNodes("./td")?
                .Where(x => x != null);

            if (headerCells != null)
            {
                foreach (var headerCell in headerCells)
                {
                    table.HeaderCells.Add(headerCell.InnerHtml);
                }
            }

            if (bodyCells != null)
            {
                var rowCells = new List<string>();
                foreach (var bodyCell in bodyCells)
                {
                    rowCells.Add(bodyCell.InnerHtml);
                }
                table.TableRows.Add(rowCells);
            }
        }

        if (table.HeaderCells.Count == 0)
        {
            return null;
        }
        return table;
    }
}