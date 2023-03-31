using OpenTK.Mathematics;

namespace Cardamom.Graphics.Camera
{
    public abstract class BaseCamera3d : ICamera
    {
        public EventHandler<EventArgs>? Changed { get; set; }

        public Vector3 Position { get; protected set; }
        public float Pitch { get; private set; } = 0;
        public float Yaw { get; private set; } = 0;
        public float AspectRatio { get; private set; }
        public float FarPlane { get; }
        public float FieldOfView { get; private set; } = MathHelper.PiOver2;

        private bool _updateView = true;
        private Matrix4 _view;

        private bool _updateProjection = true;
        private Projection _projection;

        protected BaseCamera3d(float farPlane)
        {
            FarPlane = farPlane;
        }

        protected abstract Matrix4 GetViewMatrixImpl();
        protected abstract Projection GetProjectionImpl();

        public void InvalidateView()
        {
            _updateView = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public void SetAspectRatio(float aspectRatio)
        {
            AspectRatio = aspectRatio;
            _updateProjection = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public void SetPitch(float pitch)
        {
            Pitch = pitch;
            _updateView = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public void SetYaw(float yaw)
        {
            Yaw = yaw;
            _updateView = true;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public void SetFieldOfView(float fieldOfView)
        {
            FieldOfView = MathHelper.Clamp(FieldOfView + fieldOfView, 0.01f, MathHelper.Pi - 0.1f);
            _updateProjection = true;
            Changed?.Invoke(this, EventArgs.Empty);
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

        public Projection GetProjection()
        {
            if (_updateProjection)
            {
                _projection = GetProjectionImpl();
                _updateProjection = false;
            }
            return _projection;
        }
    }
}
