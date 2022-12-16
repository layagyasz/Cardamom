using OpenTK.Mathematics;
using System.Diagnostics;

namespace Cardamom.Graphics.Camera
{
    public class SubjectiveCamera3d : BaseCamera3d
    {
        public Vector3 Focus { get; private set; }
        public float Distance { get; private set; }

        private Vector3 _front;
        private Vector3 _right;
        private Vector3 _up;

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
            _front.X = MathF.Cos(Pitch) * MathF.Cos(Yaw);
            _front.Y = MathF.Sin(Pitch);
            _front.Z = MathF.Cos(Pitch) * MathF.Sin(Yaw);
            _front = Vector3.Normalize(_front);
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
            return Matrix4.LookAt(Focus + Distance * _front, Focus, _up);
        }

        protected override Matrix4 GetProjectionMatrixImpl()
        {
            return Matrix4.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, 0.01f, 100f);
        }
    }
}
