using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace Epidesim.Engine.Drawing.Types
{
	class VertexArrayObject : IDisposable
	{
		private readonly int id;
		private readonly VertexBufferObject[] buffers;

		public static readonly int MaxVertexAttributes;

		static VertexArrayObject()
		{
			MaxVertexAttributes = GL.GetInteger(GetPName.MaxVertexAttribs);
		}

		public VertexArrayObject()
		{
			this.id = GL.GenVertexArray();
			this.buffers = new VertexBufferObject[MaxVertexAttributes];
		}

		public void Bind()
		{
			if (GL.GetInteger(GetPName.VertexArrayBinding) != 0)
				throw new InvalidOperationException("Another VAO is already bound");

			GL.BindVertexArray(this.id);
		}

		public void SetVertexBuffer(VertexBufferObject vbo, int index)
		{
			if (index < 0 || index >= MaxVertexAttributes)
				throw new ArgumentException(String.Format("{0} is out of range", nameof(index)));
			
			this.buffers[index] = vbo;
		}

		public VertexBufferObject GetBufferAtIndex(int index)
		{
			if (index < 0 || index >= MaxVertexAttributes)
				throw new ArgumentException(String.Format("{0} is out of range", nameof(index)));

			return this.buffers[index];
		}

		public void EnableVertexBuffer(int index)
		{
			VertexBufferObject vbo = this.buffers[index];

			GL.VertexAttribPointer(index, vbo.ComponentCount, vbo.Type, vbo.IsNormalized, vbo.Size * vbo.ComponentCount, 0);
			GL.EnableVertexAttribArray(index);
		}

		public void DisableVertexBuffer(int index)
		{
			GL.DisableVertexAttribArray(index);
		}

		public void Unbind()
		{
			if (GL.GetInteger(GetPName.VertexArrayBinding) != this.id)
				throw new InvalidOperationException("Attempt to unbind wrong VAO");

			GL.BindVertexArray(0);
		}

		public void Dispose()
		{
			GL.DeleteVertexArray(this.id);
		}
	}
}
