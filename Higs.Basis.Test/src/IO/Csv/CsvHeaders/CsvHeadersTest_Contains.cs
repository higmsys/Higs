using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class CsvOptionTest_Contains : CsvHeadersTest
{
    public CsvOptionTest_Contains(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [MemberData(nameof(TestData_WithoutComparer))]
    [MemberData(nameof(TestData_WithComparer))]
    public void Test(string title, string[] names, string name, IEqualityComparer<string>? comparer, bool expected)
    {
        WriteLine();
        WriteLine(title);
        WriteLine($"names: [{string.Join(",", names.Select(n => $"'{n}'"))}]");
        WriteLine($"name : {name}");
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var headers = new CsvHeaders(names);

        // --------------------------
        // Invoke
        // --------------------------
        var res = headers.Contains(name, comparer);

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(expected, res);
    }

    public static IEnumerable<object?[]> TestData_WithoutComparer()
    {
        IEqualityComparer<string>? comparer = null;

        yield return new object?[] {
            "Return false when headers is empty.",
            // test data
            new string[] { },
            // parameters
            "",
            comparer,
            // expected
            false,
        };

        yield return new object?[] {
            "Returns true when the specified name contains one.",
            // test data
            new string[] { "A", "AA", "AAA" },
            // parameters
            "A",
            comparer,
            // expected
            true,
        };

        yield return new object?[] {
            "Case sensitive.",
            // test data
            new string[] { "a", "AA", "AAA" },
            // parameters
            "A",
            comparer,
            // expected
            false,
        };

        yield return new object?[] {
            "Returns true when the specified name contains two or more.",
            // test data
            new string[] { "A", "B", "A", "A" },
            // parameters
            "A",
            comparer,
            // expected
            true,
        };
    }

    public static IEnumerable<object?[]> TestData_WithComparer()
    {
        IEqualityComparer<string>? comparer = new CsvHeadersTest.CaseSensitiveEqualityComparer();

        yield return new object?[] {
            "Returns true using specified comparer.",
            // test data
            new string[] { "B", "a" },
            // parameters
            "A",
            comparer,
            // expected
            true,
        };
    }
}