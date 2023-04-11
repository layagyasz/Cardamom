namespace Cardamom.Trackers
{
	public class Pool
	{
		public float Amount { get; private set; }
		public float MaxAmount { get; private set; }
		public float Remaining => MaxAmount - Amount;

		public Pool(float maxAmount, bool startFull = true)
		{
			MaxAmount = maxAmount;
			Amount = startFull ? maxAmount : 0;
		}

		public void Change(float amount)
		{
			Amount = Math.Min(Math.Max(Amount + amount, 0), MaxAmount);
		}

		public void ChangeMax(float amount)
		{
            MaxAmount = Math.Max(0, MaxAmount + amount);
            Amount = Math.Min(Amount, MaxAmount);
        }

		public bool IsEmpty()
		{
			return Amount <= float.Epsilon;
		}

		public bool IsFull()
		{
			return MaxAmount - Amount <= float.Epsilon;
		}

		public void Merge(Pool other)
		{
			ChangeMax(other.MaxAmount);
			Change(other.Amount);
		}

		public float PercentFull()
		{
			return Amount / MaxAmount;
		}
	}
}