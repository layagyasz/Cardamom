using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class Classify : IFilter
    {
        private static readonly Vector2i s_LocalGroupSize = new(32, 32);

        private static ComputeShader? s_ClassifyShader;
        private static readonly int s_ModeLocation = 0;
        private static readonly int s_ClassificationCountLocation = 1;
        private static readonly int s_ColorLocation = 2;
        private static readonly int s_CenterLocation = 34;
        private static readonly int s_AxisWeightLocation = 66;
        private static readonly int s_WeightLocation = 98;
        private static readonly int s_BlendRangeLocation = 130;
        private static readonly int s_ChannelLocation = 162;

        public struct Classification
        {
            public Color4 Color { get; set; }
            public Vector4 Center { get; set; }
            public Vector4 AxisWeight { get; set; }
            public float Weight { get; set; }
            public float BlendRange { get; set; }

            public Classification() { }
        }

        private readonly bool _blend;
        private readonly Color4[] _colors;
        private readonly Vector4[] _centers;
        private readonly Vector4[] _axisWeights;
        private readonly float[] _weights;
        private readonly float[] _blendRanges;

        public Classify(bool blend, IEnumerable<Classification> classifications)
        {
            var c = classifications.ToArray();

            _blend = blend;
            _colors = new Color4[c.Length];
            _centers = new Vector4[c.Length];
            _axisWeights = new Vector4[c.Length];
            _weights = new float[c.Length];
            _blendRanges = new float[c.Length];

            for (int i = 0; i < c.Length; ++i)
            {
                _colors[i] = c[i].Color;
                _centers[i] = c[i].Center;
                _axisWeights[i] = c[i].AxisWeight;
                _weights[i] = c[i].Weight;
                _blendRanges[i] = c[i].BlendRange;
            }
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            s_ClassifyShader ??= 
                ComputeShader.FromFile("Resources/ImageProcessing/Filters/classify.comp", s_LocalGroupSize);

            s_ClassifyShader.SetInt32(s_ModeLocation, _blend ? 1 : 0);
            s_ClassifyShader.SetInt32(s_ClassificationCountLocation, _colors.Length);
            s_ClassifyShader.SetColorArray(s_ColorLocation, _colors);
            s_ClassifyShader.SetVector4Array(s_CenterLocation, _centers);
            s_ClassifyShader.SetVector4Array(s_AxisWeightLocation, _axisWeights);
            s_ClassifyShader.SetFloatArray(s_WeightLocation, _weights);
            s_ClassifyShader.SetFloatArray(s_BlendRangeLocation, _blendRanges);

            s_ClassifyShader.SetInt32(s_ChannelLocation, (int)channel);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            s_ClassifyShader.DoCompute(inTex.Size);
            Texture.UnbindImage(0);
            Texture.UnbindImage(1);
        }

        public class Builder : IFilter.IFilterBuilder
        {
            private bool _blend;
            private readonly List<Classification> _classifications = new();

            public Builder AddClassification(Classification classification)
            {
                _classifications.Add(classification);
                return this;
            }

            public Builder AddAllClassifications(IEnumerable<Classification> classifications)
            {
                _classifications.AddRange(classifications);
                return this;
            }

            public Builder SetBlend(bool blend)
            {
                _blend = blend;
                return this;
            }

            public IFilter Build()
            {
                return new Classify(_blend, _classifications);
            }
        }
    }
}
