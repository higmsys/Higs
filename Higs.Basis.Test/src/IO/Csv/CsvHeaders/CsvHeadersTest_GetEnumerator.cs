using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvHeadersTest_GetEnumerator : CsvHeadersTest
{
    public CsvHeadersTest_GetEnumerator(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Fact]
    public void TestGetEnumeratorOfIEnumerator()
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var names = new string[] { "H1", "H2", "H3" };
        var headers = new CsvHeaders(names);

        // --------------------------
        // Invoke
        // --------------------------
        var enumerator = headers.GetEnumerator();

        // --------------------------
        // Inspecting
        // --------------------------
        var index = 0;
        while (enumerator.MoveNext())
        {
            var act = enumerator.Current;

            AssertEqual(names[index], act, $"{index + 1}th");
            index++;
        }

        AssertEqual(names.Length, index, $"Count");
    }


    [Fact]
    public void TestGetEnumeratorOfIEnumerable()
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var names = new string[] { "H1", "H2", "H3" };
        var headers = new CsvHeaders(names);

        // --------------------------
        // Invoke
        // --------------------------
        var enumerator = ((System.Collections.IEnumerable)headers).GetEnumerator();

        // --------------------------
        // Inspecting
        // --------------------------
        var index = 0;
        while (enumerator.MoveNext())
        {
            var act = enumerator.Current;

            AssertEqual(names[index], act, $"{index + 1}th");
            index++;
        }

        AssertEqual(names.Length, index, $"Count");
    }
}