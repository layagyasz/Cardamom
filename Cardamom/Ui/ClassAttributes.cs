using SFML.Graphics;
using SFML.System;

namespace Cardamom.Ui
{
    public class ClassAttributes
    {
        public int[] Margin { get; set; } = new int[4];
        public Vector2f LeftMargin => new(Margin[0], Margin[1]);
        public Vector2f RightMargin => new(Margin[2], Margin[3]);
        public int[] Padding { get; set; } = new int[4];
        public Vector2f LeftPadding => new(Padding[0], Padding[1]);
        public Vector2f RightPadding => new(Padding[2], Padding[3]);
        public Vector2f Size { get; set; }
        public KeyedWrapper<Font>? FontFace { get; set; }
        public Color[] BackgroundColor { get; set; } = new Color[4];
        public Color[] BorderColor { get; set; } = new Color[4];
        public int[] BorderWidth { get; set; } = new int[4];

        public class Builder
        {
            public int[]? Margin { get; set; }
            public int[]? Padding { get; set; }
            public Vector2f? Size { get; set; }
            public KeyedWrapper<Font>? FontFace { get; set; }
            public Color[]? BackgroundColor { get; set; }
            public Color[]? BorderColor { get; set; }
            public int[]? BorderWidth { get; set; }

            public ClassAttributes Build(IEnumerable<Builder> ancestors) => new()
            {
                Margin = Precondition.HasSize<int[], int>(
                    Inherit(ancestors.Select(x => x.Margin), Margin) ?? new int[4], 4),
                Padding = Precondition.HasSize<int[], int>(
                    Inherit(ancestors.Select(x => x.Padding), Padding) ?? new int[4], 4),
                Size = Inherit(ancestors.Select(x => x.Size), Size) ?? new Vector2f(),
                FontFace = Inherit(ancestors.Select(x => x.FontFace), FontFace),
                BackgroundColor = Precondition.HasSize<Color[], Color>(
                    Inherit(ancestors.Select(x => x.BackgroundColor), BackgroundColor) ?? new Color[4], 4),
                BorderColor = Precondition.HasSize<Color[], Color>(
                    Inherit(ancestors.Select(x => x.BorderColor), BorderColor) ?? new Color[4], 4),
                BorderWidth = Precondition.HasSize<int[], int>(
                    Inherit(ancestors.Select(x => x.BorderWidth), BorderWidth) ?? new int[4], 4)
            };

            private static T Inherit<T>(IEnumerable<T> ancestors, T child)
            {
                return ancestors.Aggregate((left, right) => right ?? left) ?? child;
            }
        }
    }
}
