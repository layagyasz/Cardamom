namespace Cardamom.Trackers
{
    public class IntPool
    {
        public int Amount { get; private set; }
        public int MaxAmount { get; }

        public IntPool(int maxAmount)
        {
            MaxAmount = maxAmount;
            Amount = maxAmount;
        }

        public void Change(int amount)
        {
            Amount = Math.Min(Math.Max(Amount + amount, 0), MaxAmount);
        }

        public bool IsEmpty()
        {
            return Amount == 0;
        }

        public bool IsFull()
        {
            return Amount == MaxAmount;
        }

        public float PercentFull()
        {
            return 1f * Amount / MaxAmount;
        }
    }
}
