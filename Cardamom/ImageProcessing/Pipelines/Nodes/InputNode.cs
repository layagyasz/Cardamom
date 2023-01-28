namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class InputNode : IPipelineNode
    {
        public string Key { get; set; }
        public int Index { get; }
        public bool External => true;
        public bool Inline => false;

        private Canvas? _canvas;

        public InputNode(string key, int index)
        {
            Key = key;
            Index = index;
        }

        public Dictionary<string, string> GetInputs()
        {
            return new();
        }

        public string? GetOutput()
        {
            return null;
        }

        public void SetCanvas(Canvas canvas)
        {
            _canvas = canvas;
        }

        public Canvas Run(Canvas? output, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 0);
            return _canvas!;
        }

        public class Builder : IPipelineNode.IBuilder
        {
            public string Key { get; set; } = string.Empty;
            public int Index { get; set; }

            public Builder SetKey(string key)
            {
                Key = key;
                return this;
            }

            public Builder SetIndex(int index)
            {
                Index = index;
                return this;
            }

            public IPipelineNode Build()
            {
                return new InputNode(Key!, Index);
            }
        }
    }
}
