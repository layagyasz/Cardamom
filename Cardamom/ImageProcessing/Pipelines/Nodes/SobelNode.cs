using Cardamom.ImageProcessing.Filters;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class SobelNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public IParameterValue? Roughness { get; set; }
        }

        public override bool Inline => false;

        private readonly Dictionary<string, string> _inputs;
        private readonly Parameters _parameters;

        public SobelNode(string key, Channel channel, Dictionary<string, string> inputs, Parameters parameters)
            : base(key, channel)
        {
            _inputs = inputs;
            _parameters = parameters;
        }

        public override Dictionary<string, string> GetInputs()
        {
            return _inputs;
        }

        public override IFilter BuildFilter()
        {
            var builder = new Sobel.Builder();
            if (_parameters.Roughness != null)
            {
                builder.SetRoughness((float)_parameters.Roughness.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new SobelNode(Key!, Channel, Inputs, Parameters);
            }
        }
    }
}
