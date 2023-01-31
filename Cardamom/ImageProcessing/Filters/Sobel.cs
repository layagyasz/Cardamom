using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    public class Sobel : IFilter
    {
        private static readonly Vector2i s_LocalGroupSize = new(32, 32);

        private static ComputeShader? s_SobelShader;
        private static readonly int s_ChannelIndexLocation = 0;
        private static readonly int s_RoughnessLocation = 1;

        private readonly float _roughness;

        public Sobel(float roughness)
        {
            _roughness = roughness;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            s_SobelShader ??= ComputeShader.FromFile("Resources/ImageProcessing/Filters/sobel.comp", s_LocalGroupSize);

            s_SobelShader.SetInt32(s_ChannelIndexLocation, channel.GetIndex());
            s_SobelShader.SetFloat(s_RoughnessLocation, _roughness);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            s_SobelShader.DoCompute(inTex.Size);
            Texture.UnbindImage(0);
            Texture.UnbindImage(1);
        }

        public class Builder : IFilter.IFilterBuilder
        {
            private float _roughness = 1;

            public Builder SetRoughness(float roughness)
            {
                _roughness = roughness;
                return this;
            }

            public IFilter Build()
            {
                return new Sobel(_roughness);
            }
        }
    }
}
