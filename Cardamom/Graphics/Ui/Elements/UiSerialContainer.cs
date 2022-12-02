using Cardamom.Graphics.Ui.Controller;
using Cardamom.Planar;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui.Elements
{
    public class UiSerialContainer : SimpleUiElement, IEnumerable<IUiElement>
    {
        public uint Index { get; set; }

        private readonly List<IUiElement> _elements = new();
        private uint _endIndex;

        public UiSerialContainer(Class @class, IController controller)
            : base(@class, controller) { }

        public void Add(IUiElement element)
        {
            _elements.Add(element);
            element.Parent = this;
        }

        public IEnumerator<IUiElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Draw(RenderTarget target)
        {
            base.Draw(target);
            target.PushTranslation(Position + LeftMargin + LeftPadding);
            for (int i=(int)Index; i<_endIndex; ++i)
            {
                _elements[i].Draw(target);
            }
            target.PopTransform();
        }

        public override void Update(UiContext context, long delta)
        {
            base.Update(context, delta);
            context.PushTranslation(Position + LeftMargin + LeftPadding);
            ComputeShownElements();
            for (int i = (int)Index; i < _endIndex; ++i)
            {
                _elements[i].Update(context, delta);
            }
            context.PopTransform();
        }
        
        private void ComputeShownElements()
        {
            float total = 0;
            bool ended = false;
            for (uint i=0; i<_elements.Count; ++i)
            {
                if (!ended && i >= Index)
                {
                    _elements[(int)i].Position = new Vector2(0, total);
                    total += _elements[(int)i].Size.Y;
                    if (total + LeftPadding.Y + RightPadding.Y > Size.Y)
                    {
                        _endIndex = i;
                        _elements[(int)i].Visible = false;
                        ended = true;
                    }
                    else
                    {
                        _elements[(int)i].Visible = true;
                    }
                }
                else
                {
                    _elements[(int)i].Visible = false;
                }
            }
            _endIndex = (uint)_elements.Count;
        }
    }
}
