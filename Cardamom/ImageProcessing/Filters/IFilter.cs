namespace Cardamom.ImageProcessing.Filters
{
    public interface IFilter
    {
        public interface IFilterBuilder
        {
            IFilter Build();
        }
        void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs);
    }
}
