using Cardamom.ImageProcessing.Filters;
using OpenTK.Mathematics;
using System.Threading.Channels;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class LatticeNoiseNode : BaseFilterPipelineNode<LatticeNoiseNode.Parameters>
    {
        public class Parameters
        {
            public IParameterValue? Seed { get; set; }
            public IParameterValue? HashSpace { get; set; }
            public IParameterValue? KernelSize { get; set; }
            public IParameterValue? Frequency { get; set; }
            public IParameterValue? Lacunarity { get; set; }
            public IParameterValue? Octaves { get; set; }
            public IParameterValue? Persistence { get; set; }
            public IParameterValue? Bias { get; set; }
            public IParameterValue? Amplitude { get; set; }
            public IParameterValue? Offset { get; set; }
        }

        private readonly Dictionary<string, string> _inputs;
        private readonly Parameters _parameters;

        public LatticeNoiseNode(
            string key,
            Channel channel,
            Dictionary<string, string> inputs,
            Parameters parameters)
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
            var builder = new LatticeNoise.Builder();
            if (_parameters.Seed != null)
            {
                builder.SetSeed((int)_parameters.Seed.Get());
            }
            if (_parameters.HashSpace != null)
            {
                builder.SetHashSpace((int)_parameters.HashSpace.Get());
            }
            if (_parameters.KernelSize != null)
            {
                builder.SetKernelSize((int)_parameters.KernelSize.Get());
            }
            if (_parameters.Frequency != null)
            {
                builder.SetFrequency((float)_parameters.Frequency.Get());
            }
            if (_parameters.Lacunarity != null)
            {
                builder.SetLacunarity((float)_parameters.Lacunarity.Get());
            }
            if (_parameters.Octaves != null)
            {
                builder.SetOctaves((int)_parameters.Octaves.Get());
            }
            if (_parameters.Persistence != null)
            {
                builder.SetPersistence((float)_parameters.Persistence.Get());
            }
            if (_parameters.Bias != null)
            {
                builder.SetBias((float)_parameters.Bias.Get());
            }
            if (_parameters.Amplitude != null)
            {
                builder.SetAmplitude((float)_parameters.Amplitude.Get());
            }
            if (_parameters.Offset != null)
            {
                builder.SetOffset((Vector2)_parameters.Offset.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNode<Parameters>.BaseFilterPipelineNodeBuilder
        {
            public override IPipelineNode Build()
            {
                return new LatticeNoiseNode(Key!, Channel, Inputs, Parameters);
            }
        }
    }
}
