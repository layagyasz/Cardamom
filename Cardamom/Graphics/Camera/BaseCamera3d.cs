using OpenTK.Mathematics;

namespace Cardamom.Graphics.Camera
{
    public abstract class BaseCamera3d : ICamera
    {
        public float Pitch { get; private set; } = 0;
        public float Roll { get; private set; } = 0;
        public float AspectRatio { get; private set; }
        public float FarPlane { get; }
        public float FieldOfView { get; private set; } = MathHelper.PiOver2;

        private bool _updateView = true;
        private Matrix4 _view;

        private bool _updateProjection = true;
        private Matrix4 _projection;

        protected BaseCamera3d(float aspectRatio, float farPlane)
        {
            AspectRatio = aspectRatio;
            FarPlane = farPlane;
        }

        protected abstract Matrix4 GetViewMatrixImpl();
        protected abstract Matrix4 GetProjectionMatrixImpl();

        public void InvalidateView()
        {
            _updateView = true;
        }

        public void SetAspectRatio(float aspectRatio)
        {
            AspectRatio = aspectRatio;
            _updateProjection = true;
        }

        public void SetPitch(float pitch)
        {
            Pitch = pitch;
            _updateView = true;
        }

        public void SetRoll(float roll)
        {
            Roll = roll;
            _updateView = true;
        }

        public void SetFieldOfView(float fieldOfView)
        {
            FieldOfView = MathHelper.Clamp(FieldOfView + fieldOfView, 0.01f, MathHelper.Pi - 0.1f);
            _updateProjection = true;
        }

        public Matrix4 GetViewMatrix()
        {
            if (_updateView)
            {
                _view = GetViewMatrixImpl();
                _updateView = false;
            }
            return _view;
        }

        public Matrix4 GetProjectionMatrix()
        {
            if (_updateProjection)
            {
                _projection = GetProjectionMatrixImpl();
                _updateProjection = false;
            }
            return _projection;
        }
    }
}
