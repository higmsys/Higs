using System.Text;
using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvReaderTest_TryItOut : CsvReaderTest
{
    public CsvReaderTest_TryItOut(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Fact]
    public void Test()
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var encoding = Encoding.UTF8;
        var csvPath = GetTestDataPath(
            "IO/Csv/CsvReader/TryItOut",
            "states_withHeaders_withSomeDoubleQuotes_CRLF_UTF8.csv");
        var csvOption = new CsvOption
        {
            Delimiter = ',',
            NewLine = "\r\n",
            EscapeChar = '\"',
            HasHeader = true,
        };

        using var reader = new CsvReader(new StreamReader(csvPath, encoding), csvOption);

        // --------------------------
        // Invoke & Inspecting
        // --------------------------

        // Headers
        AssertEqual(new string[] {
            "Stats",
            "Stats(ja)",
            "Codes",
            "Capital",
            "Area(km2)",
            "wikipedia",
        }, reader.Headers.ToArray(), "Headers");

        // 1st line
        {
            var csvLine = reader.ReadLine();

            AssertEqual(
                new string[] {
                    "Alabama",
                    "アラバマ",
                    "AL",
                    "Montgomery",
                    "135,765",
                    "https://en.wikipedia.org/wiki/Alabama"
                },
                csvLine!.ToArray(), "1st line");
        }

        // lines from 2nd to 9th.
        reader.ReadLine();
        reader.ReadLine();
        reader.ReadLine();
        reader.ReadLine();
        reader.ReadLine();
        reader.ReadLine();
        reader.ReadLine();
        reader.ReadLine();

        // last line.
        {
            var csvLine = reader.ReadLine();

            AssertEqual(
                new string[] {
                    "Georgia",
                    "ジョージア",
                    "GA",
                    "Atlanta",
                    "153,909",
                    "https://en.wikipedia.org/wiki/Georgia_(U.S._state)"
                },
                csvLine!.ToArray(), "last line");
        }

    }
}