using BasicStore.Core.Messages;

namespace BasicStore.Core.Bus
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T evento) where T : Event;

        Task<bool> EnviarComando<T>(T comando) where T : Command;

    }
}
