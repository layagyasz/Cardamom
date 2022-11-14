using SFML.System;

namespace Cardamom.Planar
{
    public interface IPolygon
    {
        int VertexCount { get; }

        bool ContainsPoint(Vector2f point);
        float GetArea();
        Vector2f GetVertex(int index);
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
