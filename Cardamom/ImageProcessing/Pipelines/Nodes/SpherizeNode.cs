using Cardamom.ImageProcessing.Filters;
using Cardamom.Utils.Suppliers;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class SpherizeNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public ISupplier<Spherize.YScale>? YScale { get; set; }
            public ISupplier<float>? Radius { get; set; }
        }

        public override bool Inline => true;

        private readonly Parameters _parameters;

        public SpherizeNode(
            string key, Channel channel, Dictionary<string, string> inputs, string? output, Parameters parameters)
            : base(key, channel, inputs, output)
        {
            _parameters = parameters;
        }

        public override IFilter BuildFilter()
        {
            var builder = new Spherize.Builder();
            if (_parameters.YScale != null)
            {
                builder.SetYScale(_parameters.YScale.Get());
            }
            if (_parameters.Radius != null)
            {
                builder.SetRadius(_parameters.Radius.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new SpherizeNode(Key!, Channel, Inputs, Output, Parameters);
            }
        }
    }
}
