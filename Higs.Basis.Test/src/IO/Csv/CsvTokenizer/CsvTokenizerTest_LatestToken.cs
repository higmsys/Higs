using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvTokenizerTest_LatestToken : CsvTokenizerTest
{
    public CsvTokenizerTest_LatestToken(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [MemberData(nameof(TestData_NullWhenBeforeReading))]
    [MemberData(nameof(TestData_CsvTokenWhenAfterReading))]
    public void Test(
        string title,
        string data,
        CsvOption option,
        int raedCount,
        CsvToken? expected)
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
        for (var i = 0; i < raedCount; i++)
        {
            tokenizer.Next();
        }
        var actual = tokenizer.LatestToken;

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(expected, actual);
    }

    public static IEnumerable<object?[]> TestData_NullWhenBeforeReading()
    {
        yield return new object?[] {
            "It's null when before reading.",
            // csv data
            "abc,def",
            new CsvOption { Delimiter = ',' },
            // read count
            0,
            // expected
            null,
        };
    }

    public static IEnumerable<object?[]> TestData_CsvTokenWhenAfterReading()
    {
        yield return new object?[] {
            "It's the latest csv token after reading field data.",
            // csv data
            "abc,def",
            new CsvOption { Delimiter = ',' },
            // read count
            1,
            // expected
            new CsvToken(CsvTokenType.FieldData, "abc"),
        };

        yield return new object?[] {
            "It's the latest csv token after reading delimiter.",
            // csv data
            "abc,def",
            new CsvOption { Delimiter = ',' },
            // read count
            2,
            // expected
            new CsvToken(CsvTokenType.Delimiter, ","),
        };

        yield return new object?[] {
            "It's the latest csv token after reading all.",
            // csv data
            "abc,def",
            new CsvOption { Delimiter = ',' },
            // read count
            4,
            // expected
            new CsvToken(CsvTokenType.EndOfFile, ""),
        };

    }
}