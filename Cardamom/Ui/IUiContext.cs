using OpenTK.Mathematics;

namespace Cardamom.Ui
{
    public interface IUiContext
    {
        void Clear();
        void Flatten();
        IInteractive? GetTopElement();
        Vector3 GetTopIntersection();
        void Register(IInteractive element);
    }
}
