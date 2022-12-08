using OpenTK.Windowing.Common;

namespace Cardamom.Window
{
    public interface IKeyMapper
    {
        string Map(KeyboardKeyEventArgs key);
    }
}
