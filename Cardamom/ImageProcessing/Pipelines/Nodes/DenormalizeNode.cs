using Cardamom.ImageProcessing.Filters;
using Cardamom.Utils.Suppliers;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class DenormalizeNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public ISupplier<Vector4>? Mean { get; set; }
            public ISupplier<Vector4>? StandardDeviation { get; set; }
        }

        public override bool Inline => true;

        private readonly Parameters _parameters;

        public DenormalizeNode(
            string key, Channel channel, Dictionary<string, string> inputs, string? output, Parameters parameters)
            : base(key, channel, inputs, output)
        {
            _parameters = parameters;
        }

        public override IFilter BuildFilter()
        {
            var builder = new Denormalize.Builder();
            if (_parameters.Mean != null)
            {
                builder.SetMean(_parameters.Mean.Get());
            }
            if (_parameters.StandardDeviation != null)
            {
                builder.SetStandardDeviation(_parameters.StandardDeviation.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new DenormalizeNode(Key!, Channel, Inputs, Output, Parameters);
            }
        }
    }
}
