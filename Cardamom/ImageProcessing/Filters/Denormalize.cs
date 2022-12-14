using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    public class Denormalize : IFilter
    {
        private static Shader? DENORMALIZE_SHADER;
        private static readonly int MEAN_LOCATION = 0;
        private static readonly int STANDARD_DEVIATION_LOCATION = 1;
        private static readonly int CHANNEL_LOCATION = 2;

        public bool InPlace => true;

        private readonly Vector4 _mean;
        private readonly Vector4 _standardDeviation;

        public Denormalize(Vector4 mean, Vector4 standardDeviation)
        {
            _mean = mean;
            _standardDeviation = standardDeviation;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            DENORMALIZE_SHADER ??= new Shader.Builder().SetCompute("Resources/denormalize.comp").Build();

            DENORMALIZE_SHADER.SetVector4(MEAN_LOCATION, _mean);
            DENORMALIZE_SHADER.SetVector4(STANDARD_DEVIATION_LOCATION, _standardDeviation);
            DENORMALIZE_SHADER.SetInt32(CHANNEL_LOCATION, (int)channel);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            DENORMALIZE_SHADER.DoCompute(inTex.Size);
            Texture.UnbindImage(0);
            Texture.UnbindImage(1);
        }

        public class Builder : IFilter.IFilterBuilder
        {
            private Vector4 _mean = new(0.5f, 0.5f, 0.5f, 1f);
            private Vector4 _standardDeviation = new(0.075f, 0.075f, 0.075f, 0f);

            public Builder SetMean(Vector4 mean)
            {
                _mean = mean;
                return this;
            }

            public Builder SetStandardDeviation(Vector4 standardDeviation)
            {
                _standardDeviation = standardDeviation;
                return this;
            }

            public IFilter Build()
            {
                return new Denormalize(_mean, _standardDeviation);
            }
        }
    }
}
