using Cardamom.Trackers;
using System.Diagnostics;

namespace Cardamom.Logging
{
    internal class LogContext : ILogger
    {
        private readonly Logger _parent;

        // Uniquely identifies a persisted logging context.
        private readonly Type? _type;
        private readonly object? _key;

        private readonly LogLevel _level;
        private readonly int? _every;
        private readonly Accessor<int> _calls;
        private readonly int? _everySpan;
        private readonly Stopwatch _timer;

        public LogContext(
            Logger parent, 
            Type? type,
            object? key, 
            LogLevel level,
            int? every,
            Accessor<int> calls,
            int? everySpan,
            Stopwatch timer)
        {
            _parent = parent;
            _type = type;
            _key = key;
            _level = level;
            _every = every;
            _calls = calls;
            _everySpan = everySpan;
            _timer = timer;
        }

        public ILogger AtLevel(LogLevel level)
        {
            return new LogContext(_parent, _type, _key, level, _every, _calls, _everySpan, _timer);
        }

        internal ILogger Combine(LogContext other)
        {
            return new LogContext(
                _parent,  _type, _key, other._level, other._every, _calls, other._everySpan, _timer);
        }

        public ILogger Every(int calls)
        {
            return new LogContext(_parent, _type, _key, _level, calls, _calls, _everySpan, _timer);
        }

        public ILogger EverySpan(int millis)
        {
            return new LogContext(_parent, _type, _key, _level, _every, _calls, millis, _timer);
        }

        public ILogger ForType(Type type)
        {
            return _parent.GetContext(type, _key).Combine(this);
        }

        public void Log(Func<string> messageFn)
        {
            if (_level < _parent.GetLevel())
            {
                return;
            }
            ++_calls.Value;
            if (_every != null && _every % _calls.Value != 0)
            {
                return;
            }
            if (_everySpan != null && _timer!.ElapsedMilliseconds < _everySpan)
            {
                return;
            }
            _timer?.Restart();
            string datetime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");
            string type = _type == null ? string.Empty : string.Format($"[{_type.Name}]");
            string key = _key == null ? string.Empty : string.Format($"[{_key}]");
            _parent.GetBackend().Write($"[{datetime}]{type}{key}[{_level}]: {messageFn()}");
        }

        public ILogger With(object key)
        {
            return _parent.GetContext(_type, key).Combine(this);
        }
    }
}
