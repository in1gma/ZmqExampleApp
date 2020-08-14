using System;
using System.IO;
using Logic;
using ZmqConnector;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                var configuration = BuildConfig();

                var servicesProvider = BuildDi(configuration);
                using (servicesProvider as IDisposable)
                {
                    var app = servicesProvider.GetRequiredService<Application>();
                    app.Start();
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Error");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        private static IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
        }

        private static IServiceProvider BuildDi(IConfiguration config)
        {
            return new ServiceCollection()
                .Configure<ApplicationSettings>(config.GetSection("Application"))
                .Configure<ConnectionSettings>(config.GetSection("Connection"))
                .AddSingleton<Application>()
                .AddSingleton<ISender, Sender>()
                .AddSingleton<IReceiver, Receiver>()
                .AddSingleton<ITextOperations, TextOperations>()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    loggingBuilder.AddNLog(config);
                })
                .BuildServiceProvider();
        }
    }
}
