using Cardamom.Graphics;
using Cardamom.Mathematics;
using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Filters
{
    [FilterBuilder(typeof(Builder))]
    [FilterInline]
    public class Bucketize : IFilter
    {
        private static ComputeShader? s_BucketizeShader;
        private static readonly int s_BucketCountLocation = 0;
        private static readonly int s_ColorLocation = 1;
        private static readonly int s_ConditionPositionLocation = 33;
        private static readonly int s_ConditionChannelLocation = 65;
        private static readonly int s_ConditionRangeLocation = 193;
        private static readonly int s_ChannelLocation = 321;

        public struct Bucket
        {
            public Color4 Color { get; set; }
            public List<Condition> Conditions { get; set; } = new();

            public Bucket() { }
        }

        public struct Condition
        {
            public Channel Channel { get; set; }
            public Interval Range { get; set; } = new(0, 1);

            public Condition() { }
        }

        private readonly Color4[] _colors;
        private readonly Vector2i[] _bucketPositions;
        private readonly int[] _conditionChannels;
        private readonly Vector2[] _conditionRanges;

        public Bucketize(IEnumerable<Bucket> buckets)
        {
            var c = buckets.ToArray();
            int numConditions = c.Sum(x => x.Conditions.Count);

            _colors = new Color4[c.Length];
            _bucketPositions = new Vector2i[c.Length];
            _conditionChannels = new int[numConditions];
            _conditionRanges = new Vector2[numConditions];

            int i = 0;
            for (int x=0; x < c.Length; ++x)
            {
                int j = i;
                foreach (var condition in c[x].Conditions)
                {
                    _conditionChannels[j] = condition.Channel.GetIndex();
                    _conditionRanges[j] = new(condition.Range.Minimum, condition.Range.Maximum);
                    ++j;
                }
                _bucketPositions[x] = new(i, j);
                _colors[x] = c[x].Color;
                i = j;
            }
        }

        public void Apply(Canvas output, Channel channel, Dictionary<string, Canvas> inputs)
        {
            Precondition.Check(inputs.Count == 1);

            s_BucketizeShader ??= ComputeShader.FromFile("Resources/ImageProcessing/Filters/bucketize.comp");

            s_BucketizeShader.SetInt32(s_BucketCountLocation, _colors.Length);
            s_BucketizeShader.SetColorArray(s_ColorLocation, _colors);
            s_BucketizeShader.SetVector2iArray(s_ConditionPositionLocation, _bucketPositions);
            s_BucketizeShader.SetInt32Array(s_ConditionChannelLocation, _conditionChannels);
            s_BucketizeShader.SetVector2Array(s_ConditionRangeLocation, _conditionRanges);

            s_BucketizeShader.SetInt32(s_ChannelLocation, (int)channel);

            var inTex = inputs.First().Value.GetTexture();
            var outTex = output.GetTexture();
            inTex.BindImage(0);
            outTex.BindImage(1);
            s_BucketizeShader.DoCompute(inTex.Size);
            Texture.UnbindImage(0);
            Texture.UnbindImage(1);
        }

        public class Builder : IFilter.IFilterBuilder
        {
            private readonly List<Bucket> _buckets = new();

            public Builder AddBucket(Bucket bucket)
            {
                _buckets.Add(bucket);
                return this;
            }

            public Builder AddAllBuckets(IEnumerable<Bucket> buckets)
            {
                _buckets.AddRange(buckets);
                return this;
            }

            public IFilter Build()
            {
                return new Bucketize(_buckets);
            }
        }
    }
}
