using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.Graphics.Ui
{
    public class ClassAttributes
    {
        public float[] Margin { get; set; } = new float[4];
        public Vector2 LeftMargin => new(Margin[0], Margin[1]);
        public Vector2 RightMargin => new(Margin[2], Margin[3]);
        public float[] Padding { get; set; } = new float[4];
        public Vector2 LeftPadding => new(Padding[0], Padding[1]);
        public Vector2 RightPadding => new(Padding[2], Padding[3]);
        public Vector2 Size { get; set; }
        // Implement fonts with SharpFont
        //public KeyedWrapper<Font>? FontFace { get; set; }
        public Color4[] BackgroundColor { get; set; } = new Color4[4];
        public Color4[] BorderColor { get; set; } = new Color4[4];
        public float[] BorderWidth { get; set; } = new float[4];
        public KeyedWrapper<Shader>? Shader { get; set; }

        public class Builder
        {
            public float[]? Margin { get; set; }
            public float[]? Padding { get; set; }
            public Vector2? Size { get; set; }
            
            // Implement fonts with SharpFont
            // public KeyedWrapper<Font>? FontFace { get; set; }
            public Color4[]? BackgroundColor { get; set; }
            public Color4[]? BorderColor { get; set; }
            public float[]? BorderWidth { get; set; }
            public KeyedWrapper<Shader>? Shader { get; set; }

            public ClassAttributes Build(IEnumerable<Builder> ancestors) => new()
            {
                Margin = Precondition.HasSize<float[], float>(
                    Inherit(ancestors.Select(x => x.Margin), Margin) ?? new float[4], 4),
                Padding = Precondition.HasSize<float[], float> (
                    Inherit(ancestors.Select(x => x.Padding), Padding) ?? new float[4], 4),
                Size = Inherit(ancestors.Select(x => x.Size), Size) ?? new Vector2(),
                // Implement font with 
                // FontFace = Inherit(ancestors.Select(x => x.FontFace), FontFace),
                BackgroundColor = Precondition.HasSize<Color4[], Color4>(
                    Inherit(ancestors.Select(x => x.BackgroundColor), BackgroundColor) ?? new Color4[4], 4),
                BorderColor = Precondition.HasSize<Color4[], Color4>(
                    Inherit(ancestors.Select(x => x.BorderColor), BorderColor) ?? new Color4[4], 4),
                BorderWidth = Precondition.HasSize<float[], float>(
                    Inherit(ancestors.Select(x => x.BorderWidth), BorderWidth) ?? new float[4], 4),
                Shader = Inherit(ancestors.Select(x => x.Shader), null)!
            };

            private static T Inherit<T>(IEnumerable<T> ancestors, T child)
            {
                return ancestors.Aggregate((left, right) => right ?? left) ?? child;
            }
        }
    }
}
