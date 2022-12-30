using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvTokenizerTest_DisposeNextLine : CsvTokenizerTest
{
    public CsvTokenizerTest_DisposeNextLine(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [MemberData(nameof(TestData))]
    public void Test(
        string title,
        string data,
        CsvOption option,
        string[] expectedLines)
    {
        WriteLine();
        WriteLine(title);
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var tokenizer = CreateCsvTokenizer(data, option);

        // --------------------------
        // Invoke
        // --------------------------
        var disposedLines = new string[expectedLines.Length];
        for (var i = 0; i < expectedLines.Length; i++)
        {
            disposedLines[i] = tokenizer.DisposeNextLine();
        }

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(expectedLines, disposedLines);
    }

    public static IEnumerable<object[]> TestData()
    {
        yield return new object[] {
            "Returns an empty string when the data is empty.",
            // test data
            "",
            new CsvOption { Delimiter = ',', NewLine = "\n" },
            // expected lines
            new string[] { "" },
        };

        yield return new object[] {
            "Returns lines without NewLines(=\n).",
            // test data
            "A,B,C\nD",
            new CsvOption { Delimiter = ',', NewLine = "\n" },
            // expected lines
            new string[] { "A,B,C", "D" },
        };

        yield return new object[] {
            "Returns lines without NewLines(=\r\n).",
            // test data
            "A,B,C\r\nD",
            new CsvOption { Delimiter = ',', NewLine = "\r\n" },
            // expected lines
            new string[] { "A,B,C", "D" },
        };

    }
}