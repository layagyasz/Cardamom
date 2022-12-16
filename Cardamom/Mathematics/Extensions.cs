using OpenTK.Mathematics;
using System.Drawing;

namespace Cardamom.Mathematics
{
    public static class Extensions
    {
        public static Box2 GetIntersection(this Box2 left, Box2 right)
        {
            Vector2 topLeft = new(Math.Max(left.Min.X, right.Min.X), Math.Max(left.Min.Y, right.Min.Y));
            Vector2 bottomRight = new(Math.Min(left.Max.X, right.Max.X), Math.Min(left.Max.Y, right.Max.Y));
            return new(topLeft, bottomRight);
        }
    }
}
