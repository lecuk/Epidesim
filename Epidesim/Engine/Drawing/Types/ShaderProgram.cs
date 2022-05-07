using System;
using OpenTK.Graphics.OpenGL;
using Epidesim.Engine.Drawing.Types.Shaders;
using System.Diagnostics;

namespace Epidesim.Engine.Drawing.Types
{
	class ShaderProgram : IDisposable
	{
		public readonly int handle;
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
			Debug.WriteLine(String.Format("link ShaderProgram: ", info));
		}

		public void UseProgram()
		{
			GL.UseProgram(this.handle);
			GL.GetProgramInfoLog(this.handle, out string info);
			//Debug.WriteLine(String.Format("use ShaderProgram: ", info));
		}

		public void Dispose()
		{
			vertexShader.DetachFromProgram(this.handle);
			fragmentShader.DetachFromProgram(this.handle);
			GL.DeleteProgram(this.handle);
		}
	}
}
