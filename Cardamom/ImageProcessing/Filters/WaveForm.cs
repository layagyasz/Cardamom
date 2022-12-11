using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    public class WaveForm : IFilter
    {
        private static Shader? WAVE_FORM_SHADER;
        private static readonly int WAVE_TYPE_LOCATION = 0;
        private static readonly int AMPLITITUDE_LOCATION = 1;
        private static readonly int BIAS_LOCATION = 2;
        private static readonly int PERIODICITY_LOCATION = 3;
        private static readonly int TURBULENCE_LOCATION = 4;
        private static readonly int CHANNEL_LOCATION = 5;

        public enum WaveType
        {
            SINE = 0,
            COSINE = 1
        }

        public struct Settings
        {
            public WaveType WaveType { get; set; } = WaveType.SINE;
            public float Amplitude { get; set; } = 0.5f;
            public float Bias { get; set; } = 0.5f;
            public Vector2 Periodicity { get; set; } = new(0.01f, 0.01f);
            public Vector2 Turbulence { get; set; } = new();

            public Settings() { }
        }

        public bool InPlace => true;

        private readonly Settings _settings;

        public WaveForm(Settings settings)
        {
            _settings = settings;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1 && inputs.First().Value == output);

            WAVE_FORM_SHADER ??= new Shader.Builder().SetCompute("Resources/wave_form.comp").Build();

            WAVE_FORM_SHADER.SetInt32(WAVE_TYPE_LOCATION, (int)_settings.WaveType);
            WAVE_FORM_SHADER.SetFloat(AMPLITITUDE_LOCATION, _settings.Amplitude);
            WAVE_FORM_SHADER.SetFloat(BIAS_LOCATION, _settings.Bias);
            WAVE_FORM_SHADER.SetVector2(PERIODICITY_LOCATION, _settings.Periodicity);
            WAVE_FORM_SHADER.SetVector2(TURBULENCE_LOCATION, _settings.Turbulence);

            WAVE_FORM_SHADER.SetInt32(CHANNEL_LOCATION, (int)channel);
            WAVE_FORM_SHADER.DoCompute(output.GetTexture());
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

            public IFilter Build()
            {
                return new WaveForm(_settings);
            }
        }
    }
}
