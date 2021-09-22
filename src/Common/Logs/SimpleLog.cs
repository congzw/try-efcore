using System.Diagnostics;
using Common.Utilities;

namespace Common.Logs
{
    public interface ISimpleLog
    {
        void Log(string category, object msg, int level);
    }

    public class SimpleLog : ISimpleLog
    {
        #region for di extensions

        [LazySingleton]
        public static ISimpleLog Instance => LazySingleton.Instance.Resolve<ISimpleLog>(() => new SimpleLog());

        #endregion

        public void Log(string category, object msg, int level)
        {
            Trace.WriteLine($"{category} => {msg}");
        }
    }
}