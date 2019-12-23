using MediatR;

namespace EventSourcingSampleWithCQRSandMediatr.Domain.Queries
{
    public interface IQuery<out TResponse>: IRequest<TResponse>
    {
    }
}
