using OpenTK.Graphics.OpenGL;
using System.IO;

namespace Epidesim.Engine.Drawing.Types.Shaders
{
	public class VertexShader : Shader
	{
		public VertexShader(string source) : base(source, ShaderType.VertexShader)
		{

		}

		public static VertexShader Load(string path)
		{
			string source = File.ReadAllText(path);
			return new VertexShader(source);
		}
	}
}
