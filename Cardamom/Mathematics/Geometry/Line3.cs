using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Geometry
{
    public class Line3
    {
        private readonly Vector3[] _points;

        public int Count => _points.Length;
        public bool IsLoop { get; }

        public Vector3 this[int index]
        {
            get => _points[index];
            set => _points[index] = value;
        }

        public Line3(Vector3[] points, bool isLoop = false)
        {
            _points = points;
            IsLoop = isLoop;
        }

        public Line3(Segment3[] segments, bool isLoop = false)
        {
            _points = new Vector3[segments.Length];
            for (int i=0; i<segments.Length - (isLoop ? 1 : 0); ++i)
            {
                _points[i] = segments[i].Left;
            }
            IsLoop = isLoop;
        }

        public Segment3 GetSegment(int index)
        {
            return new(
                _points[(_points.Length + index) % _points.Length], 
                _points[(_points.Length + index + 1) % _points.Length]);
        }
    }
}
