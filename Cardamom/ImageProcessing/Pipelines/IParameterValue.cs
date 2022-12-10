namespace Cardamom.ImageProcessing.Pipelines
{
    public interface IParameterValue
    {
        bool IsExternal { get; }
        object Get();
    }
}
