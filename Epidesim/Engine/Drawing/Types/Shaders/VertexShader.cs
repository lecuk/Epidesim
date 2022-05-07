using OpenTK.Graphics.OpenGL;

namespace Epidesim.Engine.Drawing.Types.Shaders
{
	class VertexShader : Shader
	{
		public VertexShader(string path)
			: base(path, ShaderType.VertexShader) {  }
	}
}
