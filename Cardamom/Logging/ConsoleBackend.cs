namespace Cardamom.Logging
{
    public class ConsoleBackend : ILogBackend
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}
