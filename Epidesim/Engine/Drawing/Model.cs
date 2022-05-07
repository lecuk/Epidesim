using Epidesim.Engine.Drawing.Types;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Epidesim.Engine.Drawing
{
	class Model
	{
		private readonly VertexArrayObject VAO;
		private readonly VertexBufferObject VBO_Vertices;
		private readonly VertexBufferObject VBO_Colors;

		private Vector3[] vertices;
		private Color[] colors;

		public Model()
		{

		}

		public void InitVertices(int size)
		{
			this.vertices = new Vector3[size * 3];
		}

		public void SetVertex(int i, Vector3 vertex)
		{
			vertices[i] = vertex;
		}

		public void SetVertices(int i, Vector3[] vertices)
		{
			if (i + vertices.Length > this.vertices.Length)
			{
				throw new ArgumentOutOfRangeException("vertices");
			}

			Buffer.BlockCopy(vertices, 0, this.vertices, i, vertices.Length);
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

			Buffer.BlockCopy(colors, 0, this.colors, i, colors.Length);
		}
	}
}
