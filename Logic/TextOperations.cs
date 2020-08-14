using Microsoft.Extensions.Logging;

namespace Logic
{
    public class TextOperations : ITextOperations
    {
        private readonly ILogger<TextOperations> _logger;

        public TextOperations(ILogger<TextOperations> logger)
        {
            _logger = logger;
        }

        public int GetLength(string message)
        {
            var length = message.Length;

            _logger.LogTrace($"Message: '{message}',  length: {length}");

            return length;
        }
    }
}
