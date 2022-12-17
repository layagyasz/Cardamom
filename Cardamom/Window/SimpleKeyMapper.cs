using Cardamom.Collections;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Cardamom.Window
{
    public class SimpleKeyMapper : IKeyMapper
    {
        public static readonly SimpleKeyMapper Us =
            new(
                new()
                {
                    {Keys.A, 'a' },
                    {Keys.Apostrophe, '\'' },
                    {Keys.B, 'b' },
                    {Keys.Backslash, '\\' },
                    {Keys.C, 'c' },
                    {Keys.Comma, ',' },
                    {Keys.D, 'd' },
                    {Keys.D0, '0' },
                    {Keys.D1, '1' },
                    {Keys.D2, '2' },
                    {Keys.D3, '3' },
                    {Keys.D4, '4' },
                    {Keys.D5, '5' },
                    {Keys.D6, '6' },
                    {Keys.D7, '7' },
                    {Keys.D8, '8' },
                    {Keys.D9, '9' },
                    {Keys.E, 'e' },
                    {Keys.Enter, '\n' },
                    {Keys.Equal, '=' },
                    {Keys.E, 'e' },
                    {Keys.F, 'f' },
                    {Keys.G, 'g' },
                    {Keys.GraveAccent, '`' },
                    {Keys.H, 'h' },
                    {Keys.I, 'i' },
                    {Keys.J, 'j' },
                    {Keys.K, 'k' },
                    {Keys.L, 'l' },
                    {Keys.LeftBracket, '[' },
                    {Keys.L, 'l' },
                    {Keys.M, 'm' },
                    {Keys.Minus, '-' },
                    {Keys.N, 'n' },
                    {Keys.O, 'o' },
                    {Keys.P, 'p' },
                    {Keys.Period, '.' },
                    {Keys.Q, 'q' },
                    {Keys.R, 'r' },
                    {Keys.RightBracket, ']' },
                    {Keys.S, 's' },
                    {Keys.Semicolon, ';' },
                    {Keys.Slash, '/' },
                    {Keys.Space, ' ' },
                    {Keys.T, 't' },
                    {Keys.Tab, '\t' },
                    {Keys.U, 'u' },
                    {Keys.V, 'v' },
                    {Keys.W, 'w' },
                    {Keys.X, 'x' },
                    {Keys.Y, 'y' },
                    {Keys.Z, 'z' }
                },
                new()
                {
                    {Keys.Apostrophe, '"' },
                    {Keys.Backslash, '|' },
                    {Keys.Comma, '<' },
                    {Keys.D0, ')' },
                    {Keys.D1, '!' },
                    {Keys.D2, '@' },
                    {Keys.D3, '#' },
                    {Keys.D4, '$' },
                    {Keys.D5, '%' },
                    {Keys.D6, '^' },
                    {Keys.D7, '&' },
                    {Keys.D8, '*' },
                    {Keys.D9, '(' },
                    {Keys.Equal, '+' },
                    {Keys.GraveAccent, '~' },
                    {Keys.LeftBracket, '{' },
                    {Keys.Minus, '_' },
                    {Keys.Period, '>' },
                    {Keys.RightBracket, '}' },
                    {Keys.Semicolon, ':' },
                    {Keys.Slash, '?' },
                }
                );

        private readonly EnumMap<Keys, char> _map;
        private readonly EnumMap<Keys, char> _shiftMap;

        public SimpleKeyMapper(EnumMap<Keys, char> map, EnumMap<Keys, char> shiftMap)
        {
            _map = map;
            _shiftMap = shiftMap;
        }

        public string Map(KeyboardKeyEventArgs key)
        {
            char value;
            if (key.Shift || key.Modifiers.HasFlag(KeyModifiers.CapsLock))
            {
                if (_shiftMap.TryGetValue(key.Key, out value))
                {
                    return value.ToString();
                }
                if (_map.TryGetValue(key.Key, out value))
                {
                    return char.ToUpper(_map[key.Key]).ToString();
                }
                return string.Empty;
            }
            if (_map.TryGetValue(key.Key, out value))
            {
                return value.ToString();
            }
            return string.Empty;
        }
    }
}
