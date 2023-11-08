namespace Cardamom.Trackers
{
    public interface IPool
    {
        bool IsEmpty();
        float PercentFull();
        string ToString(string format);
    }
}
