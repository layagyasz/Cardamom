namespace Cardamom.Trackers
{
	public class Pool
	{
		public double Amount { get; private set; }
		public double MaxAmount { get; }

		public Pool(double maxAmount)
		{
			MaxAmount = maxAmount;
			Amount = maxAmount;
		}

		public void Change(double amount)
		{
			Amount = Math.Min(Math.Max(Amount + amount, 0), MaxAmount);
		}

		public bool IsEmpty()
		{
			return Amount <= double.Epsilon;
		}

		public bool IsFull()
		{
			return MaxAmount - Amount <= double.Epsilon;
		}

		public double PercentFull()
		{
			return Amount / MaxAmount;
		}
	}
}