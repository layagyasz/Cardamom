using Cardamom.ImageProcessing.Filters;
using Cardamom.Utils.Suppliers;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class WhiteNoiseNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public ISupplier<Vector4i>? Seed { get; set; }
        }

        public override bool Inline => true;

        private readonly Parameters _parameters;

        public WhiteNoiseNode(
            string key, Channel channel, Dictionary<string, string> inputs, string? output, Parameters parameters)
            : base(key, channel, inputs, output)
        {
            _parameters = parameters;
        }

        public override IFilter BuildFilter()
        {
            var builder = new WhiteNoise.Builder();
            if (_parameters.Seed != null)
            {
                builder.SetSeed(_parameters.Seed.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new WhiteNoiseNode(Key!, Channel, Inputs, Output, Parameters);
            }
        }
    }
}
