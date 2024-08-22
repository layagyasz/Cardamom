using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class WhiteNoise : IFilter
    {
        private static readonly Vector2i s_LocalGroupSize = new(32, 32);

        private static ComputeShader? s_WhiteNoiseShader;
        private static readonly int s_SeedLocation = 0;
        private static readonly int s_ChannelLocation = 1;

        private readonly int _seed;

        public WhiteNoise(int seed)
        {
            _seed = seed;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            s_WhiteNoiseShader ??= 
                ComputeShader.FromFile("Resources/ImageProcessing/Filters/white_noise.comp", s_LocalGroupSize);

            s_WhiteNoiseShader.SetInt32(s_SeedLocation, _seed);
            s_WhiteNoiseShader.SetInt32(s_ChannelLocation, (int)channel);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            s_WhiteNoiseShader.DoCompute(inTex.Size);
            Texture.UnbindImage(0);
            Texture.UnbindImage(1);
        }

        public class Builder : IFilter.IFilterBuilder
        {
            private int _seed;

            public Builder SetSeed(int seed)
            {
                _seed = seed;
                return this;
            }

            public IFilter Build()
            {
                return new WhiteNoise(_seed);
            }
        }
    }
}
