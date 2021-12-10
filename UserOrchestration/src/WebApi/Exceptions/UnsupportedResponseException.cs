using System.Runtime.Serialization;

namespace UserOrchestration.Exceptions;

public class UnsupportedResponseException : Exception
{
    public UnsupportedResponseException()
    {
    }

    public UnsupportedResponseException(string? message) : base(message)
    {
    }

    public UnsupportedResponseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected UnsupportedResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}