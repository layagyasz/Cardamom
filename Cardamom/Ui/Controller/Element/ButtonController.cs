﻿namespace Cardamom.Ui.Controller.Element
{
    public class ButtonController : ClassedUiElementController<ClassedUiElement>
    {
        public override bool HandleMouseButtonClicked(MouseButtonClickEventArgs e)
        {
            Clicked?.Invoke(this, e);
            return true;
        }

        public override bool HandleMouseEntered()
        {
            SetHover(true);
            return true;
        }

        public override bool HandleMouseLeft()
        {
            SetHover(false);
            return true;
        }
    }
}
