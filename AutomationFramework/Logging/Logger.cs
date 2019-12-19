using System;
using System.IO;
using System.Threading;
using AutomationFramework.Utilities;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace AutomationFramework.Logging
{
    public sealed class Logger
    {
        private static readonly Lazy<Logger> LazyInstance = new Lazy<Logger>(() => new Logger());
        private static readonly ThreadLocal<ILogger> Log = new ThreadLocal<ILogger>(() => LogManager.GetLogger(Thread.CurrentThread.ManagedThreadId.ToString()));

        private Logger()
        {
            try
            {
                LogManager.LoadConfiguration("NLog.config");
            }
            catch (FileNotFoundException)
            {
                LogManager.Configuration = GetConfiguration();
            }
        }

        private LoggingConfiguration GetConfiguration()
        {
            var layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss} ${level:uppercase=true} - ${message}";
            var config = new LoggingConfiguration();
            var fileName = Path.Combine(FileProvider.GetOutputDirectory(), "log.log");
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            config.AddRule(LogLevel.Info, LogLevel.Fatal, new ConsoleTarget("logconsole")
            {
                Layout = layout
            });
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, new FileTarget("logfile")
            {
                FileName = fileName,
                Layout = layout
            });
            return config;
        }

        public static Logger Instance => LazyInstance.Value;

        public static string GetLogLocation()
        {
            return Path.Combine(FileProvider.GetOutputDirectory(), "log.log");
        }

        public void Debug(string message, Exception exception = null)
        {
            Log.Value.Debug(exception, message);
        }

        public void Info(string message)
        {
            Log.Value.Info(message);
        }

        public void Warn(string message)
        {
            Log.Value.Warn(message);
        }

        public void Error(string message)
        {
            Log.Value.Error(message);
        }

        public void Fatal(string message, Exception exception)
        {
            Log.Value.Fatal(exception, message);
        }
    }
}
