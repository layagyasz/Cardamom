namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public interface IPipelineNode : IKeyed
    {
        bool External { get; }
        bool Inline { get; }

        Dictionary<string, string> GetInputs();
        Canvas Run(Canvas? output, Dictionary<string, Canvas> inputs);

        public interface IBuilder : IKeyed
        {
            IPipelineNode Build();
        }
    }
}
