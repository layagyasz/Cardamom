using System.Diagnostics;

namespace Cardamom.Graphing.BehaviorTree
{
    public class TimedSupplier<T> : ISupplier<T>
    {
        private readonly T _value;
        private readonly Stopwatch _timer;
        private readonly int _delayMilliseconds;

        public TimedSupplier(T value, int delayMilliseconds)
        {
            _value = value;
            _timer = new Stopwatch();
            _timer.Start();
            _delayMilliseconds = delayMilliseconds;
        }

        public T Get()
        {
            if (_timer.ElapsedMilliseconds < _delayMilliseconds)
            {
                Thread.Sleep((int)(_delayMilliseconds - _timer.ElapsedMilliseconds));
            }
            _timer.Restart();
            return _value;
        }
    }
}