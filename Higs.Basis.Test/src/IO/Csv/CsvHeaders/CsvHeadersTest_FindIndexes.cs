using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvOptionTest_FindIndexes : CsvHeadersTest
{
    public CsvOptionTest_FindIndexes(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [MemberData(nameof(TestData_WithoutComparer))]
    [MemberData(nameof(TestData_WithComparer))]
    public void Test(string title, string[] names, string searchName, IEqualityComparer<string>? comparer, int[] expectedIndexes)
    {
        WriteLine();
        WriteLine(title);
        WriteLine($"names     : [{string.Join(",", names.Select(n => $"'{n}'"))}]");
        WriteLine($"searchName: {searchName}");
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var headers = new CsvHeaders(names);

        // --------------------------
        // Invoke
        // --------------------------
        var resIndexes = headers.FindIndexes(searchName, comparer);

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(expectedIndexes, resIndexes);
    }

    public static IEnumerable<object?[]> TestData_WithoutComparer()
    {
        IEqualityComparer<string>? comparer = null;

        yield return new object?[] {
            "Return an empty array when headers is empty.",
            // test data
            new string[] { },
            // parameters
            "",
            comparer,
            // expected
            new int[] { },
        };

        yield return new object?[] {
            "Returns the index when the specified name contains one.",
            // test data
            new string[] { "A", "AA", "AAA" },
            // parameters
            "A",
            comparer,
            // expected
            new int[] { 0 },
        };

        yield return new object?[] {
            "Case sensitive.",
            // test data
            new string[] { "a", "AA", "AAA" },
            // parameters
            "A",
            comparer,
            // expected
            new int[] { },
        };

        yield return new object?[] {
            "Returns the indexes when the specified name contains two or more.",
            // test data
            new string[] { "A", "B", "A", "A" },
            // parameters
            "A",
            comparer,
            // expected
            new int[] { 0, 2, 3 },
        };
    }

    public static IEnumerable<object?[]> TestData_WithComparer()
    {
        IEqualityComparer<string>? comparer = new CsvHeadersTest.CaseSensitiveEqualityComparer();

        yield return new object?[] {
            "Returns the indexes using specified comparer.",
            // test data
            new string[] { "A", "B", "a", "A" },
            // parameters
            "A",
            comparer,
            // expected
            new int[] { 0, 2, 3 },
        };
    }
}