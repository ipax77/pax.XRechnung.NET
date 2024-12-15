
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
}
