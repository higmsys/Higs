namespace Higs.Basis.IO.Csv;

public interface ICsvLine
{
    /// <summary>
    /// A raw line string.
    /// </summary>
    /// <value></value>
    public string RawValue { get; }

    /// <summary>
    /// A line index inside a file started from 0.
    /// </summary>
    /// <value></value>
    public int LineIndex { get; }

    /// <summary>
    /// The count of fields of this line.
    /// </summary>
    /// <value></value>
    public int FieldCount { get; }

    /// <summary>
    /// The field value in the specified index.
    /// </summary>
    /// <value></value>
    public string this[int fieldIndex] { get; }

    /// <summary>
    /// The field value in the specified field name.
    /// </summary>
    /// <value></value>
    public string this[string fieldName] { get; }

    public string[] ToArray();
}
