namespace Higs.Basis;

public class HigsException : Exception
{
    public HigsException() : base() { }

    public HigsException(string? message) : base(message) { }

    public HigsException(Exception? innerException) : base(null, innerException) { }

    public HigsException(string? message, Exception? innerException) : base(message, innerException) { }

}
