using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvTokenTest_Equals : CsvTokenTest
{
    public CsvTokenTest_Equals(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [MemberData(nameof(ReturnsTrueData_TheSameObject))]
    [MemberData(nameof(ReturnsTrueData_HaveTheSameContents))]
    public void ReturnsTrue(string title, CsvToken token1, CsvToken token2)
    {
        WriteLine();
        WriteLine(title);
        WriteLine($"token1: {token1}");
        WriteLine($"token2: {token2}");
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------

        // --------------------------
        // Invoke
        // --------------------------
        var res = token1.Equals(token2);

        // --------------------------
        // Inspecting
        // --------------------------
        AssertTrue(res);
    }

    public static IEnumerable<object[]> ReturnsTrueData_TheSameObject()
    {
        var token = new CsvToken(CsvTokenType.FieldData, "A");
        yield return new object[] {
            "Returns True when the same object.",
            token,
            token,
        };
    }

    public static IEnumerable<object[]> ReturnsTrueData_HaveTheSameContents()
    {
        var token1 = new CsvToken(CsvTokenType.FieldData, "A");
        var token2 = new CsvToken(CsvTokenType.FieldData, "A");
        yield return new object[] {
            "Returns True when the tokens have the same contents.",
            token1,
            token2,
        };
    }

    [Theory]
    [MemberData(nameof(ReturnsFalseData_HaveDifferenctContents))]
    [MemberData(nameof(ReturnsFalseData_OtherClassObject))]
    [MemberData(nameof(ReturnsFalseData_SpecifiedNull))]
    public void ReturnsFalse(string title, CsvToken token1, object? token2)
    {
        WriteLine();
        WriteLine(title);
        WriteLine($"token1: {token1}");
        WriteLine($"token2: {token2}");
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------

        // --------------------------
        // Invoke
        // --------------------------
        var res = token1.Equals(token2);

        // --------------------------
        // Inspecting
        // --------------------------
        AssertFalse(res);
    }

    public static IEnumerable<object[]> ReturnsFalseData_HaveDifferenctContents()
    {
        yield return new object[] {
            "Returns False when the type is difference.",
            new CsvToken(CsvTokenType.FieldData, ","),
            new CsvToken(CsvTokenType.Delimiter, ","),
        };

        yield return new object[] {
            "Returns False when the value is difference.",
            new CsvToken(CsvTokenType.FieldData, "A"),
            new CsvToken(CsvTokenType.FieldData, "A "),
        };
    }

    public static IEnumerable<object[]> ReturnsFalseData_OtherClassObject()
    {
        yield return new object[] {
            "Returns False when the object isn't CsvToken.",
            new CsvToken(CsvTokenType.Delimiter, ","),
            new { Type = CsvTokenType.Delimiter, Value = ","},
        };
    }

    public static IEnumerable<object?[]> ReturnsFalseData_SpecifiedNull()
    {
        yield return new object?[] {
            "Returns False when specfied null.",
            new CsvToken(CsvTokenType.FieldData, ","),
            null as CsvToken,
        };
    }

}