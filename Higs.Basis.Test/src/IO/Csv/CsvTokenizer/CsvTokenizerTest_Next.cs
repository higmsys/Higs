using System.Text;
using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvTokenizerTest_Next : CsvTokenizerTest
{
    public CsvTokenizerTest_Next(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [MemberData(nameof(ScenarioData_ReturnsEndOfFileWhenReachedToEndOfStream))]
    [MemberData(nameof(ScenarioData_ReturnsFieldDataAndDelimiterAlternately))]
    [MemberData(nameof(ScenarioData_ReturnsEndOfLineWhenFoundNewLine))]
    [MemberData(nameof(ScenarioData_SurroundedWithDoubleQuotations))]
    [MemberData(nameof(ScenarioData_ReturnsUnknownWhenExistsDataAfterData))]
    public void Scenario(string title, string data, CsvOption option, CsvToken[] expectedTokens)
    {
        WriteLine();
        WriteLine(title);
        WriteLine($"Input data: '{data}'");
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var tokenizer = CreateCsvTokenizer(data, option);

        // --------------------------
        // Invoke & Inspecting
        // --------------------------
        for (var i = 0; i < expectedTokens.Length; i++)
        {
            var exp = expectedTokens[i];
            var act = tokenizer.Next();

            AssertEqual(exp, act, $"{i + 1}");
        }
    }

    public static IEnumerable<object[]> ScenarioData_ReturnsEndOfFileWhenReachedToEndOfStream()
    {
        yield return new object[] {
            "No data",
            // data
            "",
            new CsvOption(),
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };

        yield return new object[] {
            "No data after reading",
            // data
            "A",
            new CsvOption(),
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, "A"),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };
    }

    public static IEnumerable<object[]> ScenarioData_ReturnsFieldDataAndDelimiterAlternately()
    {
        var option = new CsvOption
        {
            Delimiter = ',',
        };

        yield return new object[] {
            "Returns empty field data when starts with delimiter",
            // data
            $",",
            option,
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, ""),
                new CsvToken(CsvTokenType.Delimiter, ","),
                new CsvToken(CsvTokenType.FieldData, ""),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };

        yield return new object[] {
            "Returns empty field data when delimiters are contiguous.",
            // data
            $",,",
            option,
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, ""),
                new CsvToken(CsvTokenType.Delimiter, ","),
                new CsvToken(CsvTokenType.FieldData, ""),
                new CsvToken(CsvTokenType.Delimiter, ","),
                new CsvToken(CsvTokenType.FieldData, ""),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };

        yield return new object[] {
            "Returns field data with a letter.",
            // data
            $"A,",
            option,
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, "A"),
                new CsvToken(CsvTokenType.Delimiter, ","),
                new CsvToken(CsvTokenType.FieldData, ""),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };

        yield return new object[] {
            "Returns field data with letters.",
            // data
            $"AB,",
            option,
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, "AB"),
                new CsvToken(CsvTokenType.Delimiter, ","),
                new CsvToken(CsvTokenType.FieldData, ""),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };
    }

    public static IEnumerable<object[]> ScenarioData_SurroundedWithDoubleQuotations()
    {

        yield return new object[] {
            "Returns empty FieldData when only double quotations",
            // data
            $"\"\"",
            new CsvOption { EscapeChar = '"' },
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, ""),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };

        yield return new object[] {
            "Returns FieldData of a letter when surrounded with double quotations",
            // data
            $"\"A\"",
            new CsvOption { EscapeChar = '"' },
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, "A"),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };

        yield return new object[] {
            "Returns FieldData contains a delimiter when surrounded with double quotations",
            // data
            $"\"A,B\"",
            new CsvOption { EscapeChar = '"' },
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, "A,B"),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };

        yield return new object[] {
            "Returns FieldData contains a NewLine when surrounded with double quotations",
            // data
            $"\"A\r\nB\"",
            new CsvOption { EscapeChar = '"', NewLine = "\r\n" },
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, "A\r\nB"),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };

        yield return new object[] {
            "Returns FieldData contains an escaped double quotation when surrounded with double quotations (The escaped char is '\"')",
            // data
            $"\"A\"\"B\"",
            new CsvOption { EscapeChar = '"' },
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, "A\"B"),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };

        yield return new object[] {
            "Returns FieldData contains an escaped double quotation when surrounded with double quotations (The escaped char is '\\')",
            // data
            $"\"A\\B\"",
            new CsvOption { EscapeChar = '\\' },
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, "A\\B"),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };

    }

    public static IEnumerable<object[]> ScenarioData_ReturnsEndOfLineWhenFoundNewLine()
    {

        yield return new object[] {
            "Returns EndOfLine when found NewLine (CrLf)",
            // data
            $"A\r\n",
            new CsvOption { NewLine = "\r\n" },
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, "A"),
                new CsvToken(CsvTokenType.EndOfLine, "\r\n"),
            },
        };

        yield return new object[] {
            "Returns EndOfLine when found NewLine (Cr)",
            // data
            $"A\r",
            new CsvOption { NewLine = "\r" },
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, "A"),
                new CsvToken(CsvTokenType.EndOfLine, "\r"),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };

        yield return new object[] {
            "Returns EndOfLine when found NewLine (Lf)",
            // data
            $"A\n",
            new CsvOption { NewLine = "\n" },
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, "A"),
                new CsvToken(CsvTokenType.EndOfLine, "\n"),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };

        yield return new object[] {
            "Returns EndOfLine when found NewLine (starts with NewLine)",
            // data
            $"\r\n",
            new CsvOption { NewLine = "\r\n" },
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, ""),
                new CsvToken(CsvTokenType.EndOfLine, "\r\n"),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };

        yield return new object[] {
            "Returns FieldData when found NewLine continuously.",
            // data
            $"\r\n\r\n",
            new CsvOption { NewLine = "\r\n" },
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, ""),
                new CsvToken(CsvTokenType.EndOfLine, "\r\n"),
                new CsvToken(CsvTokenType.FieldData, ""),
                new CsvToken(CsvTokenType.EndOfLine, "\r\n"),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };
    }

    public static IEnumerable<object[]> ScenarioData_ReturnsUnknownWhenExistsDataAfterData()
    {
        yield return new object[] {
            "Returns Unknown.",
            // data
            $"\"\"A",
            new CsvOption { NewLine = "\r\n" },
            // expected
            new CsvToken[] {
                new CsvToken(CsvTokenType.FieldData, ""),
                new CsvToken(CsvTokenType.Unknown, "A"),
                new CsvToken(CsvTokenType.EndOfFile, ""),
            },
        };
    }
}