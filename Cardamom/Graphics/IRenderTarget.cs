using OpenTK.Graphics.OpenGL4;

namespace Cardamom.Graphics
{
    public interface IRenderTarget : IGraphicsContext
    {
        void SetActive(bool active);
        void Display();
        void Draw(VertexArray vertices, int start, int count, RenderResources resources);
        void Draw(Vertex3[] vertices, PrimitiveType primitiveType, int start, int count, RenderResources resources);
        void Draw<T>(VertexBuffer<T> buffer, int start, int count, RenderResources resources) where T : struct;
    }
}
