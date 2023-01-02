namespace Cardamom.Mathematics.Coordinates
{
    public struct Vector3f : IVector
    {
        public int Cardinality => 3;

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

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
                    case 2:
                        return Z;
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
                    case 2:
                        Z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public Vector3f() { }

        public Vector3f(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public IVector Clone()
        {
            return new Vector3f(X, Y, Z);
        }
    }
}
