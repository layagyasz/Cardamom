using Cardamom.Ui.Controller;

namespace Cardamom.Ui
{
    public abstract class ClassedUiElement : BaseUiInteractiveElement
    {
        private readonly Class _class;
        private Class.State _state;

        public ClassedUiElement(Class @class, IController controller)
            : base(controller)
        {
            _class = @class;
            _state = Class.State.NONE;
        }

        public abstract void SetAttributes(ClassAttributes attributes);

        public void SetState(Class.State state)
        {
            _state = state;
            SetAttributes(_class.Get(state));
        }
    }
}
