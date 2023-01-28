using Cardamom.ImageProcessing.Filters;
using Cardamom.Utils.Suppliers;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class GradientNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public ISupplier<Matrix4x2>? Gradient { get; set; }
            public ISupplier<Vector4>? Bias { get; set; }
            public ISupplier<Vector2>? Scale { get; set; }
            public ISupplier<Vector2>? Offset { get; set; }
            public ISupplier<OverflowBehavior>? OverflowBehavior { get; set; }
        }

        public override bool Inline => true;

        private readonly Parameters _parameters;

        public GradientNode(
            string key, Channel channel, Dictionary<string, string> inputs, string? output, Parameters parameters)
            : base(key, channel, inputs, output)
        {
            _parameters = parameters;
        }

        public override IFilter BuildFilter()
        {
            var builder = new Gradient.Builder();
            if (_parameters.Gradient != null)
            {
                builder.SetGradient(_parameters.Gradient.Get());
            }
            if (_parameters.Bias != null)
            {
                builder.SetBias(_parameters.Bias.Get()) ;
            }
            if (_parameters.Scale != null)
            {
                builder.SetScale(_parameters.Scale.Get());
            }
            if (_parameters.Offset != null)
            {
                builder.SetOffset(_parameters.Offset.Get());
            }
            if (_parameters.OverflowBehavior != null)
            {
                builder.SetOverflowBehavior(_parameters.OverflowBehavior.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new GradientNode(Key!, Channel, Inputs, Output, Parameters);
            }
        }
    }
}
