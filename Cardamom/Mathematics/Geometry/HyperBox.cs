namespace Cardamom.Mathematics.Geometry
{
    public struct HyperBox
    {
        public HyperVector Min { get; set; }
        public HyperVector Max { get; set; }

        public HyperBox(HyperVector min, HyperVector max)
        {
            Min = min;
            Max = max;
        }

        public static HyperBox GetBoundingBox(IEnumerable<HyperVector> points, int cardinality)
        {
            var min = new HyperVector(cardinality);
            var max = new HyperVector(cardinality);
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

        public Tuple<HyperBox, HyperBox> Split(HyperVector point, int dim)
        {
            var newMin = Min.Clone();
            var newMax = Max.Clone();
            newMin[dim] = newMax[dim] = point[dim];
            return new(new(Min, newMax), new(newMin, Max));
        }
    }
}
