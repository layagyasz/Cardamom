using Cardamom.ImageProcessing.Filters;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class CombineNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public IParameterValue? OverflowBehavior { get; set; }
            public IParameterValue? LeftFactor { get; set; }
            public IParameterValue? RightFactor { get; set; }
            public IParameterValue? Bias { get; set; }
        }

        public override bool Inline => true;

        private readonly Dictionary<string, string> _inputs;
        private readonly Parameters _parameters;

        public CombineNode(string key, Channel channel, Dictionary<string, string> inputs, Parameters parameters)
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
            var builder = new Combine.Builder();
            if (_parameters.OverflowBehavior != null)
            {
                builder.SetOverflowBehavior((OverflowBehavior)_parameters.OverflowBehavior.Get());
            }
            if (_parameters.LeftFactor != null)
            {
                builder.SetLeftFactor((Vector4)_parameters.LeftFactor.Get());
            }
            if (_parameters.RightFactor != null)
            {
                builder.SetRightFactor((Vector4)_parameters.RightFactor.Get());
            }
            if (_parameters.Bias != null)
            {
                builder.SetBias((Vector4)_parameters.Bias.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new CombineNode(Key!, Channel, Inputs, Parameters);
            }
        }
    }
}
