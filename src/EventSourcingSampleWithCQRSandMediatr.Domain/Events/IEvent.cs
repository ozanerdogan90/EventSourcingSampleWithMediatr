using MediatR;

namespace EventSourcingSampleWithCQRSandMediatr.Domain.Events
{
    public interface IEvent: INotification
    {
    }
}
