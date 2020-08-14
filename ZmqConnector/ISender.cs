using System;

namespace ZmqConnector
{
    public interface ISender
    {
        void Run();
        void SendMessage(string message);
        event EventHandler<MessageSentEventArgs> PushMessageSent;
    }
}
