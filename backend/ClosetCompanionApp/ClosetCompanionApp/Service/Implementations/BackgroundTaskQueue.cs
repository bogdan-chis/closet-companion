using System.Threading.Channels;

namespace ClosetCompanionApp.Service.Implementations
{
    public interface IBackgroundTaskQueue
    {
        void QueueGeneration(Guid generatedOutfitId);
        ChannelReader<Guid> Reader { get; }
    }

    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Guid> _channel = Channel.CreateUnbounded<Guid>();
        public ChannelReader<Guid> Reader => _channel.Reader;

        public void QueueGeneration(Guid generatedOutfitId) => _channel.Writer.TryWrite(generatedOutfitId);
    }
}