using MediatR;

namespace EventSourcingSampleWithCQRSandMediatr.Domain.Queries
{
    public interface IQueryHandler<in TQuery, TResponse>: IRequestHandler<TQuery, TResponse>
           where TQuery : IQuery<TResponse>
    {
    }
}
