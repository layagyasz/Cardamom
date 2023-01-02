using Cardamom.Mathematics.Coordinates;

namespace Cardamom.Mathematics.Geometry
{
    public class HyperBox
    {
        public IVector Min { get; set; }
        public IVector Max { get; set; }

        public HyperBox(IVector min, IVector max)
        {
            Min = min;
            Max = max;
        }

        public static HyperBox GetBoundingBox(IEnumerable<IVector> points, int cardinality)
        {
            var min = points.First().Clone();
            var max = min.Clone();
            foreach (var point in points)
            {
                for (int j=0; j < cardinality; ++j)
                {
                    min[j] = Math.Min(min[j], point[j]);
                    max[j] = Math.Max(max[j], point[j]);
                }
            }
            return new(min, max);
        }

        public Tuple<HyperBox, HyperBox> Split(IVector point, int dim)
        {
            var newMin = Min.Clone();
            var newMax = Max.Clone();
            newMin[dim] = newMax[dim] = point[dim];
            return new(new(Min.Clone(), newMax), new(newMin, Max.Clone()));
        }
    }
}
