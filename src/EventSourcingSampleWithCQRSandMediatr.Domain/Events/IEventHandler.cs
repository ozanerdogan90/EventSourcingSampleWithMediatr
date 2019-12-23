using MediatR;

namespace EventSourcingSampleWithCQRSandMediatr.Domain.Events
{
    public interface IEventHandler<in TEvent>: INotificationHandler<TEvent>
           where TEvent : IEvent
    {
    }
}
