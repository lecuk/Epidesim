using Epidesim.Engine.Drawing.Types;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace Epidesim.Engine.Drawing
{
	abstract class Renderer : IDisposable
	{
		protected readonly VertexArrayObject VAO;

		protected readonly VertexBufferObject VBO_Vertices;
		protected readonly VertexBufferObject VBO_Colors;
		protected readonly VertexBufferObject VBO_Indices;
		
		public void Dispose()
		{
			VBO_Vertices.Dispose();
			VBO_Colors.Dispose();
			VBO_Indices.Dispose();
			VAO.Dispose();
		}

		public Renderer()
		{
			VBO_Vertices = new VertexBufferObject(VertexAttribPointerType.Float, sizeof(float), 3, true, BufferUsageHint.StaticDraw, BufferTarget.ArrayBuffer, GetPName.ArrayBufferBinding);
			VBO_Colors = new VertexBufferObject(VertexAttribPointerType.Float, sizeof(float), 4, true, BufferUsageHint.StaticDraw, BufferTarget.ArrayBuffer, GetPName.ArrayBufferBinding);
			VBO_Indices = new VertexBufferObject(VertexAttribPointerType.Int, sizeof(int), 1, false, BufferUsageHint.StaticDraw, BufferTarget.ElementArrayBuffer, GetPName.ElementArrayBufferBinding);

			VAO = new VertexArrayObject();
			VAO.Bind();
			VAO.SetVertexBuffer(VBO_Vertices, 0);
			VAO.SetVertexBuffer(VBO_Colors, 1);
			VAO.SetVertexBuffer(VBO_Indices, 2);
			VAO.Unbind();
		}
	}
}
