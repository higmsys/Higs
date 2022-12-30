using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvTokenizerTest_PeekNextChar : CsvTokenizerTest
{
    public CsvTokenizerTest_PeekNextChar(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [MemberData(nameof(TestData))]
    public void Test(
        string title,
        string data,
        CsvOption option,
        int readCount,
        char? expected)
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
        for (var i = 0; i < readCount; i++)
        {
            tokenizer.Next();
        }
        var actual = tokenizer.PeekNextChar();

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(expected, actual);
    }

    public static IEnumerable<object?[]> TestData()
    {
        yield return new object?[] {
            "Returns null when data is empty.",
            // data
            "",
            new CsvOption { Delimiter = ',' },
            // read count
            0,
            // expected
            null,
        };

        yield return new object?[] {
            "Returns a next char before reading.",
            // data
            "abc,def",
            new CsvOption { Delimiter = ',' },
            // read count
            0,
            // expected
            'a',
        };

        yield return new object?[] {
            "Returns a next char after reading but not all.",
            // data
            "abc,def",
            new CsvOption { Delimiter = ',' },
            // read count
            1,
            // expected
            ',',
        };

        yield return new object?[] {
            "Returns null after reading all.",
            // data
            "abc,def",
            new CsvOption { Delimiter = ',' },
            // read count
            3,
            // expected
            null,
        };
    }
}