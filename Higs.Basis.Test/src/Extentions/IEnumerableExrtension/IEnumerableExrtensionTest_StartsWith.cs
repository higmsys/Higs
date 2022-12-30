using Higs.Basis.Extentions;

namespace Higs.Basis.Test.Extentions;

public class IEnumerableExrtensionTest_StartsWith : IEnumerableExrtensionTest
{
    public IEnumerableExrtensionTest_StartsWith(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [InlineData("Empty array returns true.", new string[] { }, new string[] { }, true)]
    [InlineData("Returns true when started with the specified array.", new string[] { "A", "B", "C" }, new string[] { "A", "B" }, true)]
    [InlineData("Returns true when the two arrays have the same values.", new string[] { "A", "B", "C" }, new string[] { "A", "B", "C" }, true)]
    [InlineData("Returns false when not started with the specified array.", new string[] { "A", "B", "C" }, new string[] { "B", "C" }, false)]
    [InlineData("Returns false when the specified array is longer than the base array.", new string[] { "A", "B" }, new string[] { "A", "B", "C" }, false)]
    public void Test(string title, string[] baseArray, string[] searchArray, bool expected)
    {
        WriteLine();
        WriteLine(title);
        WriteLine($" baseArray  : [{string.Join(",", baseArray.Select(s => $"'{s}'"))}]");
        WriteLine($" searchArray: [{string.Join(",", searchArray.Select(s => $"'{s}'"))}]");

        // --------------------------
        // Preparing
        // --------------------------

        // --------------------------
        // Invoke
        // --------------------------
        var res = baseArray.StartsWith(searchArray);

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(expected, res);
    }
}