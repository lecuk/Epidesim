using System;
using OpenTK.Graphics.OpenGL;
using Epidesim.Engine.Drawing.Types.Shaders;
using System.Diagnostics;
using OpenTK;

namespace Epidesim.Engine.Drawing.Types
{
	public class ShaderProgram : IDisposable
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
			Debug.WriteLine(String.Format("Shader program #{0} link info: {1}",
				handle,
				String.IsNullOrWhiteSpace(info) ? "<no info>" : info));
		}

		public void UseProgram()
		{
			GL.UseProgram(this.handle);
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

		public void SetUniform(string uniformName, float value)
		{
			int index = GetUniformIndex(uniformName);
			GL.Uniform1(index, value);
		}

		public void SetUniform(string uniformName, Vector2 value)
		{
			int index = GetUniformIndex(uniformName);
			GL.Uniform2(index, value);
		}

		public void SetUniform(string uniformName, float value1, float value2)
		{
			int index = GetUniformIndex(uniformName);
			GL.Uniform2(index, value1, value2);
		}

		public void SetUniform(string uniformName, Vector3 value)
		{
			int index = GetUniformIndex(uniformName);
			GL.Uniform3(index, value);
		}

		public void SetUniform(string uniformName, float value1, float value2, float value3)
		{
			int index = GetUniformIndex(uniformName);
			GL.Uniform3(index, value1, value2, value3);
		}

		public void SetUniform(string uniformName, Vector4 value)
		{
			int index = GetUniformIndex(uniformName);
			GL.Uniform4(index, value);
		}

		public void SetUniform(string uniformName, float value1, float value2, float value3, float value4)
		{
			int index = GetUniformIndex(uniformName);
			GL.Uniform4(index, value1, value2, value3, value4);
		}

		public void SetUniform(string uniformName, Matrix4 matrix)
		{
			int index = GetUniformIndex(uniformName);
			GL.UniformMatrix4(index, false, ref matrix);
		}
	}
}
