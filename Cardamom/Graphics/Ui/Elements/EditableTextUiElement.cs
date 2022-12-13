using Cardamom.Graphics.Ui.Controller;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements
{
    public class EditableTextUiElement : TextUiElement
    {
        private static readonly Texture BLANK = Texture.Create(new(1, 1), Color4.White);
        private static readonly float LEFT_CURSOR_BUFFER = 1;
        private static readonly float RIGHT_CURSOR_BUFFER = 20;
        private static readonly int CURSOR_PERIOD = 1000;

        private readonly VertexArray _cursor = new(PrimitiveType.Triangles, 6);
        private Vector3 _offset;
        private bool _cursorActive;
        private int _cursorPeriod;
        private Vector3 _cursorPosition;
        private Shader? _cursorShader;

        public EditableTextUiElement(Class @class, IController controller)
            : base(@class, controller) { }

        public override void SetAttributes(ClassAttributes attributes)
        {
            base.SetAttributes(attributes);
            _cursorShader = attributes.Shader!.Element;
            _cursor[0] = new(new(), attributes.Color, new());
            _cursor[1] = new(new(1, 0, 0), attributes.Color, new(1, 0));
            _cursor[2] = new(new(0, attributes.FontSize, 0), attributes.Color, new(0, 1));
            _cursor[3] = _cursor[2];
            _cursor[4] = _cursor[1];
            _cursor[5] = new(new(1, attributes.FontSize, 0), attributes.Color, new(1, 1));
        }

        public void SetCursorActive(bool active)
        {
            _cursorActive = active;
        }

        public void SetCursor(int index)
        {
            _cursorPosition = new(_textComponent.GetCharacterPosition(index).X, 0, 0);
            _cursorPeriod = 0;

            var windowMin = -_offset.X + RIGHT_CURSOR_BUFFER;
            var windowMax = InternalSize.X - _offset.X - LEFT_CURSOR_BUFFER;
            if (_cursorPosition.X < windowMin || _cursorPosition.X > windowMax)
            {
                _offset = new(Math.Min(InternalSize.X - _cursorPosition.X - LEFT_CURSOR_BUFFER, 0), 0, 0);
            }
        }

        public override void Draw(RenderTarget target)
        {
            if (Visible)
            {
                target.PushTranslation(Position + LeftMargin);
                _rectComponent.Draw(target);
                target.PushTranslation(LeftPadding);
                target.PushScissor(new(new(), InternalSize));
                target.PushTranslation(_offset);
                _textComponent.Draw(target);
                if (_cursorActive && _cursorPeriod < CURSOR_PERIOD >> 1)
                {
                    target.PushTranslation(_cursorPosition);
                    target.Draw(_cursor, 0, _cursor.Length, _cursorShader!, BLANK);
                    target.PopTransform();
                }
                target.PopTransform();
                target.PopScissor();
                target.PopTransform();
                target.PopTransform();
            }
        }

        public override void Update(UiContext context, long delta)
        {
            base.Update(context, delta);
            _cursorPeriod = (_cursorPeriod + (int)delta) % CURSOR_PERIOD;
        }
    }
}
