using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;

namespace ZmqConnector
{
    public class Receiver : IReceiver
    {
        private readonly ConnectionSettings _settings;

        private readonly ILogger<Receiver> _logger;

        public Receiver(IOptions<ConnectionSettings> settings, ILogger<Receiver> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public void Run()
        {
            var subAddress = _settings.SubAddress;
            var pullAddress = _settings.PullAddress;
            var topic = _settings.SubTopic;

            using (var subscriber = new SubscriberSocket())
            using (var puller = new PullSocket())
            {
                subscriber.Connect(subAddress);
                subscriber.Subscribe(topic);
                _logger.LogInformation($"Sub socket on {subAddress} topic {topic}");

                puller.Connect(pullAddress);
                _logger.LogInformation($"Pull socket on {pullAddress}");

                using (var poller = new NetMQPoller { subscriber, puller })
                {
                    subscriber.ReceiveReady += SubReceiveReady;
                    puller.ReceiveReady += PullReceiveReady;

                    poller.Run();
                }
            }
        }

        private void SubReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            var message = e.Socket.ReceiveFrameString();

            _logger.LogTrace($"Sub: {message}");

            OnSubMessageReceived(new MessageReceivedEventArgs
            {
                Message = message
            });
        }

        private void PullReceiveReady(object sender, NetMQSocketEventArgs e)
        {
            var message = e.Socket.ReceiveFrameString();

            _logger.LogTrace($"Pull: {message}");

            OnPullMessageReceived(new MessageReceivedEventArgs
            {
                Message = message
            });
        }

        protected virtual void OnSubMessageReceived(MessageReceivedEventArgs e)
        {
            var handler = SubMessageReceived;
            handler?.Invoke(this, e);
        }

        protected virtual void OnPullMessageReceived(MessageReceivedEventArgs e)
        {
            var handler = PullMessageReceived;
            handler?.Invoke(this, e);
        }

        public event EventHandler<MessageReceivedEventArgs> SubMessageReceived;
        public event EventHandler<MessageReceivedEventArgs> PullMessageReceived;
    }
}
