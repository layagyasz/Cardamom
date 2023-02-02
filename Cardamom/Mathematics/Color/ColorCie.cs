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

        public override string ToString()
        {
            return string.Format($"[ColorCie: X={X}, Y={Y}, Z={Z}]");
        }
    }
}
