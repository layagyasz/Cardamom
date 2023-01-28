using Cardamom.ImageProcessing.Filters;
using Cardamom.Utils.Suppliers;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class SobelNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public ISupplier<float>? Roughness { get; set; }
        }

        public override bool Inline => false;

        private readonly Parameters _parameters;

        public SobelNode(
            string key, Channel channel, Dictionary<string, string> inputs, string? output, Parameters parameters)
            : base(key, channel, inputs, output)
        {
            _parameters = parameters;
        }

        public override IFilter BuildFilter()
        {
            var builder = new Sobel.Builder();
            if (_parameters.Roughness != null)
            {
                builder.SetRoughness(_parameters.Roughness.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new SobelNode(Key!, Channel, Inputs, Output, Parameters);
            }
        }
    }
}
