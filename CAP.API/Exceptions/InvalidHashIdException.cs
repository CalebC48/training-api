using System;

namespace CAP.API.Exceptions;

[Serializable]
public class InvalidHashIdException : Exception
{
    public InvalidHashIdException(string message) : base(message)
    {
    }

    public InvalidHashIdException() : base()
    {
    }

    protected InvalidHashIdException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
}