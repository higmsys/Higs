using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvReaderTest_Headers : CsvReaderTest
{
    public CsvReaderTest_Headers(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [MemberData(nameof(TestData_EmptyData))]
    [MemberData(nameof(TestData_HasHeaderProperty))]
    [MemberData(nameof(TestData_SkipLeadingLines))]
    public void Test(string title, string data, CsvOption? option, string[] expectedHeaders)
    {
        WriteLine();
        WriteLine(title);
        WriteLine($"HasHeader: {option?.HasHeader}");
        WriteLine($"NewLine  : {option?.NewLine}");
        WriteLine($"data     : {data}");
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        using var reader = CreateCsvReader(data, option);

        // --------------------------
        // Invoke
        // --------------------------
        var headers = reader.Headers;

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(expectedHeaders.Length, headers.Count, "Count");

        for (var i = 0; i < expectedHeaders.Length; i++)
        {
            AssertEqual(expectedHeaders[i], headers[i], $"index:{i}");
        }
    }

    public static IEnumerable<object[]> TestData_EmptyData()
    {
        yield return new object[] {
            "Empty data has no headers.",
            // test data
            "",
            new CsvOption { HasHeader = true },
            // expected
            new string[] { },
        };
    }

    public static IEnumerable<object[]> TestData_HasHeaderProperty()
    {
        yield return new object[] {
            "The headers is returned when the CsvOption.HasHeader is true.",
            // test data
            "H1,H2,H3",
            new CsvOption { HasHeader = true },
            // expected
            new string[] { "H1", "H2", "H3" },
        };

        yield return new object[] {
            "The headers is empty when the CsvOption.HasHeader is false.",
            // test data
            "H1,H2,H3",
            new CsvOption { HasHeader = false },
            // expected
            new string[] { },
        };
    }

    public static IEnumerable<object[]> TestData_SkipLeadingLines()
    {
        yield return new object[] {
            "The header line is after skipping lead lines.",
            // test data
            "#1\n#2\n#3\nH1,H2,H3\n",
            new CsvOption { HasHeader = true, SkipLeadingLine = new SkipLineFunc(3), NewLine = "\n" },
            // expected
            new string[] { "H1", "H2", "H3" },
        };
    }
}