using OpenTK.Mathematics;

namespace Cardamom.Planar
{
    public interface IPolygon
    {
        int VertexCount { get; }

        bool ContainsPoint(Vector2 point);
        float GetArea();
        Vector2 GetVertex(int index);
        Rectangle GetBounds();
        bool Intersects(IPolygon polygon)
        {
            for (int i = 0; i < polygon.VertexCount; ++i)
            {
                if (ContainsPoint(GetVertex(i)))
                {
                    return true;
                }
            }
            return polygon.Intersects(this);
        }
    }
}
