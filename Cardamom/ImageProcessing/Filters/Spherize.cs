using Cardamom.Graphics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class Spherize : IFilter
    {
        private static ComputeShader? s_SpherizeShader;
        private static readonly int s_YScaleLocation = 0;
        private static readonly int s_RadiusLocation = 1;

        public enum YScale
        {
            None = 0,
            Zenith = 1
        }

        private readonly YScale _yScale;
        private readonly float _radius;

        public Spherize(YScale yScale, float radius)
        {
            _yScale = yScale;
            _radius = radius;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);
            Precondition.Check(channel == Channel.All);

            s_SpherizeShader ??= ComputeShader.FromFile("Resources/ImageProcessing/Filters/spherize.comp");

            s_SpherizeShader.SetInt32(s_YScaleLocation, (int)_yScale);
            s_SpherizeShader.SetFloat(s_RadiusLocation, _radius);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            s_SpherizeShader.DoCompute(inTex.Size);
            Texture.UnbindImage(0);
            Texture.UnbindImage(1);
        }

        public class Builder : IFilter.IFilterBuilder
        {
            private YScale _yScale = YScale.Zenith;
            private float _radius = 1;

            public Builder SetYScale(YScale yScale)
            {
                _yScale = yScale;
                return this;
            }

            public Builder SetRadius(float radius)
            {
                _radius = radius;
                return this;
            }

            public IFilter Build()
            {
                return new Spherize(_yScale, _radius);
            }
        }
    }
}
