using System.Runtime.CompilerServices;

namespace Higs.Basis.Test;

public abstract class Test
{
    private readonly ITestOutputHelper _outputHelper;

    public Test(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    public static string GetTestDataPath(string relativePath = "", string filename = "")
    {
        var dirPath = Path.Combine("../../../testdata/", relativePath);
        var dirInfo = new DirectoryInfo(dirPath);

        if (string.IsNullOrEmpty(filename))
        {
            return dirInfo.FullName;
        }
        else
        {
            return Path.Combine(dirInfo.FullName, filename);
        }
    }

    public void AssertEqual<T>(
        T? expected,
        T? actual,
        string title = "")
    {
        try
        {
            Assert.Equal(expected, actual);
            WriteAssertionResult(expected, actual, ok: true, title);
        }
        catch
        {
            WriteAssertionResult(expected, actual, ok: false, title);
            throw;
        }
    }

    public void AssertTrue(
        bool? actual,
        string title = "")
    {
        var expected = true;

        try
        {
            Assert.True(actual);
            WriteAssertionResult(expected, actual, ok: true, title);
        }
        catch
        {
            WriteAssertionResult(expected, actual, ok: false, title);
            throw;
        }
    }

    public void AssertFalse(
        bool? actual,
        string title = "")
    {
        var expected = false;
        try
        {
            Assert.False(actual);
            WriteAssertionResult(expected, actual, ok: true, title);
        }
        catch
        {
            WriteAssertionResult(expected, actual, ok: false, title);
            throw;
        }
    }

    public void AssertNull(
        object actual,
        string title = "")
    {
        var expected = null as object;
        try
        {
            Assert.Null(actual);
            WriteAssertionResult(expected, actual, ok: true, title);
        }
        catch
        {
            WriteAssertionResult(expected, actual, ok: false, title);
            throw;
        }
    }

    public void AssertNotNull(
        object actual,
        string title = "")
    {
        var expected = "Object";
        try
        {
            Assert.NotNull(actual);
            WriteAssertionResult(expected, actual, ok: true, title);
        }
        catch
        {
            WriteAssertionResult(expected, actual, ok: false, title);
            throw;
        }
    }

    public void AssertThrows<T>(
        Action action,
        string title = "") where T : Exception
    {
        try
        {
            Assert.Throws<T>(action);
            WriteAssertionResult(typeof(T), typeof(T), ok: true, title);
        }
        catch (Exception e)
        {
            WriteAssertionResult(typeof(T), null, ok: false, title);
            throw e;
        }
    }

    private void WriteAssertionResult<T>(
        T? expected,
        T? actual,
        bool ok,
        string title = "",
        [CallerMemberName] string memberName = "")
    {
        var res = (ok) ? "OK" : "NG";

        WriteLine($"[{memberName}({res})] {ToEscapedString(title)}");

        var toOutputStr = (T? t) =>
        {
            if (t == null) { return "null"; }
            if (!t.GetType().Equals(typeof(string)) && t is System.Collections.IEnumerable array)
            {
                var list = new List<string>();
                foreach (var a in array)
                {
                    if (a == null)
                    {
                        list.Add($"null");
                    }
                    else
                    {
                        list.Add($"'{a}'");
                    }
                }

                return "[" + string.Join(",", list.Select(s => $"{s}")) + "]";
            }

            return $"'{t}'";
        };

        var expStr = toOutputStr(expected);
        var actStr = toOutputStr(actual);

        WriteLine($" exp: {expStr}");
        WriteLine($" act: {actStr}");
    }

    public void WriteLine(string message = "")
    {
        _outputHelper.WriteLine($"      {ToEscapedString(message)}");
    }

    protected string ToEscapedString(string original)
    {
        return original
            .Replace("\r", "\\r")
            .Replace("\n", "\\n")
            .Replace("\t", "\\t")
            .Replace("\v", "\\v")
        ;
    }
}
