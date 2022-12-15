namespace Cardamom.ImageProcessing
{
    public static class Extensions
    {
        public static int GetIndex(this Channel channel)
        {
            switch (channel)
            {
                case Channel.RED:
                    return 0;
                case Channel.GREEN:
                    return 1;
                case Channel.BLUE:
                    return 2;
                case Channel.ALPHA:
                    return 3;
            }
            throw new ArgumentException("Only one input channel supported.");
        }
    }
}
