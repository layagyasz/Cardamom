namespace Cardamom.Logging
{
    public interface ILogBackend
    {
        void Write(string message);
    }
}
