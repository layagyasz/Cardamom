using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class WaveForm : IFilter
    {
        private static ComputeShader? s_WaveFormShader;
        private static readonly int s_WaveTypeLocation = 0;
        private static readonly int s_AmplitudeLocation = 1;
        private static readonly int s_BiasLocation = 2;
        private static readonly int s_PeriodicityLocation = 3;
        private static readonly int s_TurbulenceLocation = 4;
        private static readonly int s_ScaleLocation = 5;
        private static readonly int s_OffsetLocation = 6;
        private static readonly int s_ChannelLocation = 7;

        public enum WaveType
        {
            Sine = 0,
            Cosine = 1
        }

        public struct Settings
        {
            public WaveType WaveType { get; set; } = WaveType.Sine;
            public float Amplitude { get; set; } = 0.5f;
            public float Bias { get; set; } = 0.5f;
            public Vector2 Periodicity { get; set; } = new(0.01f, 0.01f);
            public Vector2 Turbulence { get; set; } = new();
            public Vector2 Scale { get; set; } = new(1, 1);
            public Vector2 Offset { get; set; } = new();

            public Settings() { }
        }

        private readonly Settings _settings;

        public WaveForm(Settings settings)
        {
            _settings = settings;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            s_WaveFormShader ??= ComputeShader.FromFile("Resources/ImageProcessing/Filters/wave_form.comp");

            s_WaveFormShader.SetInt32(s_WaveTypeLocation, (int)_settings.WaveType);
            s_WaveFormShader.SetFloat(s_AmplitudeLocation, _settings.Amplitude);
            s_WaveFormShader.SetFloat(s_BiasLocation, _settings.Bias);
            s_WaveFormShader.SetVector2(s_PeriodicityLocation, _settings.Periodicity);
            s_WaveFormShader.SetVector2(s_TurbulenceLocation, _settings.Turbulence);
            s_WaveFormShader.SetVector2(s_ScaleLocation, _settings.Scale);
            s_WaveFormShader.SetVector2(s_OffsetLocation, _settings.Offset);

            s_WaveFormShader.SetInt32(s_ChannelLocation, (int)channel);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            s_WaveFormShader.DoCompute(inTex.Size);
            Texture.UnbindImage(0);
            Texture.UnbindImage(1);
        }

        public class Builder : IFilter.IFilterBuilder
        {
            private Settings _settings = new();

            public Builder SetWaveType(WaveType waveType)
            {
                _settings.WaveType = waveType;
                return this;
            }

            public Builder SetAmplitude(float amplitude)
            {
                _settings.Amplitude = amplitude;
                return this;
            }

            public Builder SetBias(float bias)
            {
                _settings.Bias = bias;
                return this;
            }

            public Builder SetPeriodicity(Vector2 periodicity)
            {
                _settings.Periodicity = periodicity;
                return this;
            }

            public Builder SetTurbulence(Vector2 turbulence)
            {
                _settings.Turbulence = turbulence;
                return this;
            }

            public Builder SetScale(Vector2 scale)
            {
                _settings.Scale = scale;
                return this;
            }

            public Builder SetOffset(Vector2 offset)
            {
                _settings.Offset = offset;
                return this;
            }

            public IFilter Build()
            {
                return new WaveForm(_settings);
            }
        }
    }
}
