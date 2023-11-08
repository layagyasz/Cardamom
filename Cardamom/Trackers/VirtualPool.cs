namespace Cardamom.Trackers
{
    public class VirtualPool : IPool
    {
        private readonly Func<float> _totalFn;
        private readonly Func<float> _totalMaxFn;

        public VirtualPool(Func<float> totalFn, Func<float> totalMaxFn)
        {
            _totalFn = totalFn;
            _totalMaxFn = totalMaxFn;
        }

        public bool IsEmpty()
        {
            return _totalFn() < float.Epsilon;
        }

        public float PercentFull()
        {
            return _totalFn() / _totalMaxFn();
        }

        public string ToString(string format)
        {
            return $"{_totalFn().ToString(format)}/{_totalMaxFn().ToString(format)}";
        }
    }
}
