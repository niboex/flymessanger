using System;
using System.Net;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using static log4net.Appender.FileAppender;

namespace vkapi
{
    class LogHelper
    {
//        private bool isLoad = false;
        public static log4net.ILog GetLogger(Type logger)
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            if(!hierarchy.Configured)
                LogHelper.Setup();

            return log4net.LogManager.GetLogger(logger);
        }


        private static void Setup()
        {
            Hierarchy hierarchy = (Hierarchy) LogManager.GetRepository();
//            hierarchy.Root.AddAppender(LogHelper.consoleAppender());
            hierarchy.Root.AddAppender(LogHelper.fileAppender());
            hierarchy.Root.AddAppender(LogHelper.rollingFileAppender());
            hierarchy.Root.AddAppender(LogHelper.udpAppender());

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();

            hierarchy.Root.Level = Level.Debug;
            hierarchy.Configured = true;
        }

        private static PatternLayout patternLayout(String pattern)
        {
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = pattern;
            patternLayout.ActivateOptions();

            return patternLayout;
        }

        private static ConsoleAppender consoleAppender()
        {

            ConsoleAppender console = new ConsoleAppender();
            console.Layout = patternLayout("[%5.level] %date{HH:mm:ss} %message%newline%exception");
            console.ActivateOptions();
            return console;
        }

        private static FileAppender fileAppender()
        {
            FileAppender appender = new FileAppender();
            appender.File = @"Logs\vkapi.log";
            appender.LockingModel = new MinimalLock();
            appender.Layout = patternLayout("[%5.level] %date{HH:mm:ss} %logger: %message%newline%exception");
            appender.AppendToFile = true;
            appender.ActivateOptions();

            return appender;
        }

        private static RollingFileAppender rollingFileAppender()
        {
            RollingFileAppender appender = new RollingFileAppender();
            appender.File = @"Logs\vkapi_roller.log";
            appender.AppendToFile = true;
            appender.RollingStyle = RollingFileAppender.RollingMode.Size;

            appender.MaxFileSize = 10000;
            appender.MaxSizeRollBackups = 20000;

            appender.StaticLogFileName = true;
            appender.Layout = patternLayout("[%5.level][%date{HH:mm:ss}][%logger:%line] - %message%newline%exception");
            
            appender.ActivateOptions();

            return appender;
        }

        private static UdpAppender udpAppender()
        {
            UdpAppender appender = new UdpAppender();
            appender.Encoding = Encoding.UTF8;
            appender.RemoteAddress = IPAddress.Parse("127.0.0.1");
            appender.RemotePort = 8888;
            appender.Layout = patternLayout("[%5.level][%date{HH:mm:ss}][%3.line][%32.logger] - %message%exception");
            appender.ActivateOptions();

            return appender;
        }
    }
}
