namespace Cardamom.ImageProcessing
{
    public static class Extensions
    {
        public static int GetIndex(this Channel channel)
        {
            switch (channel)
            {
                case Channel.Red:
                    return 0;
                case Channel.Green:
                    return 1;
                case Channel.Blue:
                    return 2;
                case Channel.Alpha:
                    return 3;
            }
            throw new ArgumentException("Only one input channel supported.");
        }
    }
}
