namespace ZmqConnector
{
    /// <summary>
    /// Настройки ZMQ соединения
    /// </summary>
    public class ConnectionSettings
    {
        /// <summary>
        /// Протокол
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// Имя хоста
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Номер порта Push
        /// </summary>
        public int PushPort { get; set; }

        /// <summary>
        /// Номер порта Pull
        /// </summary>
        public int PullPort { get; set; }

        /// <summary>
        /// Номер порта Sub
        /// </summary>
        public int SubPort { get; set; }

        /// <summary>
        /// Имя топика для подписки
        /// </summary>
        public string SubTopic { get; set; }

        /// <summary>
        /// Полный адрес Push сокета
        /// </summary>
        public string PushAddress => GetAddress(Protocol, Host, PushPort);

        /// <summary>
        /// Полный адрес Pull сокета
        /// </summary>
        public string PullAddress => GetAddress(Protocol, Host, PullPort);

        /// <summary>
        /// Полный адрес Sub сокета
        /// </summary>
        public string SubAddress => GetAddress(Protocol, Host, SubPort);

        private static string GetAddress(string protocol, string host, int port)
        {
            return $"{protocol}://{host}:{port}";
        }
    }
}
