using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvOptionTest_FindFirstIndex : CsvHeadersTest
{
    public CsvOptionTest_FindFirstIndex(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [MemberData(nameof(TestData_WithoutComparer))]
    [MemberData(nameof(TestData_WithComparer))]
    public void Test(string title, string[] names, string searchName, IEqualityComparer<string>? comparer, int expected)
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
        var res = headers.FindFirstIndex(searchName, comparer);

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(expected, res);
    }

    public static IEnumerable<object?[]> TestData_WithoutComparer()
    {
        IEqualityComparer<string>? comparer = null;

        yield return new object?[] {
            "Return -1 when headers is empty.",
            // test data
            new string[] { },
            // parameters
            "",
            comparer,
            // expected
            -1,
        };

        yield return new object?[] {
            "Returns the index when the specified name contains one.",
            // test data
            new string[] { "A", "AA", "AAA" },
            // parameters
            "A",
            comparer,
            // expected
            0,
        };

        yield return new object?[] {
            "Case sensitive. (not found)",
            // test data
            new string[] { "a", "AA", "AAA" },
            // parameters
            "A",
            comparer,
            // expected
            -1,
        };

        yield return new object?[] {
            "Returns the first index when the specified name contains two or more.",
            // test data
            new string[] { "B", "A", "B", "A" },
            // parameters
            "A",
            comparer,
            // expected
            1,
        };
    }

    public static IEnumerable<object?[]> TestData_WithComparer()
    {
        IEqualityComparer<string>? comparer = new CsvHeadersTest.CaseSensitiveEqualityComparer();

        yield return new object?[] {
            "Returns the first index using specified comparer.",
            // test data
            new string[] { "B", "a", "B", "B" },
            // parameters
            "A",
            comparer,
            // expected
            1,
        };
    }
}