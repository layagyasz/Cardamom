using Cardamom.Graphics.Ui.Controller;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
{
    public abstract class ClassedUiElement : BaseUiInteractiveElement
    {
        public Vector3 LeftMargin { get; private set; }
        public Vector3 RightMargin { get; private set; }
        public Vector3 LeftPadding { get; private set; }
        public Vector3 RightPadding { get; private set; }
        public Vector3 TrueSize { get; private set; }
        public Vector3 InternalSize => TrueSize - LeftPadding - RightPadding;
        public override Vector3 Size => TrueSize + LeftMargin + RightMargin;

        private readonly Class _class;

        public ClassedUiElement(Class @class, IController controller)
            : base(controller)
        {
            _class = @class;
        }

        public virtual void SetAttributes(ClassAttributes attributes)
        {
            LeftMargin = new(attributes.LeftMargin.X, attributes.LeftMargin.Y, 0);
            RightMargin = new(attributes.RightMargin.X, attributes.RightMargin.Y, 0);
            LeftPadding = new(attributes.LeftPadding.X, attributes.LeftPadding.Y, 0);
            RightPadding = new(attributes.RightPadding.X, attributes.RightPadding.Y, 0);
            TrueSize = new(attributes.Size.X, attributes.Size.Y, 0);
        }

        public void SetState(Class.State state)
        {
            SetAttributes(_class.Get(state));
        }
    }
}
