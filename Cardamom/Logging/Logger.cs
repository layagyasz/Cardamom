using Cardamom.Trackers;
using System.Diagnostics;

namespace Cardamom.Logging
{
    public class Logger : ILogger
    {
        private readonly ILogBackend _backend;
        private readonly LogLevel _level;

        private readonly LogContext _defaultContext;
        private readonly Dictionary<CompositeKey<Type?, object?>, LogContext> _contexts = new();

        public Logger(ILogBackend backend, LogLevel level)
        {
            _backend = backend;
            _level = level;
            var timer = new Stopwatch();
            timer.Start();
            _defaultContext = new(this, null, null, _level, null, new Accessor<int>(0), null, timer);
        }

        public ILogger AtLevel(LogLevel level)
        {
            return _defaultContext.AtLevel(level);
        }

        public ILogger Every(int calls)
        {
            return _defaultContext.Every(calls);
        }

        public ILogger EverySpan(int millis)
        {
            return _defaultContext.EverySpan(millis);
        }

        public ILogger ForType(Type type)
        {
            return GetContext(type, null);
        }

        public ILogBackend GetBackend()
        {
            return _backend;
        }

        public LogLevel GetLevel()
        {
            return _level;
        }

        public void Log(Func<string> messageFn)
        {
            _defaultContext.Log(messageFn);
        }

        public ILogger With(object key)
        {
            return GetContext(null, key);
        }

        internal LogContext GetContext(Type? type, object? key)
        {
            var k = CompositeKey<Type?, object?>.Create(type, key);
            if (!_contexts.TryGetValue(k, out var context))
            {
                var timer = new Stopwatch();
                timer.Start();
                context = new LogContext(this, type, key, _level, null, new Accessor<int>(0), null, timer);
                _contexts.Add(k, context);
            }
            return context;
        }
    }
}
