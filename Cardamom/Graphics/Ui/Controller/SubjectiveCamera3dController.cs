using Cardamom.Graphics.Camera;
using Cardamom.Mathematics;
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
        public Interval PitchRange { get; set; } = Interval.Unbounded;
        public Interval RollRange { get; set; } = Interval.Unbounded;
        public Interval DistanceRange { get; set; } = Interval.Unbounded;

        private readonly SubjectiveCamera3d? _camera;

        public SubjectiveCamera3dController(SubjectiveCamera3d camera)
        {
            _camera = camera;
        }

        public void Bind(object @object) { }

        public void Unbind() { }

        public bool HandleKeyDown(KeyDownEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Left:
                    _camera!.SetYaw(RollRange.Clamp(_camera.Yaw - KeySensitivity * e.TimeDelta));
                    return true;
                case Keys.Right:
                    _camera!.SetYaw(RollRange.Clamp(_camera.Yaw + KeySensitivity * e.TimeDelta));
                    return true;
                case Keys.Up:
                    _camera!.SetPitch(PitchRange.Clamp(_camera.Pitch + KeySensitivity * e.TimeDelta));
                    return true;
                case Keys.Down:
                    _camera!.SetPitch(PitchRange.Clamp(_camera.Pitch - KeySensitivity * e.TimeDelta));
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
