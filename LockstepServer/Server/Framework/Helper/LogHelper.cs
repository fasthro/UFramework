// --------------------------------------------------------------------------------
// * @Author: fasthro
// * @Date: 2020/12/18 14:44:00
// * @Description:
// --------------------------------------------------------------------------------

using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;

namespace UFramework
{
    public static class LogHelper
    {
        private static ILog logger = null;
        private const bool isConsole = true;
        public static ILoggerRepository repository { get; set; }

        public static void Initialize()
        {
            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            logger = LogManager.GetLogger(repository.Name, typeof(Launcher));
        }

        public static void Debug(object message)
        {
            if (isConsole)
            {
                Console.WriteLine("[DEBUG] " + message.ToString());
            }
            else
            {
                logger.Debug(message);
            }
        }

        public static void Info(object message)
        {
            if (isConsole)
            {
                Console.WriteLine("[INFO] " + message.ToString());
            }
            else
            {
                logger.Info(message);
            }
        }

        public static void Error(object message)
        {
            if (isConsole)
            {
                Console.WriteLine("[ERROR] " + message.ToString());
            }
            else
            {
                logger.Error(message);
            }
        }

        public static void Warn(object message)
        {
            if (isConsole)
            {
                Console.WriteLine("[WARN] " + message.ToString());
            }
            else
            {
                logger.Warn(message);
            }
        }
    }
}