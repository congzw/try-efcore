using System;

namespace Common.Logs
{
    public static class SimpleLogExtensions
    {
        public static void Info(this SimpleLogger logger, object msg)
        {
            //default as 2: Info
            logger.Log(msg, 2);
        }
        public static void Debug(this SimpleLogger logger, object msg)
        {
            //default as 1: Debug
            logger.Log(msg, 1);
        }
        public static void Log(this ISimpleLog simpleLog, string category, object msg)
        {
            //default as 1: Debug
            simpleLog.Log(category, msg, 1);
        }
        public static SimpleLogger GetLogger(this ISimpleLog simpleLog, string category)
        {
            return new SimpleLogger(category, simpleLog);
        }
        public static SimpleLogger GetLogger(this ISimpleLog simpleLog, Type theType)
        {
            return simpleLog.GetLogger(theType.FullName);
        }
        public static SimpleLogger GetLogger<T>(this ISimpleLog simpleLog)
        {
            return simpleLog.GetLogger(typeof(T));
        }
    }
}