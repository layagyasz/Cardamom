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
        private static readonly int s_FrequencyLocation = 3;
        private static readonly int s_ChannelLocation = 4;

        public enum WaveType
        {
            Sine = 0,
            Cosine = 1
        }

        public struct Settings
        {
            public WaveType WaveType { get; set; } = WaveType.Sine;
            public Vector4 Amplitude { get; set; } = new(0.5f, 0.5f, 0.5f, 0.5f);
            public Vector4 Bias { get; set; } = new(0.5f, 0.5f, 0.5f, 0.5f);
            public Matrix4 Frequency { get; set; } =
                new(new(1, 1, 1, 1), new(1, 1, 1, 1), new(1, 1, 1, 1), new(1, 1, 1, 1));

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
            s_WaveFormShader.SetVector4(s_AmplitudeLocation, _settings.Amplitude);
            s_WaveFormShader.SetVector4(s_BiasLocation, _settings.Bias);
            s_WaveFormShader.SetMatrix4(s_FrequencyLocation, _settings.Frequency);

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

            public Builder SetAmplitude(Vector4 amplitude)
            {
                _settings.Amplitude = amplitude;
                return this;
            }

            public Builder SetBias(Vector4 bias)
            {
                _settings.Bias = bias;
                return this;
            }

            public Builder SetFrequency(Matrix4 frequency)
            {
                _settings.Frequency = frequency;
                return this;
            }

            public IFilter Build()
            {
                return new WaveForm(_settings);
            }
        }
    }
}
