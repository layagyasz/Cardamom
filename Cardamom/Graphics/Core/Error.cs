using OpenTK.Graphics.OpenGL4;

namespace Cardamom.Graphics.Core
{
    public static class Error
    {
        public static void LogGLError(string callPoint)
        {
# if DEBUG
            var code = GL.GetError();
            if (code != ErrorCode.NoError)
            {
                throw new InvalidProgramException($"({callPoint}): {code}");
            }
# endif
        }
    }
}
