using System;

namespace ZmqConnector
{
    public interface IReceiver
    {
        void Run();
        event EventHandler<MessageReceivedEventArgs> SubMessageReceived;
        event EventHandler<MessageReceivedEventArgs> PullMessageReceived;
    }
}
