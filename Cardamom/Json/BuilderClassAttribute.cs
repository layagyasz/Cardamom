namespace Cardamom.Json
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BuilderClassAttribute : Attribute
    {
        public Type BuilderType { get; }

        public BuilderClassAttribute(Type builderType)
        {
            BuilderType = builderType;
        }
    }
}
