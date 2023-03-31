using OpenTK.Mathematics;

namespace Cardamom.Graphics.Camera
{
    public interface ICamera
    {
        EventHandler<EventArgs>? Changed { get; set; }

        Vector3 Position { get; }
        float AspectRatio { get; }
        void SetAspectRatio(float aspectRatio);
        Matrix4 GetViewMatrix();
        Projection GetProjection();
    }
}
