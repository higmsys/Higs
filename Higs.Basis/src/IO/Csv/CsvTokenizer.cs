using System.Text;
using Higs.Basis.Extentions;

namespace Higs.Basis.IO.Csv;

public sealed class CsvTokenizer
{
    private static readonly int MaxQueueSize = 1024 * 8;

    private static readonly char SurroundingChar = '"';

    private readonly CsvOption _option;

    private StreamReader _reader;

    private Queue<char> _uncompletedCharQueue;

    // Since this variable is used only within the ReadFieldData() method,
    //  it doesn't need to be defined here to work.
    // However, to reduce the cost of instantiation and increase speed, it is defined here.
    private FieldData _workFieldData = new FieldData();
    private readonly CsvToken _delimiterToken;
    private readonly CsvToken _endOfLineToken;

    public CsvTokenizer(StreamReader reader, CsvOption option)
    {
        _reader = reader;
        _option = new CsvOption(option);
        _uncompletedCharQueue = new Queue<char>(MaxQueueSize);

        _delimiterToken = new CsvToken(CsvTokenType.Delimiter, _option.Delimiter.ToString());
        _endOfLineToken = new CsvToken(CsvTokenType.EndOfLine, _option.NewLine);
    }


    public CsvToken? LatestToken { get; private set; } = null;

