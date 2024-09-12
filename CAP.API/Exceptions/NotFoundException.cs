using System;

namespace CAP.API.Exceptions;

/// <summary>
///  Example exception for when a resource is not found
/// </summary>
[Serializable]
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException() : base()
    {
    }

    protected NotFoundException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
}