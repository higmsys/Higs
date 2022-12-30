using System.Text;
using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public abstract class CsvTokenizerTest : Test
{
    protected CsvTokenizerTest(ITestOutputHelper outputHelper) : base(outputHelper) { }

    protected StreamReader CreateStreamReader(string data)
    {
        var encoding = System.Text.Encoding.UTF8;
        var stream = new MemoryStream(encoding.GetBytes(data));

        return new StreamReader(stream, encoding);
    }

    protected CsvTokenizer CreateCsvTokenizer(string data, CsvOption option)
    {
        var reader = CreateStreamReader(data);

        return new CsvTokenizer(reader, option);
    }

}