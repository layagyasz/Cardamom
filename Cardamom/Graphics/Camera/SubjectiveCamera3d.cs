using OpenTK.Mathematics;

namespace Cardamom.Graphics.Camera
{
    public class SubjectiveCamera3d : BaseCamera3d
    {
        public Vector3 Focus { get; private set; }
        public Vector3 Center { get; private set; }
        public float Distance { get; private set; }

        public SubjectiveCamera3d(float aspectRatio, float farPlane, Vector3 focus, Vector3 center, float distance)
            : base(aspectRatio, farPlane)
        {
            Focus = focus;
            Center = center;
            Distance = distance;
        }

        public void SetFocus(Vector3 focus)
        {
            Focus = focus;
            InvalidateView();
        }

        public void SetCenter(Vector3 center)
        {
            Center = center;
            InvalidateView();
        }

        public void SetDistance(float distance)
        {
            Distance = distance;
            InvalidateView();
        }

        protected override Matrix4 GetViewMatrixImpl()
        {
            return Matrix4.CreateTranslation(Focus) 
                * Matrix4.CreateRotationZ(Roll) 
                * Matrix4.CreateRotationX(Pitch)
                * Matrix4.CreateTranslation(new Vector3(0, 0, -Distance))
                * Matrix4.CreateTranslation(Center);
        }

        protected override Matrix4 GetProjectionMatrixImpl()
        {
            return Matrix4.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, 0.01f, FarPlane);
        }
    }
}
