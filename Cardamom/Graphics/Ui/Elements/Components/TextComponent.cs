using Cardamom.Planar;

namespace Cardamom.Graphics.Ui.Elements.Components
{
    internal class TextComponent
    {
        private readonly Text _text = new();

        public void SetAttributes(ClassAttributes attributes)
        {
            _text.SetFont(attributes.FontFace!.Element!);
            _text.SetCharacterSize(attributes.FontSize);
            _text.SetColor(attributes.Color);
            _text.SetShader(attributes.Shader!.Element!);
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
