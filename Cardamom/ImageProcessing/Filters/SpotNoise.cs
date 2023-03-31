using Cardamom.Graphics;
using Cardamom.Mathematics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class SpotNoise : IFilter
    {
        private static readonly Vector2i s_LocalGroupSize = new(32, 32);

        private static ComputeShader? s_SpotNoiseShader;
        private static readonly int s_FrequencyLocation = 0;
        private static readonly int s_LacunarityLocation = 1;
        private static readonly int s_OctavesLocation = 2;
        private static readonly int s_PersistenceLocation = 3;
        private static readonly int s_AmplitudeLocation = 4;
        private static readonly int s_DensityLocation = 5;
        private static readonly int s_ScaleLocation = 6;
        private static readonly int s_SeedLocation = 7;
        private static readonly int s_ChannelLocation = 8;

        public struct Settings
        {
            public Vector3 Frequency { get; set; } = new(1, 1, 1);
            public Vector3 Lacunarity { get; set; } = new(2, 2, 2);
            public int Octaves { get; set; } = 6;
            public float Persistence { get; set; } = 0.6f;
            public float Amplitude { get; set; } = 1f;
            public IntInterval Density { get; set; } = new(0, 1);
            public Interval Scale { get; set; } = new(1, 1);
            public int Seed { get; set; }

            public Settings() { }
        }

        private readonly Settings _settings;

        private SpotNoise(Settings settings)
        {
            _settings = settings;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            s_SpotNoiseShader ??=
                ComputeShader.FromFile("Resources/ImageProcessing/Filters/spot_noise.comp", s_LocalGroupSize);

            s_SpotNoiseShader.SetVector3(s_FrequencyLocation, _settings.Frequency);
            s_SpotNoiseShader.SetVector3(s_LacunarityLocation, _settings.Lacunarity);
            s_SpotNoiseShader.SetInt32(s_OctavesLocation, _settings.Octaves);
            s_SpotNoiseShader.SetFloat(s_PersistenceLocation, _settings.Persistence);
            s_SpotNoiseShader.SetFloat(s_AmplitudeLocation, _settings.Amplitude);
            s_SpotNoiseShader.SetVector2i(
                s_DensityLocation, new(_settings.Density.Minimum, _settings.Density.Maximum));
            s_SpotNoiseShader.SetVector2(s_ScaleLocation, new(_settings.Scale.Minimum, _settings.Scale.Maximum));
            s_SpotNoiseShader.SetInt32(s_SeedLocation, _settings.Seed);

            s_SpotNoiseShader.SetInt32(s_ChannelLocation, (int)channel);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            s_SpotNoiseShader.DoCompute(inTex.Size);
            Texture.UnbindImage(0);
            Texture.UnbindImage(1);
        }

        public class Builder : IFilter.IFilterBuilder
        {
            private Settings _settings = new();

            public Builder SetSeed(int seed)
            {
                _settings.Seed = seed;
                return this;
            }

            public Builder SetFrequency(Vector3 frequency)
            {
                _settings.Frequency = frequency;
                return this;
            }

            public Builder SetLacunarity(Vector3 lacunarity)
            {
                _settings.Lacunarity = lacunarity;
                return this;
            }

            public Builder SetOctaves(int octaves)
            {
                _settings.Octaves = octaves;
                return this;
            }

            public Builder SetPersistence(float persistence)
            {
                _settings.Persistence = persistence;
                return this;
            }

            public Builder SetAmplitude(float amplitude)
            {
                _settings.Amplitude = amplitude;
                return this;
            }

            public Builder SetDensity(IntInterval density)
            {
                _settings.Density = density;
                return this;
            }

            public Builder SetScale(Interval scale)
            {
                _settings.Scale = scale;
                return this;
            }

            public IFilter Build()
            {
                return new SpotNoise(_settings);
            }
        }
    }
}
