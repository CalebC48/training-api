using System;

namespace CAP.API.Exceptions;

[Serializable]
public class InvalidFileTypeException : Exception
{
    public InvalidFileTypeException(string message) : base(message)
    {
    }

    public InvalidFileTypeException() : base()
    {
    }

    protected InvalidFileTypeException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
}