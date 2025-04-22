using System.Net;
using System.Net.Http.Headers;
using System.Text;
using HtmlAgilityPack;

namespace pax.XRechnung.NET.Validator;

public static class KositValidator
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

            var response = await client.PostAsync(null as Uri, content);
            var result = await response.Content.ReadAsStringAsync();
            ArgumentNullException.ThrowIfNull(result);
            var tables = ParseValidatorResponse(result);
            return ToValidationResult(tables);
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

    private static SchematronValidationResult ToValidationResult(List<HtmlTable> htmlTables)
    {
        if (htmlTables.Count < 2)
            throw new InvalidOperationException("Expected at least 2 tables.");

        var result = new SchematronValidationResult();

        // First table: Summary of steps
        var summaryTable = htmlTables[0];
        foreach (var row in summaryTable.TableRows)
        {
            if (row.Count < 4) continue;

            result.Steps.Add(new ValidationStep
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
                result.Messages.Add(currentMessage);
                currentMessage = null;
            }
        }

        result.IsValid = result.Steps.Sum(s => s.Errors) == 0;
        return result;
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