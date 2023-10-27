using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements.Components
{
    public class TextComponent : GraphicsResource
    {
        public Vector2 Size => _text.Size;

        private readonly bool _supportLineBreaks;
        private readonly Text _text = new();

        public TextComponent(bool supportLineBreaks)
        {
            _supportLineBreaks = supportLineBreaks;
        }

        public void SetAttributes(ClassAttributes attributes)
        {
            _text.SetFont(attributes.FontFace!);
            _text.SetCharacterSize(attributes.FontSize);
            _text.SetColor(attributes.Color);
            _text.SetShader(attributes.Shader!);
            ResizeContext(
                new Vector2(attributes.Size.Width.GetMaxSize(), attributes.Size.Height.GetMaxSize()) 
                - attributes.LeftPadding 
                - attributes.RightPadding);
        }

        public void AppendText(string text)
        {
            _text.Append(text);
        }

        public Vector2 GetCharacterPosition(int index)
        {
            return _text.GetCharacterPosition(index);
        }

        public void ResizeContext(Vector2 context)
        {
            if (_supportLineBreaks)
            {
                _text.SetMaxWidth(context.X);
            }
        }

        public void SetText(string text)
        {
            _text.SetText(text);
        }

        public void Draw(IRenderTarget target, IUiContext context)
        {
            _text.Draw(target, context);
        }

        protected override void DisposeImpl()
        {
            _text.Dispose();
        }
    }
}
