using OpenTK.Mathematics;

namespace Cardamom.Graphics.Camera
{
    public class SubjectiveCamera3d : BaseCamera3d
    {
        private static readonly float s_NearPlane = 0.01f;

        public Vector3 Focus { get; private set; }
        public float Distance { get; private set; }

        public SubjectiveCamera3d(float aspectRatio, float farPlane, Vector3 focus, float distance)
            : base(aspectRatio, farPlane)
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
            var front =
                new Vector3(MathF.Cos(Pitch) * MathF.Cos(Yaw), MathF.Sin(Pitch), MathF.Cos(Pitch) * MathF.Sin(Yaw))
                    .Normalized();
            var right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            var up = Vector3.Normalize(Vector3.Cross(right, front));

            return Matrix4.LookAt(Focus - Distance * front, Focus, up);
        }

        protected override Projection GetProjectionImpl()
        {
            return new(
                s_NearPlane, Matrix4.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, s_NearPlane, FarPlane));
        }
    }
}
