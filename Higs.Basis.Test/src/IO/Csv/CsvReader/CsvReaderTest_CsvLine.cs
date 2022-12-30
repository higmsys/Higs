using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvReaderTest_CsvLine : CsvReaderTest
{
    public CsvReaderTest_CsvLine(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Fact]
    public void RawValueTest()
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var rawLine1 = "A,\"B,\"\"C\",DEF";
        using var reader = CreateCsvReader(
            rawLine1,
            new CsvOption { Delimiter = ',', EscapeChar = '"' });

        // --------------------------
        // Invoke
        // --------------------------
        var line = reader.ReadLine();

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(rawLine1, line!.RawValue);
    }

    [Fact]
    public void LineIndexTest()
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var rawLine1 = "A\nB";
        using var reader = CreateCsvReader(
            rawLine1,
            new CsvOption { Delimiter = ',', NewLine = "\n" });

        // --------------------------
        // Invoke
        // --------------------------
        var line1 = reader.ReadLine();
        var line2 = reader.ReadLine();

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(0, line1!.LineIndex, "1st line");
        AssertEqual(1, line2!.LineIndex, "2nd line");
    }

    [Fact]
    public void FieldCountTest()
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var rawLine1 = "A,B";
        using var reader = CreateCsvReader(
            rawLine1,
            new CsvOption { Delimiter = ',' });

        // --------------------------
        // Invoke
        // --------------------------
        var line1 = reader.ReadLine();

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(2, line1!.FieldCount);
    }

    [Fact]
    public void ThisByIndexTest()
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var rawLine1 = "A,\"B,\"\"C\",DEF";
        using var reader = CreateCsvReader(
            rawLine1,
            new CsvOption { Delimiter = ',' });

        // --------------------------
        // Invoke
        // --------------------------
        var line1 = reader.ReadLine();

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual("A", line1![0], "1st field");
        AssertEqual("B,\"C", line1![1], "2nd field");
        AssertEqual("DEF", line1![2], "3rd field");
    }

    [Fact]
    public void ThisByNameTest()
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var rawLine1 = "H1,H2,H3\nV1,V2,V3";
        using var reader = CreateCsvReader(
            rawLine1,
            new CsvOption { Delimiter = ',', NewLine = "\n", HasHeader = true });

        // --------------------------
        // Invoke
        // --------------------------
        var line1 = reader.ReadLine();

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual("V1", line1!["H1"], "1st");
        AssertEqual("V2", line1!["H2"], "2st");
        AssertEqual("V3", line1!["H3"], "3rd");
        {
            string a;
            AssertThrows<HigsBasisException>(
                () => a = line1!["H4"],
                "Throws an exception because of specifing not exists header name.");
        }
    }

    [Fact]
    public void ToArrayTest()
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var rawLine1 = "V1,V2,V3";
        using var reader = CreateCsvReader(
            rawLine1,
            new CsvOption { Delimiter = ',' });

        // --------------------------
        // Invoke
        // --------------------------
        var line1 = reader.ReadLine();

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(new string[] { "V1", "V2", "V3" }, line1!.ToArray());
    }

}