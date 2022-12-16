using Cardamom.Graphics.Camera;
using Cardamom.Planar;
using Cardamom.Window;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Graphics.Ui.Controller
{
    public class SubjectiveCamera3dController : IController
    {
        public EventHandler<MouseButtonEventArgs>? Clicked { get; set; }
        public EventHandler<EventArgs>? Focused { get; set; }

        public float KeySensitivity { get; set; } = 1f;
        public float MouseWheelSensitivity { get; set; } = 1f;
        public FloatRange PitchRange { get; set; } = FloatRange.UNBOUNDED;
        public FloatRange YawRange { get; set; } = FloatRange.UNBOUNDED;
        public FloatRange DistanceRange { get; set; } = FloatRange.UNBOUNDED;

        private SubjectiveCamera3d? _camera;

        public SubjectiveCamera3dController(SubjectiveCamera3d camera)
        {
            _camera = camera;
        }

        public void Bind(object @object) { }

        public void Unbind() { }

        public bool HandleKeyPressed(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Left:
                    _camera!.SetYaw(YawRange.Clamp(_camera.Yaw - KeySensitivity));
                    return true;
                case Keys.Right:
                    _camera!.SetYaw(YawRange.Clamp(_camera.Yaw + KeySensitivity));
                    return true;
                case Keys.Up:
                    _camera!.SetPitch(PitchRange.Clamp(_camera.Pitch + KeySensitivity));
                    return true;
                case Keys.Down:
                    _camera!.SetPitch(PitchRange.Clamp(_camera.Pitch - KeySensitivity));
                    return true;
            }
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

        public bool HandleMouseButtonClicked(MouseButtonEventArgs e)
        {
            return false;
        }

        public bool HandleMouseButtonDragged(MouseButtonDragEventArgs e)
        {
            return false;
        }

        public bool HandleMouseWheelScrolled(MouseWheelEventArgs e)
        {
            _camera!.SetDistance(DistanceRange.Clamp(_camera.Distance - MouseWheelSensitivity * e.OffsetY));
            return true;
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
