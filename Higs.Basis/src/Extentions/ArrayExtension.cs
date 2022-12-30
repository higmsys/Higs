namespace Higs.Basis.Extentions;

public static class ArrayExtension
{
    public static T[] Duplicate<T>(this T[] sourceArray)
    {
        return sourceArray.ToArray();
    }

    public static T[] Duplicate<T>(this T[] sourceArray, int offset, int length)
    {
        return sourceArray
            .Skip(offset)
            .Take(length)
            .ToArray();
    }

}