using pax.XRechnung.NET.CodeListModel;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace pax.XRechnung.NET;

/// <summary>
/// CodeListRepository
/// </summary>
public static partial class CodeListRepository
{
    private static readonly ConcurrentDictionary<string, CodeList?> CodeListCache = [];
    private const string CodeListRessourceBasePath = "pax.XRechnung.NET.Resources.CodeListFiles.";
    private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="listId">The base list ID (e.g., "UNTDID_1001").</param>
    /// <param name="code">Code to validate against the listId (e.g. 380)</param>
    /// <returns></returns>
    public static bool IsValidCode(string listId, string code)
    {
        var codeList = GetCodeList(listId);
        if (codeList == null)
        {
            return false;
        }
        return codeList.DataDictionary.ContainsKey(code);
    }

    /// <summary>
    /// Get CodeList by ID, defaulting to the highest version.
    /// </summary>
    /// <param name="listId">The base list ID (e.g., "UNTDID_1001").</param>
    /// <param name="version">Optional: Specify the version (e.g., 4).</param>
    /// <returns>The deserialized CodeList or null if not found.</returns>
    public static CodeList? GetCodeList(string listId, int? version = null)
    {
        ArgumentNullException.ThrowIfNull(listId);

        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = ResolveResourceName(assembly, listId, version);

        if (string.IsNullOrEmpty(resourceName))
        {
            return null;
        }

        if (CodeListCache.TryGetValue(resourceName, out var cachedCodeList))
        {
            return cachedCodeList;
        }

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            return null;
        }

        var codeList = JsonSerializer.Deserialize<CodeList>(stream, jsonOptions);
        SetDataDictionary(codeList);
        CodeListCache.AddOrUpdate(resourceName, codeList, (k, v) => v = codeList);
        return codeList;
    }

    private static void SetDataDictionary(CodeList? codeList)
    {
        if (codeList is null)
        {
            return;
        }
        codeList.DataDictionary = codeList.Data.Where(row => row.Count > 0 && !string.IsNullOrEmpty(row[0]))
                .ToDictionary(
                    row => row[0]!,
                    row => row.Skip(1).ToList()
                );
    }

    private static string? ResolveResourceName(Assembly assembly, string listId, int? version)
    {
        var allResources = assembly.GetManifestResourceNames();
        var matchingResources = allResources
            .Where(res => res.StartsWith($"{CodeListRessourceBasePath}{listId}", StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matchingResources.Count == 0)
        {
            return null;
        }

        if (version.HasValue)
        {
            // Match exact version
            return matchingResources
                .FirstOrDefault(res => res.EndsWith($"_{version}.json", StringComparison.OrdinalIgnoreCase));
        }

        // Default to the highest version
        return matchingResources
            .OrderByDescending(res => ExtractVersionFromResourceName(res))
            .FirstOrDefault();
    }

    private static int ExtractVersionFromResourceName(string resourceName)
    {
        var match = RessNameRx().Match(resourceName);
        return match.Success && int.TryParse(match.Groups[1].Value, out var version) ? version : 0;
    }

    [GeneratedRegex(@"_(\d+)\.json$")]
    private static partial Regex RessNameRx();
}
