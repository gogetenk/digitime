using System.Runtime.Serialization;

namespace Digitime.Shared.Exceptions;
public class BackendException : Exception
{
    public BackendException()
    {
    }

    public BackendException(string? message) : base(message)
    {
    }

    public BackendException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected BackendException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
