namespace Cardamom.ImageProcessing.Pipelines
{
    public class ConstantValue : IParameterValue
    {
        public bool IsExternal => false;
        public object? Value { get; set; }

        public static ConstantValue Create(object value)
        {
            return new ConstantValue() { Value = value };
        }

        public object Get()
        {
            return Value!;
        }
    }
}
