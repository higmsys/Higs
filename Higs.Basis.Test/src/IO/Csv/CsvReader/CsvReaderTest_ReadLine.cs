using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvReaderTest_ReadLine : CsvReaderTest
{
    public CsvReaderTest_ReadLine(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [MemberData(nameof(NormalTestData_NoData))]
    [MemberData(nameof(NormalTestData_SingleLine))]
    [MemberData(nameof(NormalTestData_MultiLine))]
    [MemberData(nameof(NormalTestData_HasHeader))]
    [MemberData(nameof(NormalTestData_SkipLeadingLines))]
    public void NormalTest(
        string title,
        string data,
        CsvOption? option,
        List<(string RawValue, string[] fields)> expectedList)
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
        var resLineList = new List<ICsvLine>(expectedList.Count);
        {
            ICsvLine? line;

            while ((line = reader.ReadLine()) != null)
            {
                resLineList.Add(line);
            }
        }

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(expectedList.Count, resLineList.Count, "Line count");

        for (var i = 0; i < expectedList.Count; i++)
        {
            WriteLine();
            var res = resLineList[i];
            var expRaw = expectedList[i].RawValue;
            var expFields = expectedList[i].fields;

            AssertEqual(i, res.LineIndex, $"{i + 1}th line index");
            AssertEqual(expRaw, res.RawValue, $"{i + 1}th line value");
            AssertEqual(expFields, res.ToArray(), $"{i + 1}th line field values");
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
        var action = () => { reader.ReadLine(); reader.ReadLine(); };

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
            new List<(string RawValue, string[] fields)> { },
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
            new List<(string RawValue, string[] fields)> {
                ("A", new string[] { "A" }),
            },
        };

        yield return new object[] {
            "There are Field data and delimiters alternately.",
            // test data
            "A,BC,DEF",
            new CsvOption { Delimiter = ',' },
            // expected data
            new List<(string RawValue, string[] fields)> {
                ("A,BC,DEF", new string[] {"A", "BC", "DEF" }),
            },
        };

        yield return new object[] {
            "When a delimiter is at the end of the data, the last element of the field data will be an empty string.",
            // test data
            "A,",
            new CsvOption { Delimiter = ',' },
            // expected data
            new List<(string RawValue, string[] fields)> {
                ("A,", new string[] {"A", "" }),
            },
        };

        yield return new object[] {
            "Ends of a NewLine.",
            // test data
            "A,BC\r\n",
            new CsvOption { Delimiter = ',', NewLine = "\r\n" },
            // expected data
            new List<(string RawValue, string[] fields)> {
                ("A,BC", new string[] {"A", "BC" }),
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
            new List<(string RawValue, string[] fields)> {
                ("A1,B1", new string[] {"A1", "B1" }),
                ("A2,B2", new string[] {"A2", "B2" }),
            },
        };
    }

    public static IEnumerable<object[]> NormalTestData_HasHeader()
    {
        yield return new object[] {
            "Returns line from 2nd line when HasHeader property is true.",
            // test data
            "A1,B1\r\nA2,B2",
            new CsvOption { Delimiter = ',', HasHeader = true },
            // expected data
            new List<(string RawValue, string[] fields)> {
                ("A2,B2", new string[] {"A2", "B2" }),
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
            new List<(string RawValue, string[] fields)> {
                ("A1,B1", new string[] {"A1", "B1" }),
            },
        };

        yield return new object[] {
            "Skip leading lines (has header).",
            // test data
            "#1\n#2\n#3\nA1,B1\nA2,B2",
            new CsvOption { Delimiter = ',', NewLine="\n", SkipLeadingLine = new SkipLineFunc(3), HasHeader = true },
            // expected data
            new List<(string RawValue, string[] fields)> {
                ("A2,B2", new string[] {"A2", "B2" }),
            },
        };
    }
}