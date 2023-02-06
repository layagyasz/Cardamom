using Cardamom.Graphics.Camera;
using Cardamom.Mathematics;
using Cardamom.Window;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Ui.Controller.Element
{
    public class SubjectiveCamera3dController : IElementController
    {
        public EventHandler<MouseButtonClickEventArgs>? Clicked { get; set; }
        public EventHandler<EventArgs>? Focused { get; set; }

        public float KeySensitivity { get; set; } = 1f;
        public float MouseSensitivity { get; set; } = 1f;
        public float MouseWheelSensitivity { get; set; } = 1f;
        public Interval PitchRange { get; set; } = Interval.Unbounded;
        public Interval YawRange { get; set; } = Interval.Unbounded;
        public Interval DistanceRange { get; set; } = Interval.Unbounded;

        private readonly SubjectiveCamera3d _camera;
        private readonly float _surfaceDepth;

        public SubjectiveCamera3dController(SubjectiveCamera3d camera, float surfaceDepth)
        {
            _camera = camera;
            _surfaceDepth = surfaceDepth;
        }

        public void Bind(object @object) { }

        public void Unbind() { }

        public bool HandleKeyDown(KeyDownEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Left:
                    _camera.SetYaw(YawRange.Clamp(_camera.Yaw - KeySensitivity * e.TimeDelta));
                    return true;
                case Keys.Right:
                    _camera.SetYaw(YawRange.Clamp(_camera.Yaw + KeySensitivity * e.TimeDelta));
                    return true;
                case Keys.Up:
                    _camera.SetPitch(PitchRange.Clamp(_camera.Pitch + KeySensitivity * e.TimeDelta));
                    return true;
                case Keys.Down:
                    _camera.SetPitch(PitchRange.Clamp(_camera.Pitch - KeySensitivity * e.TimeDelta));
                    return true;
            }
            return false;
        }

        public bool HandleTextEntered(TextEnteredEventArgs e)
        {
            return false;
        }

        public bool HandleMouseEntered()
        {
            return false;
        }

        public bool HandleMouseLeft()
        {
            return false;
        }

        public bool HandleMouseButtonClicked(MouseButtonClickEventArgs e)
        {
            return false;
        }

        public bool HandleMouseButtonDragged(MouseButtonDragEventArgs e)
        {
            var d = (_camera.Distance - _surfaceDepth) * MouseSensitivity * e.NdcDelta;
            _camera.SetPitch(PitchRange.Clamp(_camera.Pitch + d.Y));
            _camera.SetYaw(YawRange.Clamp(_camera.Yaw + d.X));
            return true;
        }

        public bool HandleMouseWheelScrolled(MouseWheelEventArgs e)
        {
            _camera.SetDistance(DistanceRange.Clamp(_camera.Distance - MouseWheelSensitivity * e.OffsetY));
            return true;
        }

        public bool HandleMouseLingered()
        {
            return false;
        }

        public bool HandleMouseLingerBroken()
        {
            return false;
        }

        public bool HandleFocusEntered()
        {
            return false;
        }

        public bool HandleFocusLeft()
        {
            return false;
        }
    }
}
