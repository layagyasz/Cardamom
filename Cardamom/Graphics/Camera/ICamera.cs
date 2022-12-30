using OpenTK.Mathematics;

namespace Cardamom.Graphics.Camera
{
    public interface ICamera
    {
        float AspectRatio { get; }
        void SetAspectRatio(float aspectRatio);
        Matrix4 GetViewMatrix();
        Projection GetProjection();
    }
}
