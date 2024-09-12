using System;

namespace CAP.API.Exceptions;

[Serializable]
public class InvalidJwtException : Exception
{
    public InvalidJwtException(string message) : base(message)
    {
    }

    public InvalidJwtException()
    {
    }

    protected InvalidJwtException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
}