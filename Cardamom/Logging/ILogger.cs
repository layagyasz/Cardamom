namespace Cardamom.Logging
{
    public interface ILogger
    {
        ILogger AtLevel(LogLevel level);
        ILogger Every(int calls);
        ILogger EverySpan(int millis);
        ILogger ForType(Type type);
        void Log(Func<string> messageFn);
        ILogger With(object key);

        public ILogger AtInfo()
        {
            return AtLevel(LogLevel.Info);
        }

        public ILogger AtWarning()
        {
            return AtLevel(LogLevel.Warning);
        }

        public ILogger AtError()
        {
            return AtLevel(LogLevel.Error);
        }

        public ILogger EverySeconds(int seconds)
        {
            return EverySpan((int)TimeSpan.FromSeconds(seconds).TotalMilliseconds);
        }

        public ILogger EveryMinutes(int minutes)
        {
            return EverySpan((int)TimeSpan.FromMinutes(minutes).TotalMilliseconds);
        }

        public ILogger EveryHours(int hours)
        {
            return EverySpan((int)TimeSpan.FromHours(hours).TotalMilliseconds);
        }

        public void Log(string message)
        {
            Log(() => message);
        }
    }
}
