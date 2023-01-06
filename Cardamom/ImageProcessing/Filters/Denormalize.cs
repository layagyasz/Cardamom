using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class Denormalize : IFilter
    {
        private static ComputeShader? s_DenormalizeShader;
        private static readonly int s_MeanLocation = 0;
        private static readonly int s_StandardDeviationLocation = 1;
        private static readonly int s_ChannelLocation = 2;

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

            s_DenormalizeShader ??= ComputeShader.FromFile("Resources/ImageProcessing/Filters/denormalize.comp");

            s_DenormalizeShader.SetVector4(s_MeanLocation, _mean);
            s_DenormalizeShader.SetVector4(s_StandardDeviationLocation, _standardDeviation);
            s_DenormalizeShader.SetInt32(s_ChannelLocation, (int)channel);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            s_DenormalizeShader.DoCompute(inTex.Size);
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
