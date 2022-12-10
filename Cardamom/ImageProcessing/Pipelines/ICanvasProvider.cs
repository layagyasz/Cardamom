namespace Cardamom.ImageProcessing.Pipelines
{
    public interface ICanvasProvider : IDisposable
    {
        Canvas Get();
        void Return(Canvas canvas);
    }
}
