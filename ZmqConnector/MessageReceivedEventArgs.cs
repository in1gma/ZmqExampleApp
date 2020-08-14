using System;

namespace ZmqConnector
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
