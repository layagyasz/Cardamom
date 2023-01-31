using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class Gradient : IFilter
    {
        private static readonly Vector2i s_LocalGroupSize = new(32, 32);

        private static ComputeShader? s_GradientShader;
        private static readonly int s_OverflowBehaviorLocation = 0;
        private static readonly int s_GradientLocation = 1;
        private static readonly int s_ChannelLocation = 2;

        public struct Settings
        {
            public Matrix4x2 Gradient { get; set; } = 
                new(new(0.5f, 0.5f), new(0.5f, 0.5f), new(0.5f, 0.5f), new(0.5f, 0.5f));
            public Vector4 Bias { get; set; }
            public Vector2 Scale { get; set; } = new(1, 1);
            public Vector2 Offset { get; set; }
            public OverflowBehavior OverflowBehavior { get; set; } = OverflowBehavior.Clamp;

            public Settings() { }
        }

        private readonly Settings _settings;

        public Gradient(Settings settings)
        {
            _settings = settings;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            s_GradientShader ??= 
                ComputeShader.FromFile("Resources/ImageProcessing/Filters/gradient.comp", s_LocalGroupSize);

            Matrix4 gradient = new()
            {
                Row0 = _settings.Scale.X * _settings.Gradient.Column0,
                Row1 = _settings.Scale.Y * _settings.Gradient.Column1,
                Row3 = 
                    new(
                        Vector2.Dot(_settings.Gradient.Row0, _settings.Offset) + _settings.Bias.X,
                        Vector2.Dot(_settings.Gradient.Row1, _settings.Offset) + _settings.Bias.Y,
                        Vector2.Dot(_settings.Gradient.Row2, _settings.Offset) + _settings.Bias.Z,
                        Vector2.Dot(_settings.Gradient.Row3, _settings.Offset) + _settings.Bias.W)
            };

            s_GradientShader.SetInt32(s_OverflowBehaviorLocation, (int)_settings.OverflowBehavior);
            s_GradientShader.SetMatrix4(s_GradientLocation, gradient);
            s_GradientShader.SetInt32(s_ChannelLocation, (int)channel);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            s_GradientShader.DoCompute(inTex.Size);
            Texture.UnbindImage(0);
            Texture.UnbindImage(1);
        }

        public class Builder : IFilter.IFilterBuilder
        {
            private Settings _settings = new();

            public Builder SetGradient(Matrix4x2 gradient)
            {
                _settings.Gradient = gradient;
                return this;
            }

            public Builder SetBias(Vector4 bias)
            {
                _settings.Bias = bias;
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

            public Builder SetOverflowBehavior(OverflowBehavior overflowBehavior)
            {
                _settings.OverflowBehavior = overflowBehavior;
                return this;
            }

            public IFilter Build()
            {
                return new Gradient(_settings);
            }
        }
    }
}
