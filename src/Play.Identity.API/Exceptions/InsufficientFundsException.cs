using System;

namespace Play.Identity.API.Exceptions;

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(Guid userId, decimal gilToDebit) 
        : base($"Insufficient funds for user with id: {userId}, gil: {gilToDebit}")
    {
        UserId = userId;
        GilToDebit = gilToDebit;
    }
    
    public Guid UserId { get; }
    public decimal GilToDebit { get; }
}