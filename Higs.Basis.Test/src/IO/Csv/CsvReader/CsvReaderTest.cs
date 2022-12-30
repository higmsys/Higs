using System.Text;
using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public abstract class CsvReaderTest : Test
{
    protected CsvReaderTest(ITestOutputHelper outputHelper) : base(outputHelper) { }

    protected StreamReader CreateStreamReader(string data)
    {
        var encoding = Encoding.UTF8;
        var stream = new MemoryStream(encoding.GetBytes(data));
        return new StreamReader(stream, encoding);
    }

    protected CsvReader CreateCsvReader(string data, CsvOption? option)
    {
        var streamReader = CreateStreamReader(data);
        return new CsvReader(streamReader, option);
    }
}