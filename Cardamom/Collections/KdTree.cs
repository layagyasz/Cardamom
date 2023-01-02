using Cardamom.Mathematics;
using Cardamom.Mathematics.Geometry;

namespace Cardamom.Collections
{
    public class KdTree<T>
    {
        public HyperVector Key { get; }
        public T Value { get; }
        public int Dimension { get; }
        public KdTree<T>? Left { get; }
        public KdTree<T>? Right { get; }
        public HyperBox Region { get; }

        private KdTree(HyperVector key, T value, int dimension, KdTree<T>? left, KdTree<T>? right, HyperBox region)
        {
            Key = key;
            Value = value;
            Dimension = dimension;
            Left = left;
            Right = right;
            Region = region;
        }

        public T GetClosest(HyperVector point)
        {
            (var result, _) = GetClosestSubtree(new(point, float.MaxValue));
            return result.Value ?? Value;
        }

        private (KdTree<T>?, float) GetClosestSubtree(HyperSphere bounds)
        {
            KdTree<T>? current = null;

            var currentD = HyperVector.DistanceSquared(Key, bounds.Center);
            if (currentD < bounds.Radius2)
            {
                if (currentD < float.Epsilon)
                {
                    return (current, currentD);
                }
                bounds.Radius2 = currentD;
                current = this;
            }
            else if (Left == null && Right == null)
            {
                return (null, currentD);
            }

            KdTree<T>? closestLeft = null;
            float leftD = float.MaxValue;
            KdTree<T>? closestRight = null;
            float rightD = float.MaxValue;
            if (bounds.Center[Dimension] < Key[Dimension])
            {
                if (Left != null)
                {
                    (closestLeft, leftD) = Left.GetClosestSubtree(bounds);
                }
                if (closestLeft != null && leftD < currentD)
                {
                    current = closestLeft;
                    currentD = leftD;
                    bounds.Radius2 = currentD;
                }

                if (Right != null && bounds.Intersects(Right.Region))
                {
                    (closestRight, rightD) = Right.GetClosestSubtree(bounds);
                }
                if (closestRight != null && rightD < currentD)
                {
                    current = closestRight;
                    currentD = rightD;
                }
            }
            else
            {
                if (Right != null)
                {
                    (closestRight, rightD) = Right.GetClosestSubtree(bounds);
                }
                if (closestRight != null && rightD < currentD)
                {
                    current = closestRight;
                    currentD = rightD;
                    bounds.Radius2 = currentD;
                }

                if (Left != null && bounds.Intersects(Left.Region))
                {
                    (closestLeft, leftD) = Left.GetClosestSubtree(bounds);
                }
                if (closestLeft != null && leftD < currentD)
                {
                    current = closestLeft;
                    currentD = leftD;
                }
            }
            return (current, currentD);
        }

        public class Builder
        {
            public int Cardinality { get; set; } 
            public List<KeyValuePair<HyperVector, T>> Values = new();

            public Builder SetCardinality(int cardinality)
            {
                Cardinality = cardinality;
                return this;
            }

            public Builder Add(HyperVector key, T value)
            {
                Precondition.Check(key.Cardinality >= Cardinality);
                Values.Add(new(key, value));
                return this;
            }

            public KdTree<T> Build()
            {
                Shuffle(Values);
                return BuildSubtree(
                    0, Values.Count - 1, HyperBox.GetBoundingBox(Values.Select(x => x.Key), Cardinality), 0);
            }

            private KdTree<T> BuildSubtree(int indexLow, int indexHigh, HyperBox region, int depth)
            {
                int dim = depth % Cardinality;
                int pivot = Ninther(Values, indexLow, indexHigh, dim);
                var kv = Values[pivot];

                (Values[pivot], Values[indexHigh]) = (Values[indexHigh], Values[pivot]);
                int j = indexLow;
                for (int i = indexLow; i < indexHigh; ++i)
                {
                    if (Values[i].Key[dim] < Values[indexHigh].Key[dim])
                    {
                        (Values[i], Values[j]) = (Values[j], Values[i]);
                        ++j;
                    }
                }
                var subRegions = region.Split(kv.Key, dim);

                KdTree<T>? left = null;
                if (indexLow <= j - 1)
                {
                    left = BuildSubtree(indexLow, j - 1, subRegions.Item1, depth + 1);
                }
                KdTree<T>? right = null;
                if (j <= indexHigh - 1)
                {
                    right = BuildSubtree(j, indexHigh - 1, subRegions.Item2, depth + 1);
                }
                return new KdTree<T>(kv.Key, kv.Value, dim, left, right, region);
            }

            private static int Ninther(
                List<KeyValuePair<HyperVector, T>> keysAndValues, int indexLow, int indexHigh, int dim)
            {
                int c1 = indexLow + (indexHigh - indexLow) / 3;
                int c2 = indexLow + 2 * (indexHigh - indexLow) / 3;
                return MedianIndex(
                    keysAndValues,
                    MedianIndex(keysAndValues, indexLow, c1 - 1, dim),
                    MedianIndex(keysAndValues, c1, c2 - 1, dim),
                    MedianIndex(keysAndValues, c2, indexHigh, dim), dim);
            }

            private static int MedianIndex(
                List<KeyValuePair<HyperVector, T>> keysAndValues, int indexLow, int indexHigh, int dim)
            {
                if (indexHigh - indexLow > 1)
                {
                    return MedianIndex(keysAndValues, indexLow, (indexLow + indexHigh) / 2, indexHigh, dim);
                }
                if (indexHigh - indexLow == 1)
                {
                    return MaxIndex(keysAndValues, indexLow, indexHigh, dim);
                }
                return indexLow;
            }

            private static int MedianIndex(
                List<KeyValuePair<HyperVector, T>> keysAndValues, int indexA, int indexB, int indexC, int dim)
            {
                if (MaxIndex(keysAndValues, indexA, indexB, dim) == indexA)
                {
                    (indexB, indexA) = (indexA, indexB);
                }
                return MaxIndex(keysAndValues, MinIndex(keysAndValues, indexB, indexC, dim), indexA, dim);
            }

            private static int MaxIndex(
                List<KeyValuePair<HyperVector, T>> keysAndValues, int indexLeft, int indexRight, int dim)
            {
                if (keysAndValues[indexLeft].Key[dim] > keysAndValues[indexRight].Key[dim])
                {
                    return indexLeft;
                }
                else return indexRight;
            }

            private static int MinIndex(
                List<KeyValuePair<HyperVector, T>> keysAndValues, int indexLeft, int indexRight, int dim)
            {
                if (keysAndValues[indexLeft].Key[dim] < keysAndValues[indexRight].Key[dim])
                {
                    return indexLeft;
                }
                else return indexRight;
            }

            private static void Shuffle<T0>(List<T0> values)
            {
                Random Random = new();
                for (int i = 0; i < values.Count; ++i)
                {
                    int j = Random.Next(0, values.Count);
                    (values[i], values[j]) = (values[j], values[i]);
                }
            }
        }
    }
}
