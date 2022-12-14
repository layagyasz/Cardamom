using Cardamom.Graphics.TexturePacking;
using OpenTK.Mathematics;
using System.Text.Json.Serialization;

namespace Cardamom.Graphics.Ui
{
    public class ClassAttributes
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Alignment
        {
            Left,
            Center,
            Right
        }

        public float[] Margin { get; set; } = new float[4];
        public Vector2 LeftMargin => new(Margin[0], Margin[1]);
        public Vector2 RightMargin => new(Margin[2], Margin[3]);
        public float[] Padding { get; set; } = new float[4];
        public Vector2 LeftPadding => new(Padding[0], Padding[1]);
        public Vector2 RightPadding => new(Padding[2], Padding[3]);
        public ElementSizeDefinition Size { get; set; }

        public Color4[] BackgroundColor { get; set; } = new Color4[4];
        public Color4[] BorderColor { get; set; } = new Color4[4];
        public float[] BorderWidth { get; set; } = new float[4];
        public Vector2[] CornerRadius { get; set; } = new Vector2[4];
        public KeyedWrapper<RenderShader>? BackgroundShader { get; set; }
        public TextureSegment Texture { get; set; } = new(string.Empty, null, new(new(), new(1, 1)));

        public KeyedWrapper<Font>? FontFace { get; set; }
        public uint FontSize { get; set; } = 12;
        public Alignment Align { get; set; } = Alignment.Left;
        public Color4 Color { get; set; } = Color4.Black;
        public KeyedWrapper<RenderShader>? Shader { get; set; }

        public bool DisableScissor { get; set; }

        public class Builder
        {
            public float[]? Margin { get; set; }
            public float[]? Padding { get; set; }
            public ElementSizeDefinition? Size { get; set; }

            public Color4[]? BackgroundColor { get; set; }
            public Color4[]? BorderColor { get; set; }
            public float[]? BorderWidth { get; set; }
            public Vector2[]? CornerRadius { get; set; }
            public KeyedWrapper<RenderShader>? BackgroundShader { get; set; }
            public TextureSegment? Texture { get; set; }

            public KeyedWrapper<Font>? FontFace { get; set; }
            public uint? FontSize { get; set; }
            public Alignment? Align { get; set; }
            public Color4? Color { get; set; }
            public KeyedWrapper<RenderShader>? Shader { get; set; }
            public bool? DisableScissor { get; set; }

            public ClassAttributes Build(IEnumerable<Builder> ancestors) => new()
            {
                Margin = ExpandOrThrow(Inherit(ancestors.Select(x => x.Margin), Margin) ?? new float[4]),
                Padding = ExpandOrThrow(Inherit(ancestors.Select(x => x.Padding), Padding) ?? new float[4]),
                Size = Inherit(ancestors.Select(x => x.Size), Size) ?? new(),

                BackgroundColor = 
                    ExpandOrThrow(Inherit(ancestors.Select(x => x.BackgroundColor), BackgroundColor) ?? new Color4[4]),
                BorderColor = ExpandOrThrow(
                    Inherit(ancestors.Select(x => x.BorderColor), BorderColor) ?? new Color4[4]),
                BorderWidth = ExpandOrThrow(
                    Inherit(ancestors.Select(x => x.BorderWidth), BorderWidth) ?? new float[4]),
                CornerRadius = ExpandOrThrow(
                    Inherit(ancestors.Select(x => x.CornerRadius), CornerRadius) ?? new Vector2[4]),
                BackgroundShader = Inherit(ancestors.Select(x => x.BackgroundShader), BackgroundShader)!,
                Texture = 
                    Inherit(ancestors.Select(x => x.Texture), Texture) 
                        ?? new(string.Empty, null, new(new(), new(1, 1))),

                FontFace = Inherit(ancestors.Select(x => x.FontFace), FontFace),
                FontSize = Inherit(ancestors.Select(x => x.FontSize), FontSize) ?? 12,
                Align = Inherit(ancestors.Select(x => x.Align), Align) ?? Alignment.Left,
                Color = Inherit(ancestors.Select(x => x.Color), Color) ?? Color4.Black,
                Shader = Inherit(ancestors.Select(x => x.Shader), Shader)!,
                DisableScissor = Inherit(ancestors.Select(x => x.DisableScissor), DisableScissor) ?? false
            };

            private static T Inherit<T>(IEnumerable<T> ancestors, T child)
            {
                return ancestors.Aggregate((left, right) => left ?? right) ?? child;
            }

            private static T[] ExpandOrThrow<T>(T[] data)
            {
                if (data.Length == 4)
                {
                    return data;
                }
                if (data.Length == 1)
                {
                    return new T[] { data[0], data[0], data[0], data[0] };
                }
                throw new ArgumentException($"Array length must be either 1 or 4 but was {data.Length}.");
            }
        }
    }
}
