/*
 * @Author: fasthro
 * @Date: 2020/12/29 17:03:13
 * @Description:
 */

namespace Lockstep
{
    public static class Logger
    {
        public static ILogger logger;

        public static void Debug(object message)
        {
            logger?.Debug(message);
        }

        public static void Error(object message)
        {
            logger?.Error(message);
        }

        public static void Info(object message)
        {
            logger?.Info(message);
        }

        public static void Warning(object message)
        {
            logger?.Warning(message);
        }
    }
}