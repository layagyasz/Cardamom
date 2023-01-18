﻿using Cardamom.ImageProcessing.Filters;
using Cardamom.Utils.Suppliers;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class WaveFormNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public ISupplier<WaveForm.WaveType>? WaveType { get; set; }
            public ISupplier<Matrix4>? Frequency { get; set; }
        }

        public override bool Inline => true;

        private readonly Dictionary<string, string> _inputs;
        private readonly Parameters _parameters;

        public WaveFormNode(string key, Channel channel, Dictionary<string, string> inputs, Parameters parameters)
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
            var builder = new WaveForm.Builder();
            if (_parameters.WaveType != null)
            {
                builder.SetWaveType(_parameters.WaveType.Get());
            }
            if (_parameters.Frequency != null)
            {
                builder.SetFrequency(_parameters.Frequency.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new WaveFormNode(Key!, Channel, Inputs, Parameters);
            }
        }
    }
}
