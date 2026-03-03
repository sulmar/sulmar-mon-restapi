using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
    protected DomainException(string message, Exception innerException) : base(message, innerException) { }
}

public class OrderAlreadyPaidException : DomainException
{
    public OrderAlreadyPaidException() : base("Order has already been paid") { }
}


public class InvalidStateTransitionException : DomainException
{
    public InvalidStateTransitionException(string message) : base(message)
    {}
    public InvalidStateTransitionException(OrderStatus from, OrderStatus to)
        : base($"Invalid state transition from {from} to {to}") { }
}