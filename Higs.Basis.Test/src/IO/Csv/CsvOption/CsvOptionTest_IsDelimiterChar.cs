using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvOptionTest_IsDelimiterChar : CsvOptionTest
{
    public CsvOptionTest_IsDelimiterChar(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [InlineData(',', ',', true)]
    [InlineData(',', '|', false)]
    public void Test(char delimiter, char c, bool expected)
    {
        WriteLine();
        WriteLine($"delimiter: '{delimiter}'");
        WriteLine($"target   : '{c}'");
        WriteLine($"expected : '{expected}'");
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var option = new CsvOption
        {
            Delimiter = delimiter,
        };

        // --------------------------
        // Invoke
        // --------------------------
        var res = option.IsDelimiterChar(c);

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(expected, res);
    }
}