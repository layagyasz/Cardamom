using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;

namespace Cardamom.Graphics.Core
{
    public class GLVertexArray<T> : GLObject where T: struct
    {
        private readonly Type _type;
        private readonly int _size;
        private readonly GLBuffer _buffer;

        public GLVertexArray(GLBuffer buffer)
            : base(GL.GenVertexArray())
        {
            _type = typeof(T);
            _size = Marshal.SizeOf(_type);
            _buffer = buffer;

            Bind();
            _buffer.Bind();

            foreach (var field in _type.GetFields())
            {
                var attribute = 
                    (VertexAttributeAttribute?)field.GetCustomAttributes(false)
                        .FirstOrDefault(x => x is VertexAttributeAttribute);
                if (attribute != null)
                {
                    GL.VertexAttribPointer(
                        attribute.Index,
                        attribute.Cardinality,
                        attribute.Type,
                        attribute.Normalized,
                        _size,
                        Marshal.OffsetOf<T>(field.Name));
                    Error.LogGLError($"link {field.Name} attribute");

                    GL.EnableVertexAttribArray(attribute.Index);
                    Error.LogGLError($"enable {field.Name} attribute");
                }
            }
        }

        public void Bind()
        {
            GL.BindVertexArray(Handle);
            Error.LogGLError("bind vertex array");
        }

        public void Draw(PrimitiveType primitiveType, int start, int count)
        {
            Bind();
            GL.DrawArrays(primitiveType, start, count);
            Error.LogGLError($"draw {primitiveType} vertex array");
        }

        public void SetData(T[] data)
        {
            _buffer.Buffer(_size, data);
        }

        public void SetData(T[] data, int start, int count)
        {
            _buffer.Buffer(_size, data, start, count);
        }

        public void SubDataArray(T[] data, int start, int count)
        {
            _buffer.SubDataArray(_size * start, _size, data, count);
        }

        protected override void DisposeImpl()
        {
            GL.DeleteVertexArray(Handle);
            _buffer.Dispose();
        }
    }
}
