using System;
using System.Diagnostics;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace Epidesim.Engine.Drawing.Types.Shaders
{
	class Shader : IDisposable
	{
		private readonly int handle;
		public readonly ShaderType Type;

		public Shader(string path, ShaderType type)
		{
			int handle = GL.CreateShader(type);
			
			string source = File.ReadAllText(path);

			GL.ShaderSource(handle, source);
			GL.CompileShader(handle);
			GL.GetShader(handle, ShaderParameter.CompileStatus, out int statusCode);
			GL.GetShaderInfoLog(handle, out string info);
			
			Debug.WriteLine(String.Format("Shader #{0} ({1}) compile info: {2}",
				handle,
				Type.ToString(),
				String.IsNullOrWhiteSpace(info) ? "<no info>" : info));

			if (statusCode != (int)All.True)
			{
				throw new Exception(info);
			}

			this.handle = handle;
			this.Type = type;
		}

		public void AttachToProgram(int programHandle)
		{
			GL.AttachShader(programHandle, handle);
		}

		public void DetachFromProgram(int programHandle)
		{
			GL.AttachShader(programHandle, handle);
		}

		public void Dispose()
		{
			GL.DeleteShader(this.handle);
		}

		public string GetInfo()
		{
			return GL.GetShaderInfoLog(this.handle);
		}
	}
}
