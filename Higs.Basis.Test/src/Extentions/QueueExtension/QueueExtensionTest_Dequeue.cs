using Higs.Basis.Extentions;

namespace Higs.Basis.Test.Extentions;

public class QueueExtensionTest_Dequeue : ArrayExtensionTest
{
    public QueueExtensionTest_Dequeue(ITestOutputHelper outputHelper) : base(outputHelper) { }

    [Theory]
    [InlineData("No data is dequeue when specified 0.", new string[] { "A" }, 0, new string[] { })]
    [InlineData("The first data is dequeue when specified 1.", new string[] { "A", "B" }, 1, new string[] { "A" })]
    [InlineData("The first two data are dequeue when specified 2.", new string[] { "A", "B" }, 2, new string[] { "A", "B" })]
    public void Test(string title, string[] queueSrc, int count, string[] expected)
    {
        WriteLine();
        WriteLine(title);
        WriteLine($" queue: [{string.Join(", ", queueSrc.Select(s => $"'{s}'"))}]");
        WriteLine($" count: {count}");
        WriteLine();

        // --------------------------
        // Preparing
        // -------------------------- 
        var queue = new Queue<string>(queueSrc);

        // --------------------------
        // Invoke
        // --------------------------
        var res = queue.Dequeue(count);

        // --------------------------
        // Inspecting
        // --------------------------
        AssertEqual(queueSrc.Length - expected.Length, queue.Count, "Remaining count");
        AssertEqual(expected, res, $"dequeue data");
    }
}