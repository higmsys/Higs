namespace Higs.Basis.Extentions;

public static class QueueExtension
{
    public static IEnumerable<T> Dequeue<T>(this Queue<T> queue, int count)
    {
        var res = new T[count];

        for (var i = 0; i < count; i++)
        {
            res[i] = queue.Dequeue();
        }

        return res;
    }

    public static IEnumerable<T> Enqueue<T>(this Queue<T> queue, IEnumerable<T> elements)
    {
        foreach (var elem in elements)
        {
            queue.Enqueue(elem);
        }

        return queue;
    }

}