namespace Cardamom.Mathematics.Geometry
{
    public struct HyperSphere
    {
        public HyperVector Center { get; set; }
        public float Radius2 { get; set; }

        public HyperSphere(HyperVector center, float radius2)
        {
            Center = center;
            Radius2 = radius2;
        }

        public bool Intersects(HyperBox box)
        {
            double sum = 0;
            for (int i = 0; i < box.Min.Cardinality; ++i)
            {
                if (Center[i] < box.Min[i])
                {
                    double s = Center[i] - box.Min[i];
                    sum += s * s;
                }
                if (Center[i] > box.Max[i])
                {
                    double s = Center[i] - box.Max[i];
                    sum += s * s;
                }
            }
            return sum <= Radius2;
        }
    }
}
