using Cardamom.ImageProcessing.Filters;
using Cardamom.Mathematics;
using Cardamom.Utils.Suppliers;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Pipelines.Nodes
{
    public class SpotNoiseNode : BaseFilterPipelineNode
    {
        public class Parameters
        {
            public ISupplier<int>? Seed { get; set; }
            public ISupplier<float>? Frequency { get; set; }
            public ISupplier<float>? Lacunarity { get; set; }
            public ISupplier<int>? Octaves { get; set; }
            public ISupplier<float>? Persistence { get; set; }
            public ISupplier<float>? Amplitude { get; set; }
            public ISupplier<IntInterval>? Density { get; set; }
        }

        public override bool Inline => true;

        private readonly Parameters _parameters;

        public SpotNoiseNode(
            string key, Channel channel, Dictionary<string, string> inputs, string? output, Parameters parameters)
            : base(key, channel, inputs, output)
        {
            _parameters = parameters;
        }

        public override IFilter BuildFilter()
        {
            var builder = new SpotNoise.Builder();
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
            if (_parameters.Density != null)
            {
                builder.SetDensity(_parameters.Density.Get());
            }
            return builder.Build();
        }

        public class Builder : BaseFilterPipelineNodeBuilder<Parameters>
        {
            public override IPipelineNode Build()
            {
                return new SpotNoiseNode(Key!, Channel, Inputs, Output, Parameters);
            }
        }
    }
}
