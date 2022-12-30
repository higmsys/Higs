using Higs.Basis.Extentions;

namespace Higs.Basis.Test.Extentions;

public class ArrayExtensionTest_Duplicate : ArrayExtensionTest
{
    public ArrayExtensionTest_Duplicate(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Fact]
    public void Test()
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var source = new string[] { "A", "B", "C" };

        // --------------------------
        // Invoke
        // --------------------------
        var copy = source.Duplicate();

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(source, copy, $"The two arrays have the same values.");
        AssertFalse(object.ReferenceEquals(source, copy), $"The two arrays are different instances.");
    }

    [Fact]
    public void TestWithIntInt()
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var source = new string[] { "A", "B", "C", "D", "E" };

        // --------------------------
        // Invoke
        // --------------------------
        var copy = source.Duplicate(1, 3);

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(new string[] { "B", "C", "D" }, copy, $"The duplicated array have only the specified range of original array.");
    }
}