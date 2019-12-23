using MediatR;

namespace EventSourcingSampleWithCQRSandMediatr.Domain.Commands
{
    public interface ICommandHandler<in T>: IRequestHandler<T>
        where T : ICommand
    {
    }
}
