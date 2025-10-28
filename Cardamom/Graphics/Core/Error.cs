using OpenTK.Graphics.OpenGL4;

namespace Cardamom.Graphics.Core
{
    internal static class Error
    {
        public static void LogGLError(string callPoint)
        {
# if DEBUG
            var code = GL.GetError();
            if (code != ErrorCode.NoError)
            {
                throw new Exception($"({callPoint}): {code}");
            }
# endif
        }
    }
}
