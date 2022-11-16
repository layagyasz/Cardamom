namespace Cardamom.Ui
{
    public class ClassLibrary
    {
        private readonly Dictionary<string, Class> _classes = new();

        public void Add(Class @class)
        {
            _classes.Add(@class.Key, @class);
        }

        public Class Get(string key)
        {
            return _classes[key];
        }
    }
}
