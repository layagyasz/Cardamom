namespace Cardamom.ImageProcessing
{
    [Flags]
    public enum Channel
    {
        NONE = 0,
        RED = 1,
        GREEN = 2,
        BLUE = 4,
        ALPHA = 8,
        ALL = 15
    }
}
