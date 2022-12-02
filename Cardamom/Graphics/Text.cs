using Cardamom.Collections;
using Cardamom.Graphics.Ui;
using Cardamom.Planar;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static System.Net.Mime.MediaTypeNames;

namespace Cardamom.Graphics
{
    public class Text : IRenderable
    {
        private Font? _font;
        private uint _characterSize;
        private Color4 _color;
        private Shader? _shader;
        private string _text = string.Empty;
        private readonly ArrayList<Vertex2> _vertices = new(48);

        private bool _update = true;
        private Vector2 _cursor;
        private char _lastCharacter;

        public void Initialize() { }

        public void Append(char character)
        {
            _text += character;
            AppendInternal(character);
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

        public void SetShader(Shader shader)
        {
            _shader = shader;
        }

        public void SetText(string text)
        {
            _text = text;
            _update = true;
        }

        public void Update(UiContext context, Transform2 transform, long delta) { }

        public void Draw(RenderTarget target, Transform2 transform)
        {
            if (_update)
            {
                UpdateMesh();
                _update = false;
            }

            target.Draw(
                _vertices.GetData(),
                PrimitiveType.Triangles,
                0,
                _vertices.Count, 
                transform, 
                _shader!,
                _font!.GetTexure(_characterSize));
        }

        private void UpdateMesh()
        {
            _cursor = new(0, _characterSize);
            _vertices.Clear();
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

            float top = _cursor.Y + glyph.Bounds.TopLeft.Y;
            float bottom = _cursor.Y + glyph.Bounds.TopLeft.Y + glyph.Bounds.Size.Y;
            float left = _cursor.X + glyph.Bounds.TopLeft.X;
            float right = _cursor.X + glyph.Bounds.TopLeft.X + glyph.Bounds.Size.X;

            float texTop = glyph.TextureView.TopLeft.Y;
            float texBottom = glyph.TextureView.TopLeft.Y + glyph.TextureView.Size.Y;
            float texLeft = glyph.TextureView.TopLeft.X;
            float texRight = glyph.TextureView.TopLeft.X + glyph.TextureView.Size.X;

            _vertices.Add(new(new(left, top), _color, new(texLeft, texTop)));
            _vertices.Add(new(new(right, top), _color, new(texRight, texTop)));
            _vertices.Add(new(new(left, bottom), _color, new(texLeft, texBottom)));
            _vertices.Add(new(new(right, top), _color, new(texRight, texTop)));
            _vertices.Add(new(new(left, bottom), _color, new(texLeft, texBottom)));
            _vertices.Add(new(new(right, bottom), _color, new(texRight, texBottom)));

            _cursor.X += glyph.Advance;
        }
    }
}
