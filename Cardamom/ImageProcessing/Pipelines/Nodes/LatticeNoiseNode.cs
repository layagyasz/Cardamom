using Cardamom.ImageProcessing.Filters;
using OpenTK.Mathematics;
using System.Threading.Channels;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class LatticeNoiseNode : BaseFilterPipelineNode
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
            public IParameterValue? Amplitude { get; set; }
            public IParameterValue? Evaluator { get; set; }
            public IParameterValue? Interpolator { get; set; }
            public IParameterValue? PreTreatment { get; set; }
            public IParameterValue? PostTreatment { get; set; }
        }

        public override bool Inline => true;

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
            if (_parameters.Amplitude != null)
            {
                builder.SetAmplitude((float)_parameters.Amplitude.Get());
            }
            if (_parameters.Evaluator != null)
            {
                builder.SetEvaluator((LatticeNoise.Evaluator)_parameters.Evaluator.Get());
            }
            if (_parameters.Interpolator != null)
            {
                builder.SetInterpolator((LatticeNoise.Interpolator)_parameters.Interpolator.Get());
            }
            if (_parameters.PreTreatment != null)
            {
                builder.SetPreTreatment((LatticeNoise.Treatment)_parameters.PreTreatment.Get());
            }
            if (_parameters.PostTreatment != null)
            {
                builder.SetPostTreatment((LatticeNoise.Treatment)_parameters.PostTreatment.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new LatticeNoiseNode(Key!, Channel, Inputs, Parameters);
            }
        }
    }
}
