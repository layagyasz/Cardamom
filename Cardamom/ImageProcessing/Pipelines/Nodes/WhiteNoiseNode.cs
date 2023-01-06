using Cardamom.ImageProcessing.Filters;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class WhiteNoiseNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public IParameterValue? Seed { get; set; }
        }

        public override bool Inline => true;

        private readonly Dictionary<string, string> _inputs;
        private readonly Parameters _parameters;

        public WhiteNoiseNode(string key, Channel channel, Dictionary<string, string> inputs, Parameters parameters)
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
            var builder = new WhiteNoise.Builder();
            if (_parameters.Seed != null)
            {
                builder.SetSeed((Vector4i)_parameters.Seed.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new WhiteNoiseNode(Key!, Channel, Inputs, Parameters);
            }
        }
    }
}
