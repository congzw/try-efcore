namespace Common.Logs
{
    public class SimpleLogger
    {
        public string Category { get; }
        private readonly ISimpleLog _simpleLog;

        public SimpleLogger(string category, ISimpleLog simpleLog)
        {
            Category = category;
            _simpleLog = simpleLog;
        }

        public void Log(object msg, int level)
        {
            _simpleLog.Log(Category, msg, level);
        }
    }
}