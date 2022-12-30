using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvOptionTest_IsNewLineCharAt : CsvOptionTest
{
    public CsvOptionTest_IsNewLineCharAt(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [InlineData("\r\n", '\r', 0, true)]
    [InlineData("\r\n", '\n', 1, true)]
    [InlineData("\r\n", '\n', 0, false)]
    [InlineData("\n", '\n', 0, true)]
    [InlineData("\n", '\n', 1, false)]
    public void Test(string newLine, char c, int index, bool expected)
    {
        WriteLine();
        WriteLine($"new line : '{newLine}'");
        WriteLine($"target   : '{c}'");
        WriteLine($"index    : {index}");
        WriteLine($"expected : '{expected}'");
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var option = new CsvOption { NewLine = newLine, };

        // --------------------------
        // Invoke
        // --------------------------
        var res = option.IsNewLineCharAt(index, c);

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(expected, res);
    }
}