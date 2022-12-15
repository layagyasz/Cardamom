using Cardamom.Graphics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class Classify : IFilter
    {
        private static Shader? CLASSIFY_SHADER;
        private static readonly int CLASSIFICATION_COUNT_LOCATION = 0;
        private static readonly int COLOR_LOCATION = 1;
        private static readonly int CONDITION_POSITION_LOCATION = 33;
        private static readonly int CONDITION_CHANNEL_LOCATION = 65;
        private static readonly int CONDITION_RANGE_LOCATION = 193;
        private static readonly int CHANNEL_LOCATION = 321;

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

            CLASSIFY_SHADER ??= new Shader.Builder().SetCompute("Resources/classify.comp").Build();

            CLASSIFY_SHADER.SetInt32(CLASSIFICATION_COUNT_LOCATION, _colors.Length);
            CLASSIFY_SHADER.SetColorArray(COLOR_LOCATION, _colors);
            CLASSIFY_SHADER.SetVector2iArray(CONDITION_POSITION_LOCATION, _classificationPositions);
            CLASSIFY_SHADER.SetInt32Array(CONDITION_CHANNEL_LOCATION, _conditionChannels);
            CLASSIFY_SHADER.SetVector2Array(CONDITION_RANGE_LOCATION, _conditionRanges);

            CLASSIFY_SHADER.SetInt32(CHANNEL_LOCATION, (int)channel);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            CLASSIFY_SHADER.DoCompute(inTex.Size);
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
