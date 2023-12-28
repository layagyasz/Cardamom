namespace Cardamom.Trackers
{
	public class Pool : IPool
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
			if (Amount < float.Epsilon || MaxAmount < float.Epsilon)
			{
				return 0;
			}
			return Amount / MaxAmount;
		}

		public void Set(float amount)
		{
			Amount = Math.Min(amount, MaxAmount);
		}

		public void SetMax(float amount)
		{
			MaxAmount = amount;
			Amount = Math.Min(Amount, MaxAmount);
		}

		public string ToString(string format)
		{
			return $"{Amount.ToString(format)}/{MaxAmount.ToString(format)}";
		}
	}
}