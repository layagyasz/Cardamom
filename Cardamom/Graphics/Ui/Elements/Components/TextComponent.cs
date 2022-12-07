using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements.Components
{
    public class TextComponent
    {
        private readonly Text _text = new();

        public void SetAttributes(ClassAttributes attributes)
        {
            _text.SetFont(attributes.FontFace!.Element!);
            _text.SetCharacterSize(attributes.FontSize);
            _text.SetColor(attributes.Color);
            _text.SetShader(attributes.Shader!.Element!);
        }

        public void AppendText(string text)
        {
            _text.Append(text);
        }

        public Vector2 GetCharacterPosition(int index)
        {
            return _text.GetCharacterPosition(index);
        }

        public void SetText(string text)
        {
            _text.SetText(text);
        }

        public void Draw(RenderTarget target)
        {
            _text.Draw(target);
        }
    }
}
