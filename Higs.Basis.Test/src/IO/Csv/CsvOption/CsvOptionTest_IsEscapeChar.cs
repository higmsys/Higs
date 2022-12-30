using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvOptionTest_IsEscapeChar : CsvOptionTest
{
    public CsvOptionTest_IsEscapeChar(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [InlineData('\\', '\\', true)]
    [InlineData('\\', ',', false)]
    public void Test(char escapeChar, char c, bool expected)
    {
        WriteLine();
        WriteLine($"escape   : '{escapeChar}'");
        WriteLine($"target   : '{c}'");
        WriteLine($"expected : '{expected}'");
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var option = new CsvOption
        {
            EscapeChar = escapeChar,
        };

        // --------------------------
        // Invoke
        // --------------------------
        var res = option.IsEscapeChar(c);

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(expected, res);
    }
}