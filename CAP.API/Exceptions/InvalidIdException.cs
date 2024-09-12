using System;

namespace CAP.API.Exceptions;

[Serializable]
public class InvalidIdException : Exception
{
    public InvalidIdException(string message) : base(message)
    {
    }

    public InvalidIdException() : base()
    {
    }


    protected InvalidIdException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
}