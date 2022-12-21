using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Window
{
    public struct KeyDownEventArgs
    {
        public bool Alt => (Modifiers & KeyModifiers.Alt) != 0;
        public bool Command => (Modifiers & KeyModifiers.Shift) != 0;
        public bool Control => (Modifiers & KeyModifiers.Control) != 0;
        public Keys Key { get; set; }
        public KeyModifiers Modifiers { get; set; }
        public int ScanCode { get; set; }
        public bool Shift => (Modifiers & KeyModifiers.Shift) != 0;
        public long TimeDelta { get; set; }
    }
}
