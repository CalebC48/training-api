using System;

namespace CAP.API.Exceptions;

[Serializable]
public class InvalidPutIdRequestException : Exception
{
    public InvalidPutIdRequestException(string message) : base(message)
    {
    }

    public InvalidPutIdRequestException() : base()
    {
    }

    protected InvalidPutIdRequestException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
}