using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Common.Logs
{
    public class MemoryLogger
    {
        public string Name { get; set; }
        public MemoryLogQueue Logs { get; set; } = new MemoryLogQueue();

        public void Log(object msg)
        {
            Logs.Enqueue(new MemoryLog { Message = $"{msg}" });
        }

        public static MemoryLogger Create(string name, int maxLength = 100)
        {
            var memoryLogger = new MemoryLogger();
            memoryLogger.Name = name;
            memoryLogger.Logs.MaxLength = maxLength;
            return memoryLogger;
        }
    }

    public class MemoryLogQueue : ConcurrentQueue<MemoryLog>
    {
        public MemoryLogQueue(int maxLength = 100)
        {
            MaxLength = maxLength;
        }

        public int MaxLength { get; set; }

        public new void Enqueue(MemoryLog item)
        {
            if (Count >= MaxLength)
            {
                TryDequeue(out _);
            }
            base.Enqueue(item);
        }
    }
    
    public class MemoryLog
    {
        public DateTimeOffset CreateAt { get; set; } = DateTimeOffset.Now;
        public string Message { get; set; }
    }

    public class MemoryLogFactory
    {
        private MemoryLogFactory() { }

        public static MemoryLogFactory Instance = new MemoryLogFactory();

        public MemoryLogger GetLogger(string name)
        {
            var logger = Loggers.GetBagValue(name, MemoryLogger.Create(name), true);
            return logger;
        }

        public MemoryLogger CreateLogger(string name)
        {
            var logger = new MemoryLogger {Name = name};
            return logger;
        }

        public IDictionary<string, MemoryLogger> Loggers { get; set; } = new ConcurrentDictionary<string, MemoryLogger>(StringComparer.OrdinalIgnoreCase);
    }

    public static class MemoryLogFactoryExtensions
    {
        public static MemoryLogger GetLoggerForType(this MemoryLogFactory factory, Type theType)
        {
            if (theType == null) throw new ArgumentNullException(nameof(theType));
            return factory.GetLogger(theType.FullName);
        }
        public static MemoryLogger GetLoggerForType<T>(this MemoryLogFactory factory)
        {
            return factory.GetLoggerForType(typeof(T));
        }
        public static MemoryLogger GetLoggerForInstance(this MemoryLogFactory factory, object instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            var name = $"{instance.GetType().FullName}[@]{instance.GetHashCode()}";
            return factory.GetLogger(name);
        }
    }
}
