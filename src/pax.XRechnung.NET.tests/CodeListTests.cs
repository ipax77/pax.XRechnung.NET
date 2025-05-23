
using pax.XRechnung.NET.CodeListModel;

namespace pax.XRechnung.NET.tests;

[TestClass]
public class CodeListTests
{
    [TestMethod]
    public void CanValidateCode()
    {
        var listId = "UNTDID_1001";
        var code = "380";
        var isValid = CodeListRepository.IsValidCode(listId, code);
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void CanSerializeDescription()
    {
        CodeList? codeList = CodeListRepository.GetCodeList("UNTDID_1001");
        Assert.IsNotNull(codeList);
        var desc = "Die Codeliste basiert auf der Codeliste 1001 (Document name code) des United Trade Data Interchange Directory (UNTDID). Die vorliegende Version der Codeliste entspricht in Umfang und Inhalt der Einträge (Codes, Namen und Beschreibung) der zugrundeliegenden Codeliste 1001 (Document name code) des United Trade Data Interchange Directory (UNTDID). Die Codeliste kann im Zusammenhang mit EN16931-1:2017 und der darauf basierenden XRechnung (Standard und Extension) verwendeten werden. EN16931 Annex A definiert eine Untermenge dieser Liste zur Verwendung. Des Weiteren enthält diese Codeliste alle Einträge, die für die Nutzung im Kontext des Standards XBestellung benötigt werden.";
        var current = codeList.Metadaten.Beschreibung.FirstOrDefault()?.Value;
        Assert.AreEqual(desc, current);
    }
}
