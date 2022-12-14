namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class GeneratorNode : IPipelineNode
    {
        public string? Key { get; set; }

        public GeneratorNode(string? key)
        {
            Key = key;
        }

        public Dictionary<string, string> GetInputs()
        {
            return new();
        }

        public Canvas Run(Dictionary<string, Canvas> inputs, ICanvasProvider canvasProvider)
        {
            return canvasProvider.Get();
        }

        public class Builder : IPipelineNode.IBuilder
        {
            public string? Key { get; set; }

            public Builder SetKey(string key)
            {
                Key = key;
                return this;
            }

            public IPipelineNode Build()
            {
                return new GeneratorNode(Key!);
            }
        }
    }
}
