namespace Higs.Basis.IO.Csv;

public sealed class CsvToken
{
    public CsvToken(CsvTokenType type, string rawValue, string? value = null)
    {
        Type = type;
        RawValue = rawValue;
        Value = value ?? rawValue;
    }

    public string RawValue { get; }

    public CsvTokenType Type { get; }

    public string Value { get; }

    public bool IsAnyOf(params CsvTokenType[] types)
    {
        return types.Any(t => t == Type);
    }

    public override bool Equals(object? obj)
    {
        var isEq = false;

        if (obj is CsvToken other)
        {
            isEq = this.Type == other.Type
                && this.Value == other.Value;
        }

        return isEq;
    }

    public override int GetHashCode()
    {
        var hash = 0;

        hash += Type.GetHashCode();
        hash += Value.GetHashCode();

        return hash;
    }

    public override string ToString()
    {
        return $"{{Type='{Type}', Value='{Value}'}}";
    }
}
