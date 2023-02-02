using Cardamom.Json;

namespace Cardamom.Mathematics
{
    public class LinearApproximation
    {
        public Interval Range { get; set; }
        public float Resolution { get; set; }
        public float[] Values { get; set; } = Array.Empty<float>();

        public float GetPoint(float x)
        {
            if (!Range.Contains(x))
            {
                return float.NaN;
            }

            int bucket = GetBucket(x);
            return Lerp(bucket, GetBucketA(x, bucket));
        }

        public float GetIntegral(float min, float max)
        {
            if (max > min)
            {
                return float.NaN;
            }
            if (!Range.Contains(min))
            {
                return float.NaN;
            }
            if (!Range.Contains(max))
            {
                return float.NaN;
            }

            int minBucket = GetBucket(min);
            float minA = GetBucketA(min, minBucket);
            int maxBucket = GetBucket(max);
            float maxA = GetBucketA(max, maxBucket);
            if (minBucket == maxBucket)
            {
                return 0.5f * (maxA - minA) * Resolution * (Lerp(minBucket, minA) + Lerp(maxBucket, maxA));
            }
            float result = 
                (1 - minA) * (Lerp(minBucket, minA) + Values[minBucket + 1]) 
                + maxA * (Lerp(maxBucket, maxA) + Values[maxBucket + 1]);
            for (int i=minBucket + 1; i<maxBucket; ++i)
            {
                result += Values[i] + Values[i + 1];
            }
            return 0.5f * Resolution * result;
        }

        private int GetBucket(float x)
        {
            return (int)((x - Range.Minimum) / Resolution);
        }

        private float GetBucketA(float x, int bucket)
        {
            return (x - (bucket * Resolution + Range.Minimum)) / Resolution;
        }

        private float Lerp(int bucket, float a)
        {
            if (bucket == Values.Length - 1)
            {
                return Values[bucket];
            }
            return Values[bucket] * (1 - a) + Values[bucket + 1] * a;
        }
    }
}
