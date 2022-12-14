using Cardamom.ImageProcessing.Filters;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class WaveFormNode : BaseFilterPipelineNode<WaveFormNode.Parameters>
    {
        public class Parameters
        {
            public IParameterValue? WaveType { get; set; }
            public IParameterValue? Amplitude { get; set; }
            public IParameterValue? Bias { get; set; }
            public IParameterValue? Periodicity { get; set; }
            public IParameterValue? Turbulence { get; set; }
        }

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
                builder.SetWaveType((WaveForm.WaveType)_parameters.WaveType.Get());
            }
            if (_parameters.Amplitude != null)
            {
                builder.SetAmplitude((float)_parameters.Amplitude.Get());
            }
            if (_parameters.Bias != null)
            {
                builder.SetBias((float)_parameters.Bias.Get());
            }
            if (_parameters.Periodicity != null)
            {
                builder.SetPeriodicity((Vector2)_parameters.Periodicity.Get());
            }
            if (_parameters.Turbulence != null)
            {
                builder.SetTurbulence((Vector2)_parameters.Turbulence.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNode<Parameters>.BaseFilterPipelineNodeBuilder
        {
            public override IPipelineNode Build()
            {
                return new WaveFormNode(Key!, Channel, Inputs, Parameters);
            }
        }
    }
}
