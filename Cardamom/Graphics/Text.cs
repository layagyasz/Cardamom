using Cardamom.Collections;
using Cardamom.Graphics.Ui;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public class Text : IRenderable
    {
        public Vector2 Size
        {
            get
            {
                ForceUpdate();
                return _bounds.Size;
            }
        }

        private Font? _font;
        private uint _characterSize;
        private Color4 _color;
        private RenderShader? _shader;
        private string _text = string.Empty;
        private readonly ArrayList<Vertex3> _vertices = new(48);

        private bool _update = true;
        private Vector2 _cursor;
        private Box2 _bounds;
        private char _lastCharacter;

        public void Initialize() { }

        public void ResizeContext(Vector3 bounds) { }

        public void Append(char character)
        {
            _text += character;
            AppendInternal(character);
        }

        public void Append(string text)
        {
            _text += text;
            foreach (char c in text)
            {
                AppendInternal(c);
            }
        }

        public Vector2 GetCharacterPosition(int index)
        {
            Vector2 cursor = new(0, _characterSize);
            if (index == 0)
            {
                return cursor;
            }
            char lastCharacter = (char)0x0u;
            Glyph? lastGlyph = null;
            for (int i=0; i<index; ++i)
            {
                char character = _text[i];
                cursor.X += _font!.GetKerning(lastCharacter, character, _characterSize);
                lastCharacter = character;

                if (character == '\r')
                {
                    lastGlyph = null;
                    continue;
                }
                if (character == '\n')
                {
                    lastGlyph = null;
                    cursor = new(0, cursor.Y + _font!.GetLineSpacing(_characterSize));
                    continue;
                }
                if (character == '\t')
                {
                    lastGlyph = null;
                    cursor.X += 4 * _font!.GetWhitespace(_characterSize);
                    continue;
                }
                if (character == ' ')
                {
                    lastGlyph = null;
                    cursor.X += _font!.GetWhitespace(_characterSize);
                    continue;
                }

                var glyph = _font!.GetOrLoadGlyph(character, _characterSize);
                lastGlyph = glyph;
                if (i < index - 1)
                {
                    cursor.X += glyph.Advance;
                }
            }
            return cursor + new Vector2(lastGlyph?.Bounds.Max.X ?? 0, 0);
        }

        public void SetCharacterSize(uint characterSize)
        {
            if (characterSize != _characterSize)
            {
                _characterSize = characterSize;
                _update = true;
            }
        }

        public void SetColor(Color4 color)
        {
            if (color != _color)
            {
                _color = color;
                for (uint i=0; i<_vertices.Count; i++)
                {
                    _vertices.GetData()[i].Color = _color;
                }
            }
        }

        public void SetFont(Font font)
        {
            if (font != _font)
            {
                _font = font;
                _update = true;
            }
        }

        public void SetShader(RenderShader shader)
        {
            _shader = shader;
        }

        public void SetText(string text)
        {
            _text = text;
            _update = true;
        }

        public void Update(long delta) { }

        public void Draw(RenderTarget target, UiContext context)
        {
            ForceUpdate();
            target.Draw(
                _vertices.GetData(),
                PrimitiveType.Triangles,
                0,
                _vertices.Count, 
                _shader!,
                _font!.GetTexure(_characterSize));
        }

        private void ForceUpdate()
        {
            if (_update)
            {
                UpdateMesh();
                _update = false;
            }
        }

        private void UpdateMesh()
        {
            _cursor = new(0, _characterSize);
            _vertices.Clear();
            _bounds = new();
            foreach (var character in _text)
            {
                AppendInternal(character);
            }
        }

        private void AppendInternal(char character)
        {
            _cursor.X += _font!.GetKerning(_lastCharacter, character, _characterSize);
            _lastCharacter = character;

            if (character == '\r')
            {
                return;
            }
            if (character == '\n')
            {
                _cursor = new(0, _cursor.Y + _font!.GetLineSpacing(_characterSize));
                return;
            }
            if (character == '\t')
            {
                _cursor.X += 4 * _font!.GetWhitespace(_characterSize);
                return;
            }
            if (character == ' ')
            {
                _cursor.X += _font!.GetWhitespace(_characterSize);
                return;
            }

            var glyph = _font!.GetOrLoadGlyph(character, _characterSize);

            float top = _cursor.Y + glyph.Bounds.Min.Y;
            float bottom = _cursor.Y + glyph.Bounds.Max.Y;
            float left = _cursor.X + glyph.Bounds.Min.X;
            float right = _cursor.X + glyph.Bounds.Max.X;

            float texTop = glyph.TextureView.Min.Y;
            float texBottom = glyph.TextureView.Max.Y;
            float texLeft = glyph.TextureView.Min.X;
            float texRight = glyph.TextureView.Max.X;

            _vertices.Add(new(new(left, top, 0), _color, new(texLeft, texTop)));
            _vertices.Add(new(new(right, top, 0), _color, new(texRight, texTop)));
            _vertices.Add(new(new(left, bottom, 0), _color, new(texLeft, texBottom)));
            _vertices.Add(new(new(right, top, 0), _color, new(texRight, texTop)));
            _vertices.Add(new(new(left, bottom, 0), _color, new(texLeft, texBottom)));
            _vertices.Add(new(new(right, bottom, 0), _color, new(texRight, texBottom)));

            _cursor.X += glyph.Advance;
            _bounds.Inflate(new(right, bottom));
        }
    }
}
