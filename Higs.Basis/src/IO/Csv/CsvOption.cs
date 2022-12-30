namespace Higs.Basis.IO.Csv;

public sealed class CsvOption
{
    /// <summary>
    /// Create a instance with default values.
    /// </summary>
    public CsvOption() { }

    /// <summary>
    /// Create a instance has the same values with the specified instance.
    /// </summary>
    /// <param name="source"></param>
    public CsvOption(CsvOption source)
    {
        SkipLeadingLine = source.SkipLeadingLine;
        HasHeader = source.HasHeader;
        Delimiter = source.Delimiter;
        EscapeChar = source.EscapeChar;
        NewLine = source.NewLine;
    }

    /// <summary>
    /// [Only for reading] Function to determine if the leading lines should be skipped.
    /// </summary>
    /// <remarks>
    /// Default value is the function that does not skip the leading lines.
    /// </remarks>
    public SkipLineFunc SkipLeadingLine { get; set; } = new SkipLineFunc();

    /// <summary>
    /// [Only for reading] Whether to read the first line as a header.
    /// </summary>
    /// <remarks>
    /// The first line here refers to the first line of the lines
    ///  after the leading lines are skipped.
    /// The default value is False.
    /// </remarks>
    /// <value>True when the first line is read as a header</value>
    public bool HasHeader { get; set; } = false;

    /// <summary>
    /// The charactor for delimiting field data.
    /// </summary>
    /// <remarks>
    /// The default value is ','.
    /// </remarks>
    public char Delimiter { get; set; } = ',';

    /// <summary>
    /// The charactor for escaping a double quote in a field data surrounded with double quotes.
    /// </summary>
    /// <remarks>
    /// The default value is '"'.
    /// </remarks>
    public char EscapeChar { get; set; } = '"';

    /// <summary>
    /// The newline string.
    /// </summary>
    /// <remarks>
    /// The default value is 'CrLf'.
    /// </remarks>
    public string NewLine { get; set; } = Environment.NewLine;

    /// <summary>
    /// Returns true when the specified char is delimiter.
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public bool IsDelimiterChar(char c)
    {
        return Delimiter == c;
    }

    /// <summary>
    /// Returns true when the specified char is escaping char.
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public bool IsEscapeChar(char c)
    {
        return EscapeChar == c;
    }

    /// <summary>
    /// Returns true when the specified char is the char of newline at the index.
    /// </summary>
    /// <remarks>
    /// Returns false when the index is over the length of newline string.
    /// </remarks>
    /// <param name="index"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public bool IsNewLineCharAt(int index, char c)
    {
        if (index < NewLine.Length)
        {
            return NewLine[index] == c;
        }
        else
        {
            return false;
        }
    }
}
