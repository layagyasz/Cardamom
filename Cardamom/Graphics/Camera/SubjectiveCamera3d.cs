using OpenTK.Mathematics;

namespace Cardamom.Graphics.Camera
{
    public class SubjectiveCamera3d : BaseCamera3d
    {
        public Vector3 Focus { get; private set; }
        public float Distance { get; private set; }

        public SubjectiveCamera3d(float aspectRatio, Vector3 focus, float distance)
            : base(aspectRatio)
        {
            Focus = focus;
            Distance = distance;
        }

        public void SetFocus(Vector3 focus)
        {
            Focus = focus;
            InvalidateView();
        }

        public void SetDistance(float distance)
        {
            Distance = distance;
            InvalidateView();
        }

        protected override Matrix4 GetViewMatrixImpl()
        {
            return Matrix4.CreateTranslation(Focus) * Matrix4.CreateRotationZ(Yaw) * Matrix4.CreateRotationX(Pitch);
        }

        protected override Matrix4 GetProjectionMatrixImpl()
        {
            return Matrix4.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, 0.01f, 100f);
        }
    }
}
