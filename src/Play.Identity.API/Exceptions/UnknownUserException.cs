using System;

namespace Play.Identity.API.Exceptions;

public class UnknownUserException : Exception
{
    public UnknownUserException(Guid userId) 
        : base($"Unknown user with id: {userId}.")
    {
        UserId = userId;
    }
    
    public Guid UserId { get; }
}