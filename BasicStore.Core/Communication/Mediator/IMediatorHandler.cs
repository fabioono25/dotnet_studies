using BasicStore.Core.DomainObjects;
using BasicStore.Core.Messages;
using BasicStore.Core.Messages.Common.Notifications;

namespace BasicStore.Core.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<bool> EnviarComando<T>(T comando) where T : Command;
        Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification;
        Task PublicarDomainEvent<T>(T notificacao) where T : DomainEvent;
        Task PublishEvent(ProductBellowStockEvent productBellowStockEvent);
    }

    public class ProductBellowStockEvent
    {
    }
}
