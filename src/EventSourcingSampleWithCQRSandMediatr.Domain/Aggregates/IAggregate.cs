using System;

namespace EventSourcingSampleWithCQRSandMediatr.Domain.Aggregates
{
    public interface IAggregate
    {
        Guid Id { get; }
    }
}
