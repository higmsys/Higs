using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvTokenTest_IsAnyOf : CsvTokenTest
{
    public CsvTokenTest_IsAnyOf(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [InlineData("Returns ture when specified the same type.", CsvTokenType.Delimiter, CsvTokenType.Delimiter)]
    [InlineData("Returns true when specified the same type and the other type.", CsvTokenType.Delimiter, CsvTokenType.Delimiter, CsvTokenType.FieldData)]
    public void ReturnsTrueWhenSameType(string title, CsvTokenType type, params CsvTokenType[] otherTypes)
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var token = new CsvToken(type, "");

        // --------------------------
        // Invoke
        // --------------------------
        var res = token.IsAnyOf(otherTypes);

        // --------------------------
        // Inspecting
        // --------------------------
        AssertTrue(res, title);
    }

    [Theory]
    [InlineData("Returns false when specified the other type.", CsvTokenType.Delimiter, CsvTokenType.FieldData)]
    [InlineData("Returns false when specified the other types.", CsvTokenType.Delimiter, CsvTokenType.EndOfFile, CsvTokenType.FieldData)]
    public void ReturnsFalseWhenTheOtherType(string title, CsvTokenType type, params CsvTokenType[] otherTypes)
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var token = new CsvToken(type, "");

        // --------------------------
        // Invoke
        // --------------------------
        var res = token.IsAnyOf(otherTypes);

        // --------------------------
        // Inspecting
        // --------------------------
        AssertFalse(res, title);
    }

    [Fact]
    public void ReturnsFalseWhenNothingIsSpecified()
    {

        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var token = new CsvToken(CsvTokenType.Delimiter, ",");

        // --------------------------
        // Invoke
        // --------------------------
        var res = token.IsAnyOf();

        // --------------------------
        // Inspecting
        // --------------------------
        AssertFalse(res);
    }
}