using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Window
{
    public struct TextEnteredEventArgs
    {
        public bool Alt { get; set; }
        public bool Command { get; set; }
        public bool Control { get; set; }
        public bool IsRepeat { get; set; }
        public Keys Key { get; set; }
        public KeyModifiers Modifiers { get; set; }
        public int ScanCode { get; set; }
        public bool Shift { get; set; }
        public string Text { get; set; }
    }
}
