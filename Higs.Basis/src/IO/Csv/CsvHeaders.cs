using System.Collections;

namespace Higs.Basis.IO.Csv;

public sealed class CsvHeaders : IReadOnlyList<string>
{
    private readonly IReadOnlyList<string> _names;

    public CsvHeaders() : this(new string[0]) { }

    public CsvHeaders(string[] names)
    {
        _names = Array.AsReadOnly(names);
    }

    public string this[int index] => _names[index];

    public int Count => _names.Count;

    public int FindFirstIndex(string searchName, IEqualityComparer<string>? comparer = null)
    {
        var indexes = FindIndexes(searchName, comparer);

        if (0 < indexes.Length)
        {
            return indexes[0];
        }
        else
        {
            return -1;
        }
    }

    public int[] FindIndexes(string searchName, IEqualityComparer<string>? comparer = null)
    {
        var nameIndexes = _names.Select((name, index) => new { Name = name, Index = index });

        var filterdNames = (comparer != null)
            ? nameIndexes.Where(ni => comparer.Equals(ni.Name, searchName))
            : nameIndexes.Where(ni => ni.Name == searchName);

        return filterdNames
            .Select(ni => ni.Index)
            .ToArray();
    }

    public bool Contains(string name, IEqualityComparer<string>? comparer = null)
    {
        return _names.Contains(name, comparer);
    }

    public IEnumerator<string> GetEnumerator()
    {
        return _names.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _names.GetEnumerator();
    }

}
