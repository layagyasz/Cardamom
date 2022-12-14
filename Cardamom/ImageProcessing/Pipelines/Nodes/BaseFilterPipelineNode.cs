using Cardamom.ImageProcessing.Filters;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public abstract class BaseFilterPipelineNode : IPipelineNode
    {
        public string? Key { get; set; }
        public Channel Channel { get; }

        protected BaseFilterPipelineNode(string key, Channel channel)
        {
            Key = key;
            Channel = channel;
        }

        public abstract Dictionary<string, string> GetInputs();
        public abstract IFilter BuildFilter();

        public Canvas Run(Dictionary<string, Canvas> inputs, ICanvasProvider canvasProvider)
        {
            var filter = BuildFilter();
            var outCanvas = filter.InPlace ? inputs.First().Value : canvasProvider.Get();
            filter.Apply(outCanvas, Channel, inputs);
            return outCanvas;
        }

        public abstract class BaseFilterPipelineNodeBuilder<TParameters>
            : IPipelineNode.IBuilder where TParameters : new()
        {
            public string? Key { get; set; }
            public Channel Channel { get; set; } = Channel.ALL;
            public Dictionary<string, string> Inputs { get; set; } = new();
            public TParameters Parameters { get; set; } = new();

            public BaseFilterPipelineNodeBuilder<TParameters> SetKey(string key)
            {
                Key = key;
                return this;
            }

            public BaseFilterPipelineNodeBuilder<TParameters> SetChannel(Channel channel)
            {
                Channel = channel;
                return this;
            }

            public BaseFilterPipelineNodeBuilder<TParameters> SetInput(string inputName, string stepKey)
            {
                Inputs.Add(inputName, stepKey);
                return this;
            }

            public BaseFilterPipelineNodeBuilder<TParameters> SetParameters(TParameters parameters)
            {
                Parameters = parameters;
                return this;
            }

            public abstract IPipelineNode Build();
        }
    }
}
