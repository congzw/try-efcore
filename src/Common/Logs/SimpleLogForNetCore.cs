using Microsoft.Extensions.Logging;

namespace Common.Logs
{
    public class SimpleLogForNetCore : ISimpleLog
    {
        private readonly ILoggerFactory _loggerFactory;

        public SimpleLogForNetCore(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public void Log(string category, object msg, int level)
        {
            if (msg == null)
            {
                return;
            }
            var logger = _loggerFactory.CreateLogger(category);
            logger.Log((LogLevel)level, msg.ToString());
        }
    }
}
