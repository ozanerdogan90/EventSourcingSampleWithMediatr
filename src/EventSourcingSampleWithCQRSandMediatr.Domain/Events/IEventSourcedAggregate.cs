using System.Collections.Generic;
using EventSourcingSampleWithCQRSandMediatr.Domain.Events;

namespace EventSourcingSampleWithCQRSandMediatr.Domain.Aggregates
{
    public interface IEventSourcedAggregate: IAggregate
    {
        Queue<IEvent> PendingEvents { get; }
    }
}
