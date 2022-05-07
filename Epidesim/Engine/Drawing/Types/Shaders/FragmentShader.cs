using OpenTK.Graphics.OpenGL;

namespace Epidesim.Engine.Drawing.Types.Shaders
{
	class FragmentShader : Shader
	{
		public FragmentShader(string path)
			: base(path, ShaderType.FragmentShader) { }
	}
}
