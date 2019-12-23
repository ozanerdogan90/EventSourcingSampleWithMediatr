using System.Threading.Tasks;

namespace EventSourcingSampleWithCQRSandMediatr.Domain.Commands
{
    public interface ICommandBus
    {
        Task Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
