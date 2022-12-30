using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvReaderTest_ReadFields : CsvReaderTest
{
    public CsvReaderTest_ReadFields(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [MemberData(nameof(NormalTestData_NoData))]
    [MemberData(nameof(NormalTestData_SingleLine))]
    [MemberData(nameof(NormalTestData_MultiLine))]
    [MemberData(nameof(NormalTestData_HasHeader))]
    [MemberData(nameof(NormalTestData_SkipLeadingLines))]
    public void NormalTest(string title, string data, CsvOption? option, List<string[]> expectedFieldsList)
    {
        WriteLine();
        WriteLine(title);
        WriteLine($"data: {data}");

        // --------------------------
        // Preparing
        // --------------------------
        using var reader = CreateCsvReader(data, option);

        // --------------------------
        // Invoke
        // --------------------------
        var resFieldsList = new List<string[]>(expectedFieldsList.Count);
        {
            var res = new string[0];

            while ((res = reader.ReadFields()) != null)
            {
                resFieldsList.Add(res);
            }
        }

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(expectedFieldsList.Count, resFieldsList.Count, "Line count");

        for (var i = 0; i < expectedFieldsList.Count; i++)
        {
            var res = resFieldsList[i];
            var exp = expectedFieldsList[i];

            AssertEqual(exp, res, $"{i + 1}th line");
        }
    }

    [Fact]
    public void ExceptionTest()
    {
        WriteLine();
        WriteLine("Throws an exception when found an unknown csv token.");

        // --------------------------
        // Preparing
        // --------------------------
        using var reader = CreateCsvReader("\"\" ", new CsvOption { EscapeChar = '"' });

        // --------------------------
        // Invoke
        // --------------------------
        var action = () => { reader.ReadFields(); reader.ReadFields(); };

        // --------------------------
        // Inspecting
        // --------------------------
        AssertThrows<HigsBasisException>(action);

    }

    public static IEnumerable<object[]> NormalTestData_NoData()
    {
        yield return new object[] {
            "Empty data returns no data.",
            // test data
            "",
            new CsvOption(),
            // expected data
            new List<string[]> {},
        };
    }

    public static IEnumerable<object[]> NormalTestData_SingleLine()
    {
        yield return new object[] {
            "Field data only.",
            // test data
            "A",
            new CsvOption { Delimiter = ',' },
            // expected data
            new List<string[]> {
                new string[] {"A" },
            },
        };

        yield return new object[] {
            "There are Field data and delimiters alternately.",
            // test data
            "A,BC,DEF",
            new CsvOption { Delimiter = ',' },
            // expected data
            new List<string[]> {
                new string[] {"A", "BC", "DEF" },
            },
        };

        yield return new object[] {
            "When a delimiter is at the end of the data, the last element of the field data will be an empty string.",
            // test data
            "A,",
            new CsvOption { Delimiter = ',' },
            // expected data
            new List<string[]> {
                new string[] {"A", "" },
            },
        };

        yield return new object[] {
            "Ends of a NewLine.",
            // test data
            "A,BC\r\n",
            new CsvOption { Delimiter = ',', NewLine = "\r\n" },
            // expected data
            new List<string[]> {
                new string[] {"A", "BC" },
            },
        };

    }


    public static IEnumerable<object[]> NormalTestData_MultiLine()
    {
        yield return new object[] {
            "Multi lines.",
            // test data
            "A1,B1\r\nA2,B2",
            new CsvOption { Delimiter = ',',  },
            // expected data
            new List<string[]> {
                new string[] {"A1", "B1" },
                new string[] {"A2", "B2" },
            },
        };
    }

    public static IEnumerable<object[]> NormalTestData_HasHeader()
    {
        yield return new object[] {
            "Returns row from 2nd line when HasHeader property is true.",
            // test data
            "A1,B1\r\nA2,B2",
            new CsvOption { Delimiter = ',', HasHeader = true },
            // expected data
            new List<string[]> {
                new string[] {"A2", "B2" },
            },
        };
    }

    public static IEnumerable<object[]> NormalTestData_SkipLeadingLines()
    {
        yield return new object[] {
            "Skip leading lines (no header).",
            // test data
            "#1\n#2\n#3\nA1,B1",
            new CsvOption { Delimiter = ',', NewLine="\n", SkipLeadingLine = new SkipLineFunc(3), HasHeader = false },
            // expected data
            new List<string[]> {
                new string[] {"A1", "B1" },
            },
        };

        yield return new object[] {
            "Skip leading lines (has header).",
            // test data
            "#1\n#2\n#3\nA1,B1\nA2,B2",
            new CsvOption { Delimiter = ',', NewLine="\n", SkipLeadingLine = new SkipLineFunc(3), HasHeader = true },
            // expected data
            new List<string[]> {
                new string[] {"A2", "B2" },
            },
        };
    }
}