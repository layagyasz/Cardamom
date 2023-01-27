namespace Cardamom.Mathematics.Color
{
    public struct ColorCie
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z => 1 - (X + Y);

        public ColorCie(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
