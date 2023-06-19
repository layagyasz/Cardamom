namespace Cardamom.Trackers
{
    public interface IPool
    {
        float PercentFull();
        string ToString(string format);
    }
}
