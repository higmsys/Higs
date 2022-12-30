using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvReaderTest_Constructor : CsvReaderTest
{
    public CsvReaderTest_Constructor(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Fact]
    public void DefaultCsvOption()
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var fields1 = new string[] { "A", "B\r", "C" };
        var fields2 = new string[] { "D", "E\n", "F" };
        var fields3 = new string[] { "G", "\"H\"\",\"", "I" };
        var data = string.Join("\r\n", new string[] {
            string.Join(",", fields1),
            string.Join(",", fields2),
            string.Join(",", fields3),
        });
        WriteLine($"data: {data}");
        using var reader = new CsvReader(CreateStreamReader(data));

        // --------------------------
        // Invoke
        // --------------------------
        var csvLine1 = reader.ReadLine();
        var csvLine2 = reader.ReadLine();
        var csvLine3 = reader.ReadLine();

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(fields1, csvLine1!.ToArray(), "delimit with comma and newline is crlf not cr");
        AssertEqual(fields2, csvLine2!.ToArray(), "newline is crlf, not lf");
        AssertEqual(new string[] { "G", "H\",", "I" }, csvLine3!.ToArray(), "escaped with double quote");
    }
}