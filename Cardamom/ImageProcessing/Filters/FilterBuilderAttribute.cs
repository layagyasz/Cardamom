namespace Cardamom.ImageProcessing.Filters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FilterBuilderAttribute : Attribute
    {
        public Type Type { get; set; }

        public FilterBuilderAttribute(Type type)
        {
            Type = type;
        }
    }
}
