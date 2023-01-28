using Cardamom.ImageProcessing.Filters;
using Cardamom.Utils.Suppliers;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class AdjustNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public ISupplier<OverflowBehavior>? OverflowBehavior { get; set; }
            public ISupplier<Matrix4>? Gradient { get; set; }
            public ISupplier<Vector4>? Bias { get; set; }
        }

        public override bool Inline => true;

        private readonly Parameters _parameters;

        public AdjustNode(
            string key, Channel channel, Dictionary<string, string> inputs, string? output, Parameters parameters)
            : base(key, channel, inputs, output)
        {
            _parameters = parameters;
        }

        public override IFilter BuildFilter()
        {
            var builder = new Adjust.Builder();
            if (_parameters.OverflowBehavior != null)
            {
                builder.SetOverflowBehavior(_parameters.OverflowBehavior.Get());
            }
            if (_parameters.Gradient != null)
            {
                builder.SetGradient(_parameters.Gradient.Get());
            }
            if (_parameters.Bias != null)
            {
                builder.SetBias(_parameters.Bias.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new AdjustNode(Key!, Channel, Inputs, Output, Parameters);
            }
        }
    }
}