    public bool HasNext()
    {
        if (0 == _uncompletedCharQueue.Count && _reader.EndOfStream)
        {   // There are no data.

            // When the latest token is a delimiter,
            //  a last token still remains.
            if (LatestToken != null && LatestToken.Type == CsvTokenType.Delimiter)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public char? PeekNextChar()
    {
        if (_uncompletedCharQueue.Count == 0 && !_reader.EndOfStream)
        {
            ReadCharToQueue();
        }

        if (0 < _uncompletedCharQueue.Count)
        {
            return _uncompletedCharQueue.Peek();
        }
        else
        {
            return null;
        }
    }

    public string DisposeNextLine()
    {
        var buf = new StringBuilder();
        var newLineLength = _option.NewLine.Length;

        var isSearching = true;
        while (isSearching)
        {
            // Read charactors to the queue when the queue data isn't sufficient.
            if (_uncompletedCharQueue.Count < newLineLength) { ReadCharToQueue(); }

            if (_uncompletedCharQueue.Count < newLineLength)
            {
                while (0 < _uncompletedCharQueue.Count)
                {
                    buf.Append(_uncompletedCharQueue.Dequeue());
                }
                isSearching = false;
            }
            else
            {
                var c = _uncompletedCharQueue.Peek();

                var isNewLine = _option.IsNewLineCharAt(0, c) && _uncompletedCharQueue.StartsWith(_option.NewLine);

                if (isNewLine)
                {
                    _uncompletedCharQueue.Dequeue(newLineLength);
                    isSearching = false;
                }
                else
                {
                    buf.Append(_uncompletedCharQueue.Dequeue());
                }
            }
        }

        return buf.ToString();
    }

    public CsvToken Next()
    {
        var maxCharCountPerOnce = _option.NewLine.Length;

        if (_uncompletedCharQueue.Count < maxCharCountPerOnce) { ReadCharToQueue(); }

        CsvTokenType type;
        string rawValue;
        string value;

        if (_uncompletedCharQueue.Count == 0)
        {
            // The end of stream
            if (LatestToken != null && LatestToken.Type == CsvTokenType.Delimiter)
            {
                type = CsvTokenType.FieldData;
                rawValue = "";
                value = rawValue;
            }
            else
            {
                type = CsvTokenType.EndOfFile;
                rawValue = "";
                value = rawValue;
            }
        }
        else
        {
            // Fetch the leading char and determine the type.
            var c1st = _uncompletedCharQueue.Peek();
            var isDelimiterChar = _option.IsDelimiterChar(c1st);
            var isNewLine = _option.IsNewLineCharAt(0, c1st) && _uncompletedCharQueue.StartsWith(_option.NewLine);

            if (isDelimiterChar)
            {
                // [Delimiter]
                // When the previous token is field data, current data is delimiter.
                // Otherwise returns a empty field data because there is a empty field data between previous token and current delimiter.
                if (LatestToken != null && LatestToken.Type == CsvTokenType.FieldData)
                {
                    type = CsvTokenType.Delimiter;
                    // data.Append(_uncompletedCharQueue.Dequeue());
                    rawValue = _uncompletedCharQueue.Dequeue().ToString();
                    value = rawValue;
                }
                else
                {
                    type = CsvTokenType.FieldData;
                    rawValue = "";
                    value = rawValue;
                }
            }
            else if (isNewLine)
            {
                // [NewLine]
                // When the previous token is field data, current data is NewLine.
                // Otherwise returns a empty field data because there is a empty field data between previous token and current data.
                if (LatestToken == null || !(LatestToken.Type == CsvTokenType.FieldData))
                {
                    type = CsvTokenType.FieldData;
                    // data.Append("");
                    rawValue = "";
                    value = rawValue;
                }
                else
                {
                    type = CsvTokenType.EndOfLine;
                    // data.Append(_uncompletedCharQueue.Dequeue(_option.NewLine.Length));
                    _uncompletedCharQueue.Dequeue(_option.NewLine.Length);
                    rawValue = _option.NewLine;
                    value = rawValue;
                }
            }
            else
            {
                // [FieldData]
                // Current data is field data.
                // But the previous token is also field data, current data is unknown.
                if (LatestToken != null && LatestToken.Type == CsvTokenType.FieldData)
                {
                    type = CsvTokenType.Unknown;
                    // data.Append(_uncompletedCharQueue.Dequeue());
                    rawValue = _uncompletedCharQueue.Dequeue().ToString();
                    value = rawValue;
                }
                else
                {
                    type = CsvTokenType.FieldData;

                    var data = ReadFieldData();
                    rawValue = data.GetRawValue();
                    value = data.GetValue();
                }
            }
        }

        switch (type)
        {
            case CsvTokenType.Delimiter:
                LatestToken = _delimiterToken;
                break;
            case CsvTokenType.EndOfLine:
                LatestToken = _endOfLineToken;
                break;
            default:
                LatestToken = new CsvToken(
                    type,
                    rawValue,
                    value);
                break;
        }

        return LatestToken;
    }

    private void ReadCharToQueue()
    {
        // Read charactors as much as for free space of queue.
        var freeSpace = MaxQueueSize - _uncompletedCharQueue.Count;

        if (0 < freeSpace && !_reader.EndOfStream)
        {
            var chars = new char[freeSpace];
            var count = _reader.Read(chars, 0, chars.Length);

            // It works without count judging, but Linq is slow,
            //  so do judging to improve processing speed.
            if (chars.Length == count)
            {
                _uncompletedCharQueue.Enqueue(chars);
            }
            else
            {
                _uncompletedCharQueue.Enqueue(chars.Take(count));
            }
        }
    }

    private FieldData ReadFieldData()
    {
        _workFieldData.Clear();
        var c1st = _uncompletedCharQueue.Dequeue();

        var isOpenedWithSurroundedChar = (c1st == SurroundingChar);
        _workFieldData.Append(c1st, onlyRaw: isOpenedWithSurroundedChar);

        while (true)
        {
            if (_uncompletedCharQueue.Count < _option.NewLine.Length)
            {
                ReadCharToQueue();
            }
            if (_uncompletedCharQueue.Count == 0) { break; }

            var c = _uncompletedCharQueue.Peek();

            if (isOpenedWithSurroundedChar)
            {
                var isSurroundedChar = (c == SurroundingChar);
                var isEscapedChar = _option.IsEscapeChar(c) &&
                        _uncompletedCharQueue
                            .StartsWith(new char[] { _option.EscapeChar, SurroundingChar });

                if (isEscapedChar)
                {
                    // Dispose of the escaping char.
                    _workFieldData.Append(_uncompletedCharQueue.Dequeue(), onlyRaw: true);
                    // Append the surrounded char.
                    _workFieldData.Append(_uncompletedCharQueue.Dequeue());
                }
                else if (isSurroundedChar)
                {
                    // Dispose of the surrounded char.
                    _workFieldData.Append(_uncompletedCharQueue.Dequeue(), onlyRaw: true);
                    break;
                }
                else
                {
                    _workFieldData.Append(_uncompletedCharQueue.Dequeue());
                }
            }
            else
            {
                var isNewLine = _option.IsNewLineCharAt(0, c) &&
                        _uncompletedCharQueue.StartsWith(_option.NewLine);

                if (isNewLine || _option.IsDelimiterChar(c))
                {
                    break;
                }
                else
                {
                    _workFieldData.Append(_uncompletedCharQueue.Dequeue());
                }
            }
        }

        return _workFieldData;
    }

    private class FieldData
    {
        private StringBuilder _rawValueBuf = new StringBuilder(1024);

        private StringBuilder _valueBuf = new StringBuilder(1024);

        public string GetRawValue()
        {
            return _rawValueBuf.ToString();
        }

        public string GetValue()
        {
            return _valueBuf.ToString();
        }

        public void Append(char c, bool onlyRaw = false)
        {
            _rawValueBuf.Append(c);

            if (!onlyRaw) { _valueBuf.Append(c); }
        }

        public void Clear()
        {
            _rawValueBuf.Clear();
            _valueBuf.Clear();
        }
    }
}
