namespace Higs.Basis.Extentions;

public static class IEnumerableExtension
{
    public static bool StartsWith<T>(this IEnumerable<T> own, IEnumerable<T> other)
    {
        return Enumerable.SequenceEqual(
            own.Take(other.Count()),
            other);
    }

}