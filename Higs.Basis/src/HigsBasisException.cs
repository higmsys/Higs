using Higs.Basis.IO.Csv;

namespace Higs.Basis;

public class HigsBasisException : HigsException
{
    public HigsBasisException() : base() { }

    public HigsBasisException(string? message) : base(message) { }

    public HigsBasisException(Exception? innerException) : base(null, innerException) { }

    public HigsBasisException(string? message, Exception? innerException) : base(message, innerException) { }


    internal static HigsBasisException FoundUnexpectedCsvToken(CsvToken token, int lineCount)
    {
        var msg = $"Unexpected token '{token.RawValue}' is found at the {lineCount}th line.";
        return new HigsBasisException(msg);
    }

    internal static HigsBasisException NotFoundCsvFieldName(string fieldName)
    {
        var msg = $"The specified field name '{fieldName}' does not found.";
        return new HigsBasisException(msg);
    }
}
