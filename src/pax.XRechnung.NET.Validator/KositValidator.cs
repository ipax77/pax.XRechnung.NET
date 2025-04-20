using System.Net.Http.Headers;
using System.Text;
using System.Xml.Serialization;

namespace pax.XRechnung.NET.Validator;

public static class KositValidator
{
    private static readonly string validatorUri = "http://localhost:8080";
    public static async Task<string> Validate(string xmlText, Uri? kostiUri = null)
    {
        try
        {
            using var client = new HttpClient();
            client.BaseAddress = kostiUri ?? new Uri(validatorUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));


            var content = new StringContent(xmlText, Encoding.UTF8, "application/xml");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");

            var response = await client.PostAsync("/", content);
            // response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Validation failed", ex);
        }
    }

    public static bool ParseValidatorResponse(string response)
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
        


        return true;
    }
}
