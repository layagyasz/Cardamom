﻿using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.Ui.Elements
{
    public class UiWrapper : SimpleUiElement, IUiContainer
    {
        public EventHandler<ElementEventArgs>? ElementAdded { get; set; }
        public EventHandler<ElementEventArgs>? ElementRemoved { get; set; }

        public IUiElement Element { get; }

        public UiWrapper(Class @class, IUiElement element)
            : base(@class, element.Controller)
        {
            Element = element;
            Element.Parent = this;
        }

        public void Add(IUiElement element)
        {
            throw new NotSupportedException();
        }

        public override void Initialize()
        {
            base.Initialize();
            Element.Initialize();
        }

        public IEnumerator<IUiElement> GetEnumerator()
        {
            yield return Element;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Draw(RenderTarget target, UiContext context)
        {
            if (Visible)
            {
                base.Draw(target, context);
                target.PushTranslation(Position + LeftMargin + LeftPadding);
                if (!DisableScissor)
                {
                    target.PushScissor(new(new(), InternalSize));
                }
                Box3 bounds = new();
                Element.Draw(target, context);
                bounds.Inflate(Element.Position + Element.Size);
                SetDynamicSize(bounds.Size + LeftPadding + RightPadding);
                if (!DisableScissor)
                {
                    target.PopScissor();
                }
                target.PopModelMatrix();
            }
        }

        public void Remove(IUiElement element)
        {
            throw new NotSupportedException();
        }

        public override void Update(long delta)
        {
            Element.Update(delta);
        }
    }
}