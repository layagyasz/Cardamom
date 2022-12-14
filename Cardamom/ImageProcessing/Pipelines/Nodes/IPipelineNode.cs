namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public interface IPipelineNode : IKeyed
    {
        Dictionary<string, string> GetInputs();
        Canvas Run(Dictionary<string, Canvas> inputs, ICanvasProvider canvasProvider);

        public interface IBuilder : IKeyed
        {
            IPipelineNode Build();
        }
    }
}
