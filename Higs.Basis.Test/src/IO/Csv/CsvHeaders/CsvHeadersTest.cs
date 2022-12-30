
namespace Higs.Basis.Test.IO.Csv;

public abstract class CsvHeadersTest : Test
{
    protected CsvHeadersTest(ITestOutputHelper outputHelper) : base(outputHelper) { }

    // Case sensitive comparer
    public class CaseSensitiveEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string? s1, string? s2)
        {
            if (s1 == s2) { return true; }
            if (s1 == null) { return false; }
            if (s2 == null) { return false; }

            return s1.ToUpperInvariant() == s2.ToUpperInvariant();
        }

        public int GetHashCode(string s)
        {
            return s.ToUpperInvariant().GetHashCode();
        }
    }

}