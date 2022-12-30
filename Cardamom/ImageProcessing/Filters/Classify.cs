using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class Classify : IFilter
    {
        private static ComputeShader? s_ClassifyShader;
        private static readonly int s_ClassificationCountLocation = 0;
        private static readonly int s_ColorLocation = 1;
        private static readonly int s_ConditionPositionLocation = 33;
        private static readonly int s_ConditionChannelLocation = 65;
        private static readonly int s_ConditionRangeLocation = 193;
        private static readonly int s_ChannelLocation = 321;

        public struct Classification
        {
            public Color4 Color { get; set; }
            public List<Condition> Conditions { get; set; } = new();

            public Classification() { }
        }

        public struct Condition
        {
            public Channel Channel { get; set; }
            public float Minimum { get; set; } = 0f;
            public float Maximum { get; set; } = 1f;

            public Condition() { }
        }

        private readonly Color4[] _colors;
        private readonly Vector2i[] _classificationPositions;
        private readonly int[] _conditionChannels;
        private readonly Vector2[] _conditionRanges;

        public Classify(IEnumerable<Classification> classifications)
        {
            var c = classifications.ToArray();
            int numConditions = c.Sum(x => x.Conditions.Count);

            _colors = new Color4[c.Length];
            _classificationPositions = new Vector2i[c.Length];
            _conditionChannels = new int[numConditions];
            _conditionRanges = new Vector2[numConditions];

            int i = 0;
            for (int x=0; x < c.Length; ++x)
            {
                int j = i;
                foreach (var condition in c[x].Conditions)
                {
                    _conditionChannels[j] = condition.Channel.GetIndex();
                    _conditionRanges[j] = new(condition.Minimum, condition.Maximum);
                    ++j;
                }
                _classificationPositions[x] = new(i, j);
                _colors[x] = c[x].Color;
                i = j;
            }
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            s_ClassifyShader ??= ComputeShader.FromFile("Resources/classify.comp");

            s_ClassifyShader.SetInt32(s_ClassificationCountLocation, _colors.Length);
            s_ClassifyShader.SetColorArray(s_ColorLocation, _colors);
            s_ClassifyShader.SetVector2iArray(s_ConditionPositionLocation, _classificationPositions);
            s_ClassifyShader.SetInt32Array(s_ConditionChannelLocation, _conditionChannels);
            s_ClassifyShader.SetVector2Array(s_ConditionRangeLocation, _conditionRanges);

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

            public IFilter Build()
            {
                return new Classify(_classifications);
            }
        }
    }
}
