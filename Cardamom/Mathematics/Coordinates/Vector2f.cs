namespace Cardamom.Mathematics.Coordinates
{
    public struct Vector2f : IVector
    {
        public int Cardinality => 2;

        public float X { get; set; }
        public float Y { get; set; }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                switch (index)
                {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public Vector2f() { }

        public Vector2f(float x, float y)
        {
            X = x;
            Y = y;
        }

        public IVector Clone()
        {
            return new Vector2f(X, Y);
        }
    }
}
