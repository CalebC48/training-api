using System;

namespace CAP.API.Exceptions;

[Serializable]
public class InvalidNetIdException : Exception
{
    public InvalidNetIdException(string message) : base(message)
    {
    }

    public InvalidNetIdException() : base()
    {
    }

    protected InvalidNetIdException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
}