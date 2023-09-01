using BasicStore.Core.Messages;
using BasicStore.Core.Messages.Common.Notifications;

namespace BasicStore.Core.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T evento) where T : Event;

        Task<bool> EnviarComando<T>(T comando) where T : Command;

        Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification;
    }
}
