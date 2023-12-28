namespace Cardamom.Utils.IO
{
    public static class Glob
    {
        public static IEnumerable<string> GetFiles(string query)
        {
            var p = query.Split("::");
            return Directory.EnumerateFiles(
                p.Length > 1 ? p[0] : string.Empty, p.Length > 1 ? p[1] : p[0], SearchOption.AllDirectories);
        }
    }
}
