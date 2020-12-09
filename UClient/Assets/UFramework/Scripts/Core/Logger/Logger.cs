/*
 * @Author: fasthro
 * @Date: 2020-11-25 12:26:03
 * @Description: logger
 */

namespace UFramework
{
    public enum LogLevel
    {
        Debug = 0,
        Info,
        Warn,
        Error,
        Fatal,
    }

    public class Logger
    {
        readonly static int LEVEL_DEBUG = (int)LogLevel.Debug;
        readonly static int LEVEL_INFO = (int)LogLevel.Info;
        readonly static int LEVEL_WARN = (int)LogLevel.Warn;
        readonly static int LEVEL_ERROR = (int)LogLevel.Error;
        readonly static int LEVEL_FATAL = (int)LogLevel.Fatal;

        readonly static string[] LOG_LEVEL_TITLES = new string[] { "DEBUG", "INFO", "WARN", "ERROR", "FATAL" };

        static int CUR_LEVEL = LEVEL_DEBUG;

        public static void Debug(object message)
        {
            if (CheckLevel(LEVEL_DEBUG))
                UnityEngine.Debug.Log(string.Format("<color=#04fd14>[{0}]</color> {1}", LOG_LEVEL_TITLES[LEVEL_DEBUG], message.ToString()));
        }

        public static void Info(object message)
        {
            if (CheckLevel(LEVEL_INFO))
                UnityEngine.Debug.Log(string.Format("<color=#49c9c1>[{0}]</color> {1}", LOG_LEVEL_TITLES[LEVEL_INFO], message.ToString()));
        }

        public static void Error(object message)
        {
            if (CheckLevel(LEVEL_ERROR))
                UnityEngine.Debug.Log(string.Format("<color=#ff0417>[{0}]</color> {1}", LOG_LEVEL_TITLES[LEVEL_ERROR], message.ToString()));
        }

        public static void Wraning(object message)
        {
            if (CheckLevel(LEVEL_WARN))
                UnityEngine.Debug.Log(string.Format("<color=#ebff28>[{0}]</color> {1}", LOG_LEVEL_TITLES[LEVEL_WARN], message.ToString()));
        }

        public static void SetLevel(LogLevel logLevel)
        {
            CUR_LEVEL = (int)logLevel;
        }

        public static int GetLevel()
        {
            return CUR_LEVEL;
        }

        static bool CheckLevel(int level)
        {
            return level >= CUR_LEVEL;
        }
    }
}