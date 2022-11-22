using Cardamom.Graphics;
using Cardamom.Planar;
using Cardamom.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
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

        public override void Draw(RenderTarget target, Transform2 transform)
        {
            base.Draw(target, transform);
            transform.Translate(Position + LeftMargin + LeftPadding);
            for (int i=(int)Index; i<_endIndex; ++i)
            {
                _elements[i].Draw(target, transform);
            }
        }

        public override void Update(UiContext context, Transform2 transform, long delta)
        {
            base.Update(context, transform, delta);
            transform.Translate(Position + LeftMargin + LeftPadding);
            ComputeShownElements();
            for (int i = (int)Index; i < _endIndex; ++i)
            {
                _elements[i].Update(context, transform, delta);
            }
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
