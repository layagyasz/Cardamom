using Cardamom.ImageProcessing.Filters;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class AdjustNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public IParameterValue? OverflowBehavior { get; set; }
            public IParameterValue? Adjustment { get; set; }
        }

        private readonly Dictionary<string, string> _inputs;
        private readonly Parameters _parameters;

        public AdjustNode(string key, Channel channel, Dictionary<string, string> inputs, Parameters parameters)
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
            var builder = new Adjust.Builder();
            if (_parameters.OverflowBehavior != null)
            {
                builder.SetOverflowBehavior((Adjust.OverflowBehavior)_parameters.OverflowBehavior.Get());
            }
            if (_parameters.Adjustment != null)
            {
                builder.SetAdjustment((Vector4)_parameters.Adjustment.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new AdjustNode(Key!, Channel, Inputs, Parameters);
            }
        }
    }
}
