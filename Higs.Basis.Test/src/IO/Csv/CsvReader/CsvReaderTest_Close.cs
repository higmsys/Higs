using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvReaderTest_Close : CsvReaderTest
{
    public CsvReaderTest_Close(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Fact]
    public void Test()
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        using var stream = CreateStreamReader("ABC");
        using var reader = new CsvReader(stream);

        // --------------------------
        // Invoke
        // --------------------------
        reader.Close();

        // --------------------------
        // Inspecting
        // --------------------------
        var eos = false;
        AssertThrows<ObjectDisposedException>(() => eos = stream.EndOfStream, "The stream is closed when the reader is closed.");
    }
}