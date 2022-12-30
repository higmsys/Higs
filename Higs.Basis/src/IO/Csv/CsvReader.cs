using System.Text;
using Higs.Basis.Extentions;

namespace Higs.Basis.IO.Csv;

public sealed class CsvReader : IDisposable
{
    private readonly StreamReader _reader;
    private readonly CsvTokenizer _csvTokenizer;
    private readonly CsvOption _option;
    private readonly CsvHeaders _headers;
    private int _totalReadLineCount;
    private string[] _skipLines;

    public CsvReader(StreamReader reader, CsvOption? option = null)
    {
        _reader = reader;
        _option = (option != null) ? new CsvOption(option) : CreateDefaultOption();
        _totalReadLineCount = 0;

        _csvTokenizer = new CsvTokenizer(reader, _option);

        _skipLines = SkipLeadingLines();
        if (_option.HasHeader)
        {
            var headerFields = ReadFields() ?? new string[0];
            _headers = new CsvHeaders(headerFields);
        }
        else
        {
            _headers = new CsvHeaders();
        }
    }

    public CsvHeaders Headers => _headers;

    private string[] SkipLeadingLines()
    {
        var lineList = new List<string>();
        var isContinuing = true;

        while (isContinuing)
        {
            var c1st = _csvTokenizer.PeekNextChar();

            if (c1st != null)
            {
                var isSkip = _option.SkipLeadingLine.IsSkip(lineList.Count, (char)c1st);

                if (isSkip)
                {
                    var line = _csvTokenizer.DisposeNextLine();
                    lineList.Add(line);
                }
                else
                {
                    isContinuing = false;
                }
            }
            else
            {
                isContinuing = false;
            }
        }

        return lineList.ToArray();
    }

    public ICsvLine? ReadLine()
    {
        var rawLineBuf = new StringBuilder(1024);
        var fields = ReadLineFields(rawLineBuf);

        if (0 < fields.Length)
        {
            var lineIndex = _totalReadLineCount - 1;
            if (_option.HasHeader) { lineIndex--; }

            var csvLine = new CsvLine(
                lineIndex,
                rawLineBuf.ToString(),
                _headers,
                fields);
            return csvLine;
        }
        else
        {
            return null;
        }
    }

    public void Close()
    {
        _reader.Close();
    }

    public void Dispose()
    {
        _reader.Dispose();
    }

    public string[]? ReadFields()
    {
        var fields = ReadLineFields();

        if (0 < fields.Length)
        {
            return fields;
        }
        else
        {
            return null;
        }
    }

    private string[] ReadLineFields(StringBuilder? rawLineBuf = null)
    {
        _totalReadLineCount++;
        var dataList = new List<string>((_headers != null && 0 < _headers.Count) ? _headers.Count : 32);

        var isReading = true;
        while (isReading && _csvTokenizer.HasNext())
        {
            var token = _csvTokenizer.Next();

            switch (token.Type)
            {
                case CsvTokenType.FieldData:
                    dataList.Add(token.Value);
                    break;
                case CsvTokenType.EndOfLine:
                case CsvTokenType.EndOfFile:
                    isReading = false;
                    break;
                case CsvTokenType.Delimiter:
                    // do nothing
                    break;
                default:
                    throw HigsBasisException.FoundUnexpectedCsvToken(token, _totalReadLineCount);
            }

            if (isReading && rawLineBuf != null)
            {
                rawLineBuf.Append(token.RawValue);
            }
        }

        return dataList.ToArray();
    }

    private static CsvOption CreateDefaultOption()
    {
        return new CsvOption
        {
            Delimiter = ',',
            EscapeChar = '"',
            HasHeader = false,
        };
    }

    class CsvLine : ICsvLine
    {
        private readonly int _lineIndex;
        private readonly string _originalLine;
        private readonly string[] _values;
        private readonly CsvHeaders _headers;

        public CsvLine(int lineIndex, string originalLine, CsvHeaders headers, string[] values)
        {
            _lineIndex = lineIndex;
            _originalLine = originalLine;
            _headers = headers;
            _values = values;
        }

        public string this[int fieldIndex] => _values[fieldIndex];

        public string this[string fieldName]
        {
            get
            {
                var index = _headers.FindFirstIndex(fieldName);

                if (0 <= index)
                {
                    return this[index];
                }
                else
                {
                    throw HigsBasisException.NotFoundCsvFieldName(fieldName);
                }
            }
        }

        public string RawValue => _originalLine;

        public int LineIndex => _lineIndex;

        public int FieldCount => _values.Length;

        public string[] ToArray()
        {
            return _values.Duplicate();
        }

    }
}
