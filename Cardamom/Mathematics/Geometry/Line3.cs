using Cardamom.Collections;
using OpenTK.Mathematics;

namespace Cardamom.Mathematics.Geometry
{
    public class Line3 : IEnumerable<Vector3>
    {
        private readonly Vector3[] _points;
        private readonly Vector3[] _normals;

        public int Count => _points.Length;
        public bool IsLoop { get; }

        public Vector3 this[int index]
        {
            get => _points[index];
            set => _points[index] = value;
        }

        public Line3(Vector3[] points, Vector3[] normals, bool isLoop = false)
        {
            _points = points;
            _normals = normals;
            IsLoop = isLoop;
        }

        public Line3(Vector3[] points, Vector3 normal, bool isLoop = false)
            : this(points, Enumerable.Repeat(normal, points.Length).ToArray(), isLoop) { }

        public IEnumerator<Vector3> GetEnumerator()
        {
            return _points.AsEnumerable().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Vector3 GetNormal(int index)
        {
            return _normals[index % _points.Length];
        }

        public Segment3 GetSegment(int index)
        {
            return new(
                _points[(_points.Length + index) % _points.Length], 
                _points[(_points.Length + index + 1) % _points.Length]);
        }

        public class Builder
        {
            private bool _isLoop;
            private readonly ArrayList<Vector3> _points = new();
            private readonly ArrayList<Vector3> _normals = new();

            public Builder AddPoint(Vector3 point, Vector3 normal)
            {
                _points.Add(point);
                _normals.Add(normal);
                return this;
            }

            public Builder IsLoop()
            {
                _isLoop = true;
                return this;
            }

            public Line3 Build()
            {
                return new(_points.ToArray(), _normals.ToArray(), _isLoop);
            }
        }
    }
}
