using Cardamom.ImageProcessing.Filters;
using Cardamom.Json;
using System.Text.Json.Serialization;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public abstract class BaseFilterPipelineNode : IPipelineNode
    {
        public string Key { get; set; }
        public Channel Channel { get; }
        public bool External => false;
        public abstract bool Inline { get; }

        private readonly Dictionary<string, string> _inputs;
        private readonly string? _output;

        protected BaseFilterPipelineNode(
            string key, Channel channel, Dictionary<string, string> inputs, string? output)
        {
            Key = key;
            Channel = channel;
            _inputs = inputs;
            _output = output;
        }

        public Dictionary<string, string> GetInputs()
        {
            return _inputs;
        }

        public string? GetOutput()
        {
            return _output;
        }

        public abstract IFilter BuildFilter();

        public Canvas Run(Canvas? output, Dictionary<string, Canvas> inputs)
        {
            BuildFilter().Apply(output!, Channel, inputs);
            return output!;
        }

        public abstract class BaseFilterPipelineNodeBuilder<TParameters>
            : IPipelineNode.IBuilder where TParameters : new()
        {
            public string Key { get; set; } = string.Empty;
            [JsonConverter(typeof(FlagJsonConverter<Channel>))]
            public Channel Channel { get; set; } = Channel.All;
            public Dictionary<string, string> Inputs { get; set; } = new();
            public string? Output { get; set; }
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

            public BaseFilterPipelineNodeBuilder<TParameters> SetOutput(string stepKey)
            {
                Output = stepKey; 
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
