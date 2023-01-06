using Cardamom.Graphics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    public class Sobel : IFilter
    {
        private static ComputeShader? s_SobelShader;
        private static readonly int s_ChannelIndexLocation = 0;
        private static readonly int s_ChannelLocation = 1;

        private readonly Channel _channel;

        public Sobel(Channel channel)
        {
            _channel = channel;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            s_SobelShader ??= ComputeShader.FromFile("Resources/ImageProcessing/Filters/sobel.comp");

            s_SobelShader.SetInt32(s_ChannelIndexLocation, channel.GetIndex());
            s_SobelShader.SetInt32(s_ChannelLocation, (int)_channel);

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
            private Channel _channel = Channel.All;

            public Builder SetChannel(Channel channel)
            {
                _channel = channel;
                return this;
            }

            public IFilter Build()
            {
                return new Sobel(_channel);
            }
        }
    }
}
