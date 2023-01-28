namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class GeneratorNode : IPipelineNode
    {
        public string Key { get; set; }
        public bool External => false;
        public bool Inline => false;

        public GeneratorNode(string key)
        {
            Key = key;
        }

        public Dictionary<string, string> GetInputs()
        {
            return new();
        }

        public string? GetOutput()
        {
            return null;
        }

        public Canvas Run(Canvas? output, Dictionary<string, Canvas> inputs) 
        {
            Precondition.Check(inputs.Count == 0);
            return output!;
        }

        public class Builder : IPipelineNode.IBuilder
        {
            public string Key { get; set; } = string.Empty;

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
