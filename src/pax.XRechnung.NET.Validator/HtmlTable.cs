namespace pax.XRechnung.NET.Validator;

internal sealed record HtmlTable
{
    public List<string> HeaderCells { get; set; } = [];
    public List<List<string>> TableRows { get; set; } = [];
}
