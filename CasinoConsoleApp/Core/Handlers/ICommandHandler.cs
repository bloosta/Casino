using System.Threading.Tasks;
using CasinoConsoleApp.Core.Commands;

namespace CasinoConsoleApp.Core.Handlers
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
