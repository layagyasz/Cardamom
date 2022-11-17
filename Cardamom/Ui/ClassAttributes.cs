using SFML.Graphics;
using SFML.System;

namespace Cardamom.Ui
{
    public class ClassAttributes
    {
        public byte[] Margin { get; set; } = new byte[4];
        public Vector2f LeftMargin => new(Margin[0], Margin[1]);
        public Vector2f RightMargin => new(Margin[2], Margin[3]);
        public byte[] Padding { get; set; } = new byte[4];
        public Vector2f LeftPadding => new(Padding[0], Padding[1]);
        public Vector2f RightPadding => new(Padding[2], Padding[3]);
        public Vector2f Size { get; set; }
        public Font? FontFace { get; set; }
        public Color[] BackgroundColor { get; set; } = new Color[4];
        public Color[] BorderColor { get; set; } = new Color[4];
        public byte[] BorderWidth { get; set; } = new byte[4];

        public class Builder
        {
            public byte[]? Margin { get; set; }
            public byte[]? Padding { get; set; }
            public Vector2f? Size { get; set; }
            public Font? FontFace { get; set; }
            public Color[]? BackgroundColor { get; set; }
            public Color[]? BorderColor { get; set; }
            public byte[]? BorderWidth { get; set; }

            public ClassAttributes Build(IEnumerable<Builder> ancestors) => new()
            {
                Margin = Precondition.HasSize<byte[], byte>(
                    Inherit(ancestors.Select(x => x.Margin), Margin) ?? new byte[4], 4),
                Padding = Precondition.HasSize<byte[], byte>(
                    Inherit(ancestors.Select(x => x.Padding), Padding) ?? new byte[4], 4),
                Size = Inherit(ancestors.Select(x => x.Size), Size) ?? new Vector2f(),
                FontFace = Inherit(ancestors.Select(x => x.FontFace), FontFace),
                BackgroundColor = Precondition.HasSize<Color[], Color>(
                    Inherit(ancestors.Select(x => x.BackgroundColor), BackgroundColor) ?? new Color[4], 4),
                BorderColor = Precondition.HasSize<Color[], Color>(
                    Inherit(ancestors.Select(x => x.BorderColor), BorderColor) ?? new Color[4], 4),
                BorderWidth = Precondition.HasSize<byte[], byte>(
                    Inherit(ancestors.Select(x => x.BorderWidth), BorderWidth) ?? new byte[4], 4)
            };

            private static T Inherit<T>(IEnumerable<T> ancestors, T child)
            {
                return ancestors.Aggregate((left, right) => right ?? left) ?? child;
            }
        }
    }
}
