using BasicStore.Core.Messages;

namespace BasicStore.Core.Bus
{
    public interface IMediatrHandler
    {
        Task PublishEvent<T>(T evento) where T : Event;
    }
}
