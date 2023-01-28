using Cardamom.ImageProcessing.Filters;
using Cardamom.Utils.Suppliers;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class ClassifyNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public ISupplier<IEnumerable<Classify.Classification>>? Classifications { get; set; }
        }

        public override bool Inline => true;

        private readonly Parameters _parameters;

        public ClassifyNode(
            string key, Channel channel, Dictionary<string, string> inputs, string? output, Parameters parameters)
            : base(key, channel, inputs, output)
        {
            _parameters = parameters;
        }

        public override IFilter BuildFilter()
        {
            var builder = new Classify.Builder();
            if (_parameters.Classifications != null)
            {
                builder.AddAllClassifications(_parameters.Classifications.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new ClassifyNode(Key!, Channel, Inputs, Output, Parameters);
            }
        }
    }
}
