using OpenTK.Mathematics;

namespace Cardamom.ImageProcessing.Pipelines
{
    public class CachingCanvasProvider : ICanvasProvider
    {
        public Vector2i Size { get; }
        public Color4 Color { get; }

        private readonly Queue<Canvas> _free = new();

        public CachingCanvasProvider(Vector2i size, Color4 color)
        {
            Size = size;
            Color = color;
        }

        public Canvas Get()
        {
            if (_free.Count == 0)
            {
                return new Canvas(Size, Color);
            }
            return _free.Dequeue();
        }

        public void Return(Canvas canvas)
        {
            _free.Enqueue(canvas);
        }

        public void Dispose() 
        {
            GC.SuppressFinalize(this);
            GC.KeepAlive(this);
            foreach (var canvas in _free)
            {
                canvas.Dispose();
            }
            _free.Clear();
        }
    }
}
