using Cardamom.ImageProcessing.Filters;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class GradientNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public IParameterValue? Factor { get; set; }
            public IParameterValue? Bias { get; set; }
            public IParameterValue? Scale { get; set; }
            public IParameterValue? Offset { get; set; }
            public IParameterValue? OverflowBehavior { get; set; }
        }

        public override bool Inline => true;

        private readonly Dictionary<string, string> _inputs;
        private readonly Parameters _parameters;

        public GradientNode(string key, Channel channel, Dictionary<string, string> inputs, Parameters parameters)
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
            var builder = new Gradient.Builder();
            if (_parameters.Factor != null)
            {
                builder.SetFactor((Matrix4x2)_parameters.Factor.Get());
            }
            if (_parameters.Bias != null)
            {
                builder.SetBias((Vector4)_parameters.Bias.Get()) ;
            }
            if (_parameters.Scale != null)
            {
                builder.SetScale((Vector2)_parameters.Scale.Get());
            }
            if (_parameters.Offset != null)
            {
                builder.SetOffset((Vector2)_parameters.Offset.Get());
            }
            if (_parameters.OverflowBehavior != null)
            {
                builder.SetOverflowBehavior((OverflowBehavior)_parameters.OverflowBehavior.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new GradientNode(Key!, Channel, Inputs, Parameters);
            }
        }
    }
}
