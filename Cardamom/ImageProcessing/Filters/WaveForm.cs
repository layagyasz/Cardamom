using Cardamom.Graphics;
using OpenTK.Mathematics;
using System.Text.Json.Serialization;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class WaveForm : IFilter
    {
        private static readonly Vector2i s_LocalGroupSize = new(32, 32);

        private static ComputeShader? s_WaveFormShader;
        private static readonly int s_WaveTypeLocation = 0;
        private static readonly int s_FrequencyLocation = 1;
        private static readonly int s_ChannelLocation = 2;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum WaveType
        {
            Sine = 0,
            Cosine = 1
        }

        private readonly WaveType _waveType;
        private readonly Matrix4 _frequency;

        public WaveForm(WaveType waveType, Matrix4 frequency)
        {
            _waveType = waveType;
            _frequency = frequency;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            s_WaveFormShader ??= 
                ComputeShader.FromFile("Resources/ImageProcessing/Filters/wave_form.comp", s_LocalGroupSize);

            s_WaveFormShader.SetInt32(s_WaveTypeLocation, (int)_waveType);
            s_WaveFormShader.SetMatrix4(s_FrequencyLocation, _frequency);
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
            private WaveType _waveType = WaveType.Sine;
            private Matrix4 _frequency = Matrix4.Identity;

            public Builder SetWaveType(WaveType waveType)
            {
                _waveType = waveType;
                return this;
            }

            public Builder SetFrequency(Matrix4 frequency)
            {
                _frequency = frequency;
                return this;
            }

            public IFilter Build()
            {
                return new WaveForm(_waveType, _frequency);
            }
        }
    }
}
