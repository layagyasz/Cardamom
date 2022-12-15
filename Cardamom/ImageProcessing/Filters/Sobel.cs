using Cardamom.Graphics;

namespace Cardamom.ImageProcessing.Filters
{
    public class Sobel : IFilter
    {
        private static Shader? SOBEL_SHADER;
        private static readonly int CHANNEL_INDEX_LOCATION = 0;
        private static readonly int CHANNEL_LOCATION = 1;

        public bool InPlace => false;

        private readonly Channel _channel;

        public Sobel(Channel channel)
        {
            _channel = channel;
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            SOBEL_SHADER ??= new Shader.Builder().SetCompute("Resources/sobel.comp").Build();

            SOBEL_SHADER.SetInt32(CHANNEL_INDEX_LOCATION, _channel.GetIndex());
            SOBEL_SHADER.SetInt32(CHANNEL_LOCATION, (int)channel);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            SOBEL_SHADER.DoCompute(inTex.Size);
            Texture.UnbindImage(0);
            Texture.UnbindImage(1);
        }

        public class Builder : IFilter.IFilterBuilder
        {
            private Channel _channel = Channel.ALL;

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
