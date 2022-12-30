using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvTokenizerTest_HasNext : CsvTokenizerTest
{
    public CsvTokenizerTest_HasNext(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [InlineData("Letter", "A")]
    [InlineData("New line", "\r")]
    [InlineData("New line", "\n")]
    [InlineData("New line", "\r\n")]
    public void ReturnsTrueWhenDataIsRemained(string title, string data)
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var tokenizer = CreateCsvTokenizer(data, new CsvOption());

        // --------------------------
        // Invoke
        // --------------------------
        var res = tokenizer.HasNext();

        // --------------------------
        // Inspecting
        // --------------------------
        AssertTrue(res, $"{title} ({data})");
    }

    [Theory]
    [InlineData("No data", "", 0)]
    [InlineData("No date after reading", "A", 1)]
    public void ReturnsFalseWhenDataIsNothing(string title, string data, int readCount)
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var tokenizer = CreateCsvTokenizer(data, new CsvOption());

        for (var i = 0; i < readCount; i++)
        {
            tokenizer.Next();
        }

        // --------------------------
        // Invoke
        // --------------------------
        var res = tokenizer.HasNext();

        // --------------------------
        // Inspecting
        // --------------------------
        AssertFalse(res, $"{title} ({data})");
    }
}