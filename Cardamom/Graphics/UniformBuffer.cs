using Cardamom.Graphics.Core;
using OpenTK.Graphics.OpenGL4;

namespace Cardamom.Graphics
{
    public class UniformBuffer : GraphicsResource
    {
        private readonly GLBuffer _buffer;

        public UniformBuffer(int size)
        {
            _buffer = new(BufferTarget.UniformBuffer);
            _buffer.Allocate(size);
        }

        public void Bind(int index)
        {
            _buffer.Bind(index);
        }

        public T Get<T>(int offset, int size) where T : struct
        {
            return _buffer.Read<T>(offset, size);
        }

        public T[] GetArray<T>(int offset, int size, int length) where T : struct
        {
            return _buffer.ReadArray<T>(offset, size, length);
        }

        public void Set<T>(int offset, int size, T data) where T : struct
        {
            _buffer.SubData(offset, size, data);
        }

        public void SetArray<T>(int offset, int size, T[] data) where T : struct
        {
            _buffer.SubDataArray(offset, size, data);
        }

        protected override void DisposeImpl()
        {
            _buffer.Dispose();
        }
    }
}
