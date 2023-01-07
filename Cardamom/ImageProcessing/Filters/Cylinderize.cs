using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class Cylinderize : IFilter
    {
        private static ComputeShader? s_CylinderizeShader;
        private static readonly int s_YRangeLocation = 0;
        private static readonly int s_RadiusLocation = 1;

        private readonly Vector2 _yRange;
        private readonly float _radius;

        public Cylinderize(Vector2 yRange, float radius)
        {
            _yRange = yRange;
            _radius = radius;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);
            Precondition.Check(channel == Channel.All);

            s_CylinderizeShader ??= ComputeShader.FromFile("Resources/ImageProcessing/Filters/cylinderize.comp");

            s_CylinderizeShader.SetVector2(s_YRangeLocation, _yRange);
            s_CylinderizeShader.SetFloat(s_RadiusLocation, _radius);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            s_CylinderizeShader.DoCompute(inTex.Size);
            Texture.UnbindImage(0);
            Texture.UnbindImage(1);
        }

        public class Builder : IFilter.IFilterBuilder
        {
            private Vector2 _yRange = new(-1, 1);
            private float _radius = 1;

            public Builder SetYRange(Vector2 yRange)
            {
                _yRange = yRange;
                return this;
            }

            public Builder SetRadius(float radius)
            {
                _radius = radius;
                return this;
            }

            public IFilter Build()
            {
                return new Cylinderize(_yRange, _radius);
            }
        }
    }
}
