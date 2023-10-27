using Cardamom.Collections;
using Cardamom.Ui;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public class Text : GraphicsResource, IRenderable
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
        private float _maxWidth = float.PositiveInfinity;

        private string _text = string.Empty;
        private readonly ArrayList<Vector2> _positions = new(12);
        private readonly ArrayList<Vertex3> _vertices = new(48);
        private readonly VertexBuffer<Vertex3> _buffer = new(PrimitiveType.Triangles);

        private bool _updateVertices = true;
        private bool _updateBuffer = true;
        private Vector2 _cursor;
        private Box2 _bounds;
        private int _lastWhitespace;
        private int _lastBreak;
        private char _lastCharacter;

        public void Initialize() { }

        public void ResizeContext(Vector3 bounds) { }

        public void Append(char character)
        {
            _text += character;
            AppendInternal(_text, _text.Length - 1);
        }

        public void Append(string text)
        {
            _text += text;
            for (int i = 0; i < _text.Length; ++i)
            {
                AppendInternal(_text, i);
            }
        }

        public Vector2 GetCharacterPosition(int index)
        {
            if (index == 0)
            {
                return new(0, _characterSize);
            }
            return _positions[index - 1];
        }

        public void SetCharacterSize(uint characterSize)
        {
            if (characterSize != _characterSize)
            {
                _characterSize = characterSize;
                _updateVertices = true;
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
                _updateBuffer = true;
            }
        }

        public void SetFont(Font font)
        {
            if (font != _font)
            {
                _font = font;
                _updateVertices = true;
            }
        }

        public void SetMaxWidth(float maxWidth)
        {
            _maxWidth = maxWidth;
            _updateVertices = true;
        }

        public void SetShader(RenderShader shader)
        {
            _shader = shader;
        }

        public void SetText(string text)
        {
            _text = text;
            _updateVertices = true;
        }

        public void Update(long delta) { }

        public void Draw(IRenderTarget target, IUiContext context)
        {
            ForceUpdate();
            if (_updateBuffer)
            {
                _buffer.Buffer(_vertices.GetData(), 0, _vertices.Count);
                _updateBuffer = false;
            }
            target.Draw(
                _buffer,
                0,
                _buffer.Length, 
                new(BlendMode.Alpha, _shader!, _font!.GetTexure(_characterSize)!));
        }

        private void ForceUpdate()
        {
            if (_updateVertices)
            {
                UpdateMesh();
                _updateVertices = false;
            }
        }

        private void UpdateMesh()
        {
            _cursor = new(0, _characterSize);
            _positions.Clear();
            _vertices.Clear();
            _bounds = new();
            for (int i=0; i<_text.Length;++i)
            {
                AppendInternal(_text, i);
            }
            _updateBuffer = true;
        }

        private void AppendInternal(string value, int index)
        {
            var character = value[index];
            _cursor.X += _font!.GetKerning(_lastCharacter, character, _characterSize);
            _lastCharacter = character;

            if (char.IsWhiteSpace(character))
            {
                if (character == '\r')
                {
                    _lastWhitespace = index;
                    _cursor = new(0, _cursor.Y);
                }
                if (character == '\n')
                {
                    _lastWhitespace = index;
                    _cursor = new(0, _cursor.Y + _font!.GetLineSpacing(_characterSize));
                }
                if (character == '\t')
                {
                    _lastWhitespace = index;
                    _cursor.X += 4 * _font!.GetWhitespace(_characterSize);
                }
                if (character == ' ')
                {
                    _lastWhitespace = index;
                    _cursor.X += _font!.GetWhitespace(_characterSize);
                }
                _positions.Add(_cursor);
                return;
            }

            var glyph = _font!.GetOrLoadGlyph(character, _characterSize);

            if (_cursor.X + glyph.Advance > _maxWidth)
            {
                if (_lastWhitespace > _lastBreak)
                {
                    _lastBreak = _lastWhitespace;
                    _positions.Trim(index - _lastWhitespace);
                    _vertices.Trim(6 * (index - _lastWhitespace));
                    _cursor = new(0, _cursor.Y + _font!.GetLineSpacing(_characterSize));
                    for (int i=_lastWhitespace + 1; i<=index; ++i)
                    {
                        AppendInternal(value, i);
                    }
                }
            }

            float top = _cursor.Y + glyph.Bounds.Min.Y;
            float bottom = _cursor.Y + glyph.Bounds.Max.Y;
            float left = _cursor.X + glyph.Bounds.Min.X;
            float right = _cursor.X + glyph.Bounds.Max.X;

            float texTop = glyph.TextureView.Min.Y;
            float texBottom = glyph.TextureView.Max.Y;
            float texLeft = glyph.TextureView.Min.X;
            float texRight = glyph.TextureView.Max.X;

            _positions.Add(new(right, bottom));
            _vertices.Add(new(new(left, top, 0), _color, new(texLeft, texTop)));
            _vertices.Add(new(new(right, top, 0), _color, new(texRight, texTop)));
            _vertices.Add(new(new(left, bottom, 0), _color, new(texLeft, texBottom)));
            _vertices.Add(new(new(right, top, 0), _color, new(texRight, texTop)));
            _vertices.Add(new(new(left, bottom, 0), _color, new(texLeft, texBottom)));
            _vertices.Add(new(new(right, bottom, 0), _color, new(texRight, texBottom)));

            _cursor.X += glyph.Advance;
            _bounds.Inflate(new(right, bottom));
        }

        protected override void DisposeImpl()
        {
            _buffer.Dispose();
        }
    }
}
