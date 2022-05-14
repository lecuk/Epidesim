using OpenTK.Graphics.OpenGL;
using System.IO;

namespace Epidesim.Engine.Drawing.Types.Shaders
{
	class FragmentShader : Shader
	{
		public FragmentShader(string source) : base(source, ShaderType.FragmentShader)
		{

		}

		public static FragmentShader Load(string path)
		{
			string source = File.ReadAllText(path);
			return new FragmentShader(source);
		}
	}
}
