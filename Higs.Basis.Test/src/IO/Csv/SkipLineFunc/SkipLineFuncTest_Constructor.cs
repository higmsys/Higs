using Higs.Basis.IO.Csv;

namespace Higs.Basis.Test.IO.Csv;

public class SkipLineFuncTest_Constructor : SkipLineFuncTest
{
    public SkipLineFuncTest_Constructor(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Fact]
    public void TestDefaultConstructor_ReturnsFalse()
    {
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var func = new SkipLineFunc();

        // --------------------------
        // Invoke
        // --------------------------
        var res = func.IsSkip(0, '#');

        // --------------------------
        // Inspecting
        // --------------------------
        AssertFalse(res);
    }

    [Theory]
    [InlineData("Returns false when char array is empty.", new char[0], 0, '#', false)]
    [InlineData("Returns true when char array contains the 1st char (array length = 1).", new char[] { '#' }, 0, '#', true)]
    [InlineData("Returns true when char array contains the 1st char (array length = 2 or more).", new char[] { ';', '#' }, 0, '#', true)]
    [InlineData("Returns false when char array doesn't contain the 1st char.", new char[] { ';' }, 0, '#', false)]
    public void TestConstructorWithCharArray(string title, char[] chars, int index, char c1st, bool expected)
    {
        WriteLine();
        WriteLine(title);
        WriteLine($" chars   : [{string.Join(", ", chars.Select(c => $"'{c}'"))}]");
        WriteLine($" index   : {index}");
        WriteLine($" c1st    : {c1st}");
        WriteLine($" expected: {expected}");
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var func = new SkipLineFunc(chars);

        // --------------------------
        // Invoke
        // --------------------------
        var res = func.IsSkip(index, c1st);

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(expected, res);

    }


    [Theory]
    [InlineData("Returns false when skip count is 0.", 0, 0, '#', false)]
    [InlineData("Returns true when skip count is 1 and index is 0.", 1, 0, '#', true)]
    [InlineData("Returns false when skip count is 1 and index is 1.", 1, 1, '#', false)]
    [InlineData("Returns true when skip count is 2 and index is 0.", 2, 0, '#', true)]
    [InlineData("Returns true when skip count is 2 and index is 1.", 2, 1, '#', true)]
    [InlineData("Returns false when skip count is 2 and index is 2.", 2, 2, '#', false)]
    public void TestConstructorWithInt(string title, int count, int index, char c1st, bool expected)
    {
        WriteLine();
        WriteLine(title);
        WriteLine($" skip count: {count}");
        WriteLine($" index     : {index}");
        WriteLine($" c1st      : {c1st}");
        WriteLine($" expected  : {expected}");
        WriteLine();

        // --------------------------
        // Preparing
        // --------------------------
        var func = new SkipLineFunc(count);

        // --------------------------
        // Invoke
        // --------------------------
        var res = func.IsSkip(index, c1st);

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(expected, res);

    }
}