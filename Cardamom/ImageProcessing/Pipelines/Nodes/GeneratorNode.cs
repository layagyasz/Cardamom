namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class GeneratorNode : IPipelineNode
    {
        public string? Key { get; set; }
        public bool Inline => false;

        public GeneratorNode(string? key)
        {
            Key = key;
        }

        public Dictionary<string, string> GetInputs()
        {
            return new();
        }

        public void Run(Canvas output, Dictionary<string, Canvas> inputs) 
        {
            Precondition.Check(inputs.Count == 0);
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
