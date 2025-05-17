using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace pax.XRechnung.NET.CodeListModel;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class CodeListMetadata

{
    public string Kennung { get; set; } = string.Empty;
    public string KennungInhalt { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public List<Name> NameKurz { get; set; } = [];
    public List<Name> NameLang { get; set; } = [];
    public string NameTechnisch { get; set; } = string.Empty;
    public List<Name> HerausgebernameLang { get; set; } = [];
    public List<Name> HerausgebernameKurz { get; set; } = [];
    public List<Name> Beschreibung { get; set; } = [];
    public List<Name> VersionBeschreibung { get; set; } = [];
    public List<Name> AenderungZurVorversion { get; set; } = [];
    public string HandbuchVersion { get; set; } = string.Empty;
    public bool XoevHandbuch { get; set; }
    public List<string> Bezugsorte { get; set; } = [];
}

public class Name
{
    public string Value { get; set; } = string.Empty;
}

public class Spalte
{
    public string SpaltennameLang { get; set; } = string.Empty;
    public string SpaltennameTechnisch { get; set; } = string.Empty;
    public string Datentyp { get; set; } = string.Empty;
    public bool CodeSpalte { get; set; }
    public string Verwendung { get; set; } = string.Empty;
    public bool EmpfohleneCodeSpalte { get; set; }
}

public class CodeList
{
    public CodeListMetadata Metadaten { get; set; } = new();
    public List<Spalte> Spalten { get; set; } = [];
    [JsonPropertyName("daten")]
    public List<List<string?>> Data { get; set; } = [];

    public Dictionary<string, List<string?>> DataDictionary { get; set; } = [];

    internal void OnDeserializedMethod(StreamingContext context)
    {
        DataDictionary = Data
            .Where(row => row.Count > 0 && !string.IsNullOrEmpty(row[0]))
            .ToDictionary(
                row => row[0]!,
                row => row.Skip(1).ToList()
            );
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member