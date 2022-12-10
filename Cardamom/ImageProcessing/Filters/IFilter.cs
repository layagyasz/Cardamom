namespace Cardamom.ImageProcessing.Filters
{
    public interface IFilter
    {
        public interface IFilterBuilder
        {
            IFilter Build();
        }

        public bool InPlace { get; }
        void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs);
    }
}
