using Cardamom.ImageProcessing.Filters;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class SpherizeNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public IParameterValue? YScale { get; set; }
            public IParameterValue? Radius { get; set; }
        }

        public override bool Inline => true;

        private readonly Dictionary<string, string> _inputs;
        private readonly Parameters _parameters;

        public SpherizeNode(string key, Channel channel, Dictionary<string, string> inputs, Parameters parameters)
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
            var builder = new Spherize.Builder();
            if (_parameters.YScale != null)
            {
                builder.SetYScale((Spherize.YScale)_parameters.YScale.Get());
            }
            if (_parameters.Radius != null)
            {
                builder.SetRadius((float)_parameters.Radius.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new SpherizeNode(Key!, Channel, Inputs, Parameters);
            }
        }
    }
}
