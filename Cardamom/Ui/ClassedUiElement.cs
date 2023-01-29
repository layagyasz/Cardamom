using Cardamom.Ui.Controller.Element;
using OpenTK.Mathematics;

namespace Cardamom.Ui
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
        public ElementSizeDefinition SizeDefinition { get; private set; }
        public bool DisableScissor { get; private set; }


        private readonly Class _class;

        public ClassedUiElement(Class @class, IElementController controller)
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
            SizeDefinition = attributes.Size;
            DisableScissor = attributes.DisableScissor;

            if (SizeDefinition.Width.Mode == ElementSizeDefinition.Mode.Static)
            {
                TrueSize = new(SizeDefinition.Width.Size, TrueSize.Y, 0);
            }
            if (SizeDefinition.Height.Mode == ElementSizeDefinition.Mode.Static)
            {
                TrueSize = new(TrueSize.X, SizeDefinition.Height.Size, 0);
            }
        }

        public void SetDynamicSize(Vector3 size)
        {
            Vector3 newSize = TrueSize;
            if (SizeDefinition.Width.Mode == ElementSizeDefinition.Mode.DynamicContents)
            {
                newSize.X =
                    MathHelper.Clamp(size.X, SizeDefinition.Width.MinimumSize, SizeDefinition.Width.MaximumSize);
            }
            if (SizeDefinition.Height.Mode == ElementSizeDefinition.Mode.DynamicContents)
            {
                newSize.Y =
                    MathHelper.Clamp(size.Y, SizeDefinition.Height.MinimumSize, SizeDefinition.Height.MaximumSize);
            }
            if (Vector3.DistanceSquared(TrueSize, newSize) > float.Epsilon)
            {
                TrueSize = newSize;
                SetDyamicSizeImpl(newSize.Xy);
            }
        }

        public void SetState(Class.State state)
        {
            SetAttributes(_class.Get(state));
        }

        protected abstract void SetDyamicSizeImpl(Vector2 size);
    }
}
