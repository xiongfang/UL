using System;

namespace Core
{
    //日志接口
    public interface ILog
    {
        void LogInfo(string msg, params object[] args);
        void LogWarning(string msg, params object[] args);
        void LogError(string msg, params object[] args);
    }

    //默认日志
    class DefaultLog:ILog
    {
        public void LogInfo(string msg, params object[] args)
        {
            Console.Out.WriteLine(msg, args);
        }
        public void LogWarning(string msg, params object[] args)
        {
            Console.Out.WriteLine(msg, args);
        }
        public void LogError(string msg, params object[] args)
        {
            Console.Error.WriteLine(msg, args);
        }
    }

    //全局日志
    public class Logger
    {
        static ILog Log = new DefaultLog();

        public static void LogInfo(string msg, params object[] args)
        {
            Log.LogInfo(msg, args);
        }
        public static void LogWarning(string msg, params object[] args)
        {
            Log.LogWarning(msg, args);
        }

        public static void LogError(string msg, params object[] args)
        {
            Log.LogError(msg, args);
        }

        public static void SetLogger(ILog logger) { Log = logger; }
    }
}
