namespace Higs.Basis.IO.Csv;

public sealed class SkipLineFunc
{
    private readonly Func<int, char, bool> _func;

    public SkipLineFunc()
    {
        _func = (index, c1st) => { return false; };
    }

    public SkipLineFunc(int skipLineCount)
    {
        _func = (index, c1st) => { return index < skipLineCount; };
    }

    public SkipLineFunc(char[] skipChars)
    {
        _func = (index, c1st) => { return skipChars.Contains(c1st); };
    }

    public bool IsSkip(int index, char c1st)
    {
        return _func.Invoke(index, c1st);
    }
}
