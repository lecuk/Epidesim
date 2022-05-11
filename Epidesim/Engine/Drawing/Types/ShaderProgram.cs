using System;
using OpenTK.Graphics.OpenGL;
using Epidesim.Engine.Drawing.Types.Shaders;
using System.Diagnostics;

namespace Epidesim.Engine.Drawing.Types
{
	class ShaderProgram : IDisposable
	{
		private readonly int handle;
		private readonly VertexShader vertexShader;
		private readonly FragmentShader fragmentShader;

		public ShaderProgram(VertexShader vertexShader, FragmentShader fragmentShader)
		{
			this.handle = GL.CreateProgram();
			this.vertexShader = vertexShader;
			this.fragmentShader = fragmentShader;

			vertexShader.AttachToProgram(this.handle);
			fragmentShader.AttachToProgram(this.handle);
			GL.LinkProgram(this.handle);
			
			GL.GetProgramInfoLog(handle, out string info);
		}

		public void UseProgram()
		{
			GL.UseProgram(this.handle);
			GL.GetProgramInfoLog(this.handle, out string info);
		}

		public void Dispose()
		{
			vertexShader.DetachFromProgram(this.handle);
			fragmentShader.DetachFromProgram(this.handle);
			GL.DeleteProgram(this.handle);
		}

		public int GetAttributeIndex(string attributeName)
		{
			return GL.GetAttribLocation(this.handle, attributeName);
		}

		public int GetUniformIndex(string uniformName)
		{
			return GL.GetUniformLocation(this.handle, uniformName);
		}
	}
}
