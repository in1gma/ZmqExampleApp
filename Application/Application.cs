using System.Threading.Tasks;
using Logic;
using ZmqConnector;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application
{
    public class Application
    {
        private readonly ApplicationSettings _settings;

        private readonly ILogger<Application> _logger;
        private readonly IReceiver _receiver;
        private readonly ISender _sender;
        private readonly ITextOperations _operations;

        public Application(IOptions<ApplicationSettings> settings, ILogger<Application> logger, IReceiver receiver, ISender sender, ITextOperations operations)
        {
            _settings = settings.Value;
            _logger = logger;
            _receiver = receiver;
            _sender = sender;
            _operations = operations;

            _receiver.SubMessageReceived += OnSubMessage;
            _receiver.PullMessageReceived += OnPullMessage;
            _sender.PushMessageSent += OnPushMessage;

            _logger.LogInformation(_settings.Tag);
        }

        public void Start()
        {
            var receiveTask = Task.Run(() =>
            {
                _receiver.Run();
            });

            _sender.Run();

            Task.WaitAll(receiveTask);
        }

        private void OnSubMessage(object sender, MessageReceivedEventArgs e)
        {
            // просто получаем длину сообщения
            var length = _operations.GetLength(e.Message);
            
            // и отправляем обратно
            _sender.SendMessage(length.ToString());
        }

        private void OnPullMessage(object sender, MessageReceivedEventArgs e)
        {
            // ...
        }

        private void OnPushMessage(object sender, MessageSentEventArgs e)
        {
            // ...
        }
    }
}
