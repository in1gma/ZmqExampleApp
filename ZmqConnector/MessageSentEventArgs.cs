using System;

namespace ZmqConnector
{
    public class MessageSentEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
