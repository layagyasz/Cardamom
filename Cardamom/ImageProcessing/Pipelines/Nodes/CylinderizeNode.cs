using Cardamom.ImageProcessing.Filters;
using Cardamom.Utils.Suppliers;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class CylinderizeNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public ISupplier<Vector2>? YRange { get; set; }
            public ISupplier<float>? Radius { get; set; }
        }

        public override bool Inline => true;

        private readonly Dictionary<string, string> _inputs;
        private readonly Parameters _parameters;

        public CylinderizeNode(string key, Channel channel, Dictionary<string, string> inputs, Parameters parameters)
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
            var builder = new Cylinderize.Builder();
            if (_parameters.YRange != null)
            {
                builder.SetYRange(_parameters.YRange.Get());
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
                return new CylinderizeNode(Key!, Channel, Inputs, Parameters);
            }
        }
    }
}
