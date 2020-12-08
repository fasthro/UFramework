/*
 * @Author: fasthro
 * @Date: 2020-11-25 12:26:03
 * @Description: logger
 */
 
namespace UFramework
{
    public enum LogLevel
    {
        DEBUG,
        INFO,
        WARN,
        ERROR,
        FATAL,
    }

    public static class Logger
    {
        static LogLevel LOG_LEVEL = LogLevel.DEBUG;

        public static void Debug(object message)
        {
            if (CheckLevel(LogLevel.DEBUG))
                UnityEngine.Debug.Log(string.Format("<color=#04fd14>[{0}]</color> {1}", LogLevel.DEBUG.ToString(), message.ToString()));
        }

        public static void Info(object message)
        {
            if (CheckLevel(LogLevel.INFO))
                UnityEngine.Debug.Log(string.Format("<color=#49c9c1>[{0}]</color> {1}", LogLevel.INFO.ToString(), message.ToString()));
        }

        public static void Error(object message)
        {
            if (CheckLevel(LogLevel.ERROR))
                UnityEngine.Debug.Log(string.Format("<color=#ff0417>[{0}]</color> {1}", LogLevel.ERROR.ToString(), message.ToString()));
        }

        public static void Wraning(object message)
        {
            if (CheckLevel(LogLevel.WARN))
                UnityEngine.Debug.Log(string.Format("<color=#ebff28>[{0}]</color> {1}", LogLevel.WARN.ToString(), message.ToString()));
        }

        static bool CheckLevel(LogLevel logLevel)
        {
            return (int)logLevel <= (int)LOG_LEVEL;
        }
    }
}