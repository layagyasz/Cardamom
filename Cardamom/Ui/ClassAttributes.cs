using Cardamom.Graphics;
using Cardamom.Graphics.TexturePacking;
using Cardamom.Json;
using OpenTK.Mathematics;
using System.Collections;
using System.Text.Json.Serialization;

namespace Cardamom.Ui
{
    public class ClassAttributes : GraphicsResource
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum HorizontalAlignment
        {
            Left,
            Center,
            Right
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum VerticalAlignment
        {
            Top,
            Center,
            Bottom
        }

        public float[] Margin { get; }
        public Vector2 LeftMargin => new(Margin[0], Margin[1]);
        public Vector2 RightMargin => new(Margin[2], Margin[3]);
        public float[] Padding { get; }
        public Vector2 LeftPadding => new(Padding[0], Padding[1]);
        public Vector2 RightPadding => new(Padding[2], Padding[3]);
        public ElementSizeDefinition Size { get; }

        public Color4[] BackgroundColor { get; }
        public RenderShader BackgroundShader { get; }
        public TextureSegment Texture { get; }

        public Font? FontFace { get; }
        public uint FontSize { get; }
        public HorizontalAlignment Align { get; }
        public VerticalAlignment VerticalAlign { get; }
        public Color4 Color { get; }
        public RenderShader? Shader { get; }
        
        public bool DisableScissor { get; }
        public bool DisableDraw { get; }

        private UniformBuffer? _uniforms;

        private ClassAttributes(
            float[] margin,
            float[] padding, 
            ElementSizeDefinition size,
            Color4[] backgroundColor,
            RenderShader backgroundShader,
            TextureSegment texture,
            Font? fontFace, 
            uint fontSize, 
            HorizontalAlignment align, 
            VerticalAlignment verticalAlign,
            Color4 color, 
            RenderShader? shader,
            bool disableScissor,
            bool disableDraw,
            UniformBuffer uniforms)
        {
            Margin = margin;
            Padding = padding;
            Size = size;
            BackgroundColor = backgroundColor;
            BackgroundShader = backgroundShader;
            Texture = texture;
            FontFace = fontFace;
            FontSize = fontSize;
            Align = align;
            VerticalAlign = verticalAlign;
            Color = color;
            Shader = shader;
            DisableScissor = disableScissor;
            DisableDraw = disableDraw;
            _uniforms = uniforms;
        }

        protected override void DisposeImpl()
        {
            _uniforms!.Dispose();
            _uniforms = null;
        }

        public UniformBuffer GetUniforms()
        {
            return _uniforms!;
        }

        public override bool Equals(object? @object)
        {
            if (@object is ClassAttributes other)
            {
                return Margin.SequenceEqual(other.Margin)
                    && Padding.SequenceEqual(other.Padding)
                    && Size.Equals(other.Size)
                    && BackgroundColor.SequenceEqual(other.BackgroundColor)
                    && BackgroundShader.Equals(other.BackgroundShader)
                    && Texture.Equals(other.Texture)
                    && Equals(FontFace, other.FontFace)
                    && FontSize == other.FontSize
                    && Align == other.Align
                    && VerticalAlign == other.VerticalAlign
                    && Color == other.Color
                    && Equals(Shader, other.Shader)
                    && DisableScissor == other.DisableScissor
                    && DisableDraw == other.DisableDraw
                    && Equals(_uniforms, other._uniforms);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                HashCode.Combine(
                    HashArray(Margin),
                    HashArray(Padding),
                    Size.GetHashCode(),
                    HashArray(BackgroundColor),
                    BackgroundShader.GetHashCode(),
                    Texture.GetHashCode(),
                    FontFace?.GetHashCode()), 
                HashCode.Combine(
                    FontSize.GetHashCode(),
                    Align.GetHashCode(),
                    VerticalAlign.GetHashCode(),
                    Color.GetHashCode(),
                    Shader?.GetHashCode(),
                    DisableScissor.GetHashCode(),
                    DisableDraw.GetHashCode(),
                    _uniforms?.GetHashCode()));
        }

        private static int HashArray<T>(T[] array)
        {
            return ((IStructuralEquatable)array).GetHashCode(EqualityComparer<T>.Default);
        }

        public class Builder
        {
            public float[]? Margin { get; set; }
            public float[]? Padding { get; set; }
            public ElementSizeDefinition? Size { get; set; }

            public Color4[]? BackgroundColor { get; set; }
            public Color4? ForegroundColor { get; set; }
            public Color4[]? BorderColor { get; set; }
            public float[]? BorderWidth { get; set; }
            public Vector2[]? CornerRadius { get; set; }
            [JsonConverter(typeof(ReferenceJsonConverter))]
            public KeyedWrapper<RenderShader>? BackgroundShader { get; set; }
            [JsonConverter(typeof(ReferenceJsonConverter))]
            public TextureSegment? Texture { get; set; }
            [JsonConverter(typeof(ReferenceJsonConverter))]
            public KeyedWrapper<Font>? FontFace { get; set; }
            public uint? FontSize { get; set; }
            public HorizontalAlignment? Align { get; set; }
            public VerticalAlignment? VerticalAlign { get; set; }
            public Color4? Color { get; set; }
            [JsonConverter(typeof(ReferenceJsonConverter))]
            public KeyedWrapper<RenderShader>? Shader { get; set; }
            public bool? DisableScissor { get; set; }

            public ClassAttributes Build(Class.BuilderResources resources, IEnumerable<Builder> ancestors)
            {
                var backgroundShader = Inherit(ancestors.Select(x => x.BackgroundShader), BackgroundShader)!.Element!;
                TextureSegment texture = Inherit(ancestors.Select(x => x.Texture), Texture)
                    ?? new(string.Empty, null, new(new(), new(1, 1)));
                var backgroundColor =
                    ExpandOrThrow(Inherit(ancestors.Select(x => x.BackgroundColor), BackgroundColor) ?? new Color4[4]);
                var foregroundColor = 
                    Inherit(ancestors.Select(x => x.ForegroundColor), ForegroundColor) ?? Color4.White;
                var borderColor = 
                    ExpandOrThrow(Inherit(ancestors.Select(x => x.BorderColor), BorderColor) ?? new Color4[4]);
                var borderWidth = 
                    ExpandOrThrow(Inherit(ancestors.Select(x => x.BorderWidth), BorderWidth) ?? new float[4]);
                var cornerRadius = 
                    ExpandOrThrow(Inherit(ancestors.Select(x => x.CornerRadius), CornerRadius) ?? new Vector2[4]);
                var uniforms = resources.Get(
                    new Class.UniformBufferKey(
                        backgroundShader,
                        texture.Texture == null ? 0 : 1,
                        foregroundColor, 
                        borderWidth,
                        borderColor,
                        cornerRadius));
                return resources.Dedupe(
                    new(
                        ExpandOrThrow(Inherit(ancestors.Select(x => x.Margin), Margin) ?? new float[4]),
                        ExpandOrThrow(Inherit(ancestors.Select(x => x.Padding), Padding) ?? new float[4]),
                        Inherit(ancestors.Select(x => x.Size), Size) ?? new(),
                        backgroundColor,
                        backgroundShader,
                        texture,
                        Inherit(ancestors.Select(x => x.FontFace), FontFace)?.Element,
                        Inherit(ancestors.Select(x => x.FontSize), FontSize) ?? 12,
                        Inherit(ancestors.Select(x => x.Align), Align) ?? HorizontalAlignment.Left,
                        Inherit(ancestors.Select(x => x.VerticalAlign), VerticalAlign) ?? VerticalAlignment.Top,
                        Inherit(ancestors.Select(x => x.Color), Color) ?? Color4.Black,
                        Inherit(ancestors.Select(x => x.Shader), Shader)?.Element,
                        Inherit(ancestors.Select(x => x.DisableScissor), DisableScissor) ?? false,
                        backgroundColor.All(x => x.A < float.Epsilon) 
                            && (borderColor.All(x => x.A < float.Epsilon) || borderWidth.All(x => x < float.Epsilon))
                            && (foregroundColor.A < float.Epsilon),
                        uniforms));
            }

            private static T Inherit<T>(IEnumerable<T> ancestors, T child)
            {
                return child ?? ancestors.Aggregate((left, right) => left ?? right);
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
