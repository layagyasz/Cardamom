using OpenTK.Mathematics;

namespace Cardamom.Ui
{
    public interface IUiContext
    {
        void Clear();
        void Flatten();
        IInteractive? GetTopElement();
        Vector3 GetTopIntersection();
        Vector2 GetCursorPosition();
        void Register(IInteractive element);
    }
}
