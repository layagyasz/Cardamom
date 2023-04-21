using OpenTK.Mathematics;

namespace Cardamom.Graphics
{
    public interface IGraphicsContext
    {
        void Clear();
        void Flatten();
        Matrix4 GetModelMatrix();
        Projection GetProjection();
        Box2? GetScissor();
        Matrix4 GetViewMatrix();
        Box2i GetViewPort();
        void PopScissor();
        void PushEmptyScissor();
        void PushProjection(Projection projection);
        void PushScissor(Box3 scissor);
        void PopModelMatrix();
        void PopProjectionMatrix();
        void PopViewMatrix();
        void PushModelMatrix(Matrix4 transform);
        void PushTranslation(Vector3 translation);
        void PushViewMatrix(Matrix4 view);
        void SetViewPort(Box2i viewPort);
    }
}
