﻿namespace Cardamom.Trackers
{
    public class IntPool : IPool
    {
        public int Amount { get; private set; }
        public int MaxAmount { get; private set; }
        public int Remaining => MaxAmount - Amount;

        public IntPool(int maxAmount, bool startFull = true)
        {
            MaxAmount = maxAmount;
            Amount = startFull ? maxAmount : 0;
        }

        public void Change(int amount)
        {
            Amount = Math.Min(Math.Max(Amount + amount, 0), MaxAmount);
        }

        public void ChangeMax(int amount)
        {
            MaxAmount = Math.Max(0, MaxAmount + amount);
            Amount = Math.Min(Amount, MaxAmount);
        }

        public bool IsEmpty()
        {
            return Amount == 0;
        }

        public bool IsFull()
        {
            return Amount == MaxAmount;
        }

        public void Merge(IntPool other)
        {
            ChangeMax(other.MaxAmount);
            Change(other.Amount);
        }

        public float PercentFull()
        {
            return 1f * Amount / MaxAmount;
        }

        public string ToString(string format)
        {
            return $"{Amount.ToString(format)}/{MaxAmount.ToString(format)}";
        }
    }
}
