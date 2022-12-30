using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvTokenTest_GetHashCode : CsvTokenTest
{
    public CsvTokenTest_GetHashCode(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Fact]
    public void ReturnsTheSameValueObjectsWithTheSameContents()
    {
        WriteLine();
        WriteLine("Objects with the same contents return the same value.");

        // --------------------------
        // Preparing
        // --------------------------
        var token1 = new CsvToken(CsvTokenType.Delimiter, ",");
        var token2 = new CsvToken(CsvTokenType.Delimiter, ",");

        // --------------------------
        // Invoke
        // --------------------------
        var hash1 = token1.GetHashCode();
        var hash2 = token1.GetHashCode();

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(hash1, hash2);
    }
}