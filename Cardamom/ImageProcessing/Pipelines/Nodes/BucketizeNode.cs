using Cardamom.ImageProcessing.Filters;
using Cardamom.Utils.Suppliers;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class BucketizeNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public ISupplier<IEnumerable<Bucketize.Bucket>>? Buckets { get; set; }
        }

        public override bool Inline => true;

        private readonly Parameters _parameters;

        public BucketizeNode(
            string key, Channel channel, Dictionary<string, string> inputs, string? output, Parameters parameters)
            : base(key, channel, inputs, output)
        {
            _parameters = parameters;
        }

        public override IFilter BuildFilter()
        {
            var builder = new Bucketize.Builder();
            if (_parameters.Buckets != null)
            {
                builder.AddAllBuckets(_parameters.Buckets.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new BucketizeNode(Key!, Channel, Inputs, Output, Parameters);
            }
        }
    }
}
