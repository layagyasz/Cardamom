using Cardamom.ImageProcessing.Filters;
using Cardamom.Utils.Suppliers;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class LatticeNoiseNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public ISupplier<int>? Seed { get; set; }
            public ISupplier<float>? Frequency { get; set; }
            public ISupplier<float>? Lacunarity { get; set; }
            public ISupplier<int>? Octaves { get; set; }
            public ISupplier<float>? Persistence { get; set; }
            public ISupplier<float>? Amplitude { get; set; }
            public ISupplier<LatticeNoise.Evaluator>? Evaluator { get; set; }
            public ISupplier<LatticeNoise.Interpolator>? Interpolator { get; set; }
            public ISupplier<LatticeNoise.Treatment>? PreTreatment { get; set; }
            public ISupplier<LatticeNoise.Treatment>? PostTreatment { get; set; }
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
                builder.SetSeed(_parameters.Seed.Get());
            }
            if (_parameters.Frequency != null)
            {
                builder.SetFrequency(_parameters.Frequency.Get());
            }
            if (_parameters.Lacunarity != null)
            {
                builder.SetLacunarity(_parameters.Lacunarity.Get());
            }
            if (_parameters.Octaves != null)
            {
                builder.SetOctaves(_parameters.Octaves.Get());
            }
            if (_parameters.Persistence != null)
            {
                builder.SetPersistence(_parameters.Persistence.Get());
            }
            if (_parameters.Amplitude != null)
            {
                builder.SetAmplitude(_parameters.Amplitude.Get());
            }
            if (_parameters.Evaluator != null)
            {
                builder.SetEvaluator(_parameters.Evaluator.Get());
            }
            if (_parameters.Interpolator != null)
            {
                builder.SetInterpolator(_parameters.Interpolator.Get());
            }
            if (_parameters.PreTreatment != null)
            {
                builder.SetPreTreatment(_parameters.PreTreatment.Get());
            }
            if (_parameters.PostTreatment != null)
            {
                builder.SetPostTreatment(_parameters.PostTreatment.Get());
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
