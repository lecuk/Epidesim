using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Epidesim.Engine.Drawing
{
	class Model2D
	{
		private readonly VertexArrayObject VAO;
		private readonly VertexBufferObject VBO_Vertices;
		private readonly VertexBufferObject VBO_Colors;

		private Vector2[] vertices;
		private Color[] colors;
		private Matrix3 transformationMatrix;

		public Model2D()
		{
			VAO = new VertexArrayObject();
			VBO_Vertices = new VertexBufferObject(VertexAttribPointerType.Float, sizeof(float), 2, false, BufferUsageHint.StaticDraw, BufferTarget.ArrayBuffer, GetPName.ArrayBufferBinding);
			VBO_Colors = new VertexBufferObject(VertexAttribPointerType.Float, sizeof(float), 4, false, BufferUsageHint.StaticDraw, BufferTarget.ArrayBuffer, GetPName.ArrayBufferBinding);
			VAO.Bind();
			VAO.SetVertexBuffer(VBO_Vertices, 0);
			VAO.SetVertexBuffer(VBO_Colors, 1);
		}

		public void InitVertices(int size)
		{
			this.vertices = new Vector2[size];
		}

		public void SetVertex(int i, Vector2 vertex)
		{
			vertices[i] = vertex;
		}

		public void SetVertices(int i, Vector2[] vertices)
		{
			if (i + vertices.Length > this.vertices.Length)
			{
				throw new ArgumentOutOfRangeException("vertices");
			}

			System.Buffer.BlockCopy(vertices, 0, this.vertices, i, vertices.Length);
		}

		public void InitColors(int size)
		{
			this.colors = new Color[size];
		}

		public void SetColor(int i, Color color)
		{
			colors[i] = color;
		}

		public void SetColors(int i, Color[] colors)
		{
			if (i + vertices.Length > this.vertices.Length)
			{
				throw new ArgumentOutOfRangeException("vertices");
			}

			System.Buffer.BlockCopy(colors, 0, this.colors, i, colors.Length);
		}
	}
}
