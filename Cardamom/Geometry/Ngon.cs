using OpenTK.Mathematics;

namespace Cardamom.Geometry
{
    public class Ngon : IPolygon
    {
        private readonly Vector2[] _vertices;

        public int VertexCount => _vertices.Length;

        public Ngon(Vector2[] vertices)
        {
            _vertices = vertices;
        }

        public bool ContainsPoint(Vector2 point)
        {
            bool c = false;
            int i;
            int j;
            for (i = 0, j = VertexCount - 1; i < VertexCount; j = i++)
            {
                var left = GetVertex(i);
                var right = GetVertex(j);
                if (((left.Y > point.Y) != (right.Y > point.Y)) 
                    && (point.X < (right.X - left.X) * (point.Y - left.Y) / (right.Y - left.Y) + left.X))
                {
                    c = !c;
                }
            }
            return c;
        }

        public float GetArea()
        {
            float a = 0;
            for (int i = 0; i < VertexCount - 1; ++i)
            {
                a += GetVertex(i).X * GetVertex(i + 1).Y - GetVertex(i + 1).X * GetVertex(i).Y;
            }
            a += GetVertex(VertexCount - 1).X * GetVertex(0).Y - GetVertex(0).X * GetVertex(VertexCount - 1).Y;
            return Math.Abs(a) / 2;
        }

        public Vector2 GetVertex(int index)
        {
            return _vertices[index];
        }

        public Rectangle GetBounds()
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            for (int i = 0; i < VertexCount; ++i)
            {
                var vertex = GetVertex(i);
                if (vertex.X < minX)
                {
                    minX = vertex.X;
                }
                if (vertex.X > maxX)
                {
                    maxX = vertex.X;
                }
                if (vertex.Y < minY)
                {
                    minY = vertex.Y;
                }
                if (vertex.Y > maxY)
                {
                    maxY = vertex.Y;
                }
            }

            return new Rectangle(new Vector2(minX, minY), new Vector2(maxX - minX, maxY - minY));
        }
    }
}
