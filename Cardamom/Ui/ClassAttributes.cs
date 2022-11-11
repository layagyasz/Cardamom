using SFML.Graphics;
using SFML.System;

namespace Cardamom.Ui
{
    public class ClassAttributes
    {
        public Vector2i Margin { get; set; }
        public Vector2i Padding { get; set; }
        public Vector2i Size { get; set; }
        public Font? FontFace { get; set; }
        public Color[] BackgroundColor { get; set; } = new Color[4];
        public Color[] BorderColor { get; set; } = new Color[4];
        public byte BorderWidth { get; set;}

        public class Builder
        {
            public Vector2i? Margin { get; set; }
            public Vector2i? Padding { get; set; }
            public Vector2i? Size { get; set; }
            public Font? FontFace { get; set; }
            public Color[]? BackgroundColor { get; set; }
            public Color[]? BorderColor { get; set; }
            public byte? BorderWidth { get; set; }

            public ClassAttributes Build(IEnumerable<Builder> ancestors) => new()
            {
                Margin = Inherit(ancestors.Select(x => x.Margin), Margin) ?? new Vector2i(),
                Padding = Inherit(ancestors.Select(x => x.Padding), Padding) ?? new Vector2i(),
                Size = Inherit(ancestors.Select(x => x.Size), Size) ?? new Vector2i(),
                FontFace = Inherit(ancestors.Select(x => x.FontFace), FontFace),
                BackgroundColor = Precondition.HasSize(
                    Inherit(ancestors.Select(x => x.BackgroundColor), BackgroundColor) ?? new Color[4], 4),
                BorderColor = Precondition.HasSize(
                    Inherit(ancestors.Select(x => x.BorderColor), BorderColor) ?? new Color[4], 4),
                BorderWidth = Inherit(ancestors.Select(x => x.BorderWidth), BorderWidth) ?? 0
            };

            private static T Inherit<T>(IEnumerable<T> ancestors, T child)
            {
                return ancestors.Aggregate((left, right) => right ?? left) ?? child;
            }
        }
    }
}
