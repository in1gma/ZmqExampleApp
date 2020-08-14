using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;

namespace ZmqConnector
{
    public class Sender : ISender
    {
        private readonly ConnectionSettings _settings;

        private readonly ILogger<Sender> _logger;

        private PushSocket Socket { get; }

        public Sender(IOptions<ConnectionSettings> settings, ILogger<Sender> logger)
        {
            _settings = settings.Value;
            _logger = logger;

            Socket = new PushSocket();
        }

        public void Run()
        {
            var pushAddress = _settings.PushAddress;
            Socket.Connect(pushAddress);
            _logger.LogInformation($"Push socket on {pushAddress}");
        }

        public void SendMessage(string message)
        {
            _logger.LogTrace($"Push: {message}");

            OnPushMessageSent(new MessageSentEventArgs
            {
                Message = message
            });
            Socket.SendFrame(message);
        }

        protected virtual void OnPushMessageSent(MessageSentEventArgs e)
        {
            var handler = PushMessageSent;
            handler?.Invoke(this, e);
        }

        public event EventHandler<MessageSentEventArgs> PushMessageSent;
    }
}
