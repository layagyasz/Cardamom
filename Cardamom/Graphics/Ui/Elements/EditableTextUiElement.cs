using Cardamom.Graphics.Ui.Controller;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements
{
    public class EditableTextUiElement : TextUiElement
    {
        private static readonly Texture s_Blank = Texture.Create(new(1, 1), Color4.White);
        private static readonly float s_LeftCursorBuffer = 1;
        private static readonly float s_RightCursorBuffer = 20;
        private static readonly int s_CursorPeriod = 1000;

        private readonly VertexArray _cursor = new(PrimitiveType.Triangles, 6);
        private Vector3 _offset;
        private bool _cursorActive;
        private int _cursorPeriod;
        private Vector3 _cursorPosition;
        private RenderShader? _cursorShader;

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

            var windowMin = -_offset.X + s_RightCursorBuffer;
            var windowMax = InternalSize.X - _offset.X - s_LeftCursorBuffer;
            if (_cursorPosition.X < windowMin || _cursorPosition.X > windowMax)
            {
                _offset = new(Math.Min(InternalSize.X - _cursorPosition.X - s_LeftCursorBuffer, 0), 0, 0);
            }
        }

        public override void Draw(RenderTarget target, UiContext context)
        {
            if (Visible)
            {
                target.PushTranslation(Position + LeftMargin);
                context.Register(this);
                _rectComponent.Draw(target);
                target.PushTranslation(LeftPadding + _alignAdjust);
                if (!DisableScissor)
                {
                    target.PushScissor(new(new(), InternalSize));
                }
                target.PushTranslation(_offset);
                _textComponent.Draw(target, context);
                if (_cursorActive && _cursorPeriod < s_CursorPeriod >> 1)
                {
                    target.PushTranslation(_cursorPosition);
                    target.Draw(_cursor, 0, _cursor.Length, new(_cursorShader!, s_Blank));
                    target.PopViewMatrix();
                }
                target.PopViewMatrix();
                if (!DisableScissor)
                {
                    target.PopScissor();
                }
                target.PopViewMatrix();
                target.PopViewMatrix();

                SetDynamicSize(LeftPadding + RightPadding + new Vector3(_textComponent.Size));
            }
        }

        public override void Update(long delta)
        {
            base.Update(delta);
            _cursorPeriod = (_cursorPeriod + (int)delta) % s_CursorPeriod;
        }
    }
}
