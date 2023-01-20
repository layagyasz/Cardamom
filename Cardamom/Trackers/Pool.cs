namespace Cardamom.Trackers
{
	public class Pool
	{
		public float Amount { get; private set; }
		public float MaxAmount { get; }

		public Pool(float maxAmount)
		{
			MaxAmount = maxAmount;
			Amount = maxAmount;
		}

		public void Change(float amount)
		{
			Amount = Math.Min(Math.Max(Amount + amount, 0), MaxAmount);
		}

		public bool IsEmpty()
		{
			return Amount <= float.Epsilon;
		}

		public bool IsFull()
		{
			return MaxAmount - Amount <= float.Epsilon;
		}

		public float PercentFull()
		{
			return Amount / MaxAmount;
		}
	}
}