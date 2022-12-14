using Cardamom.ImageProcessing.Filters;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class DenormalizeNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public IParameterValue? Mean { get; set; }
            public IParameterValue? StandardDeviation { get; set; }
        }

        private readonly Dictionary<string, string> _inputs;
        private readonly Parameters _parameters;

        public DenormalizeNode(string key, Channel channel, Dictionary<string, string> inputs, Parameters parameters)
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
            var builder = new Denormalize.Builder();
            if (_parameters.Mean != null)
            {
                builder.SetMean((Vector4)_parameters.Mean.Get());
            }
            if (_parameters.StandardDeviation != null)
            {
                builder.SetMean((Vector4)_parameters.StandardDeviation.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new DenormalizeNode(Key!, Channel, Inputs, Parameters);
            }
        }
    }
}
