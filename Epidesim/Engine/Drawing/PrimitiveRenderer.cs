using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace Epidesim.Engine.Drawing
{
	internal class PrimitiveRenderer : Renderer
	{
		private ShaderProgram program;

		public PrimitiveRenderer(ShaderProgram program)
			: base()
		{
			this.program = program;
		}

		public void DrawPolygon(Vector2 center, double radius, int verticesCount, float rotation, Color4 borderColor, Color4 fillColor, float thickness)
		{
			float[] vertices = new float[verticesCount * 3];
			for (int i = 0; i < verticesCount; ++i)
			{
				double angle = Math.PI * 2 / verticesCount * i + rotation;
				vertices[i * 3 + 0] = (float)(center.X + Math.Cos(angle) * radius);
				vertices[i * 3 + 1] = (float)(center.Y + Math.Sin(angle) * radius);
				vertices[i * 3 + 2] = 0;
			}

			float[] colors = new float[verticesCount * 4];
			for (int i = 0; i < verticesCount; ++i)
			{
				colors[i * 4 + 0] = fillColor.R;
				colors[i * 4 + 1] = fillColor.G;
				colors[i * 4 + 2] = fillColor.B;
				colors[i * 4 + 3] = fillColor.A;
			}

			uint[] indices = new uint[verticesCount];
			for (int i = 0; i < verticesCount; ++i)
			{
				indices[i] = (uint)i;
			}

			VAO.Bind();

			VBO_Vertices.Bind();
			VBO_Vertices.SetData(vertices);
			VAO.EnableVertexBuffer(0);
			VBO_Vertices.Unbind();

			GL.VertexAttrib4(1, new[] { fillColor.R, fillColor.G, fillColor.B, fillColor.A });
			GL.DrawArrays(PrimitiveType.Polygon, 0, verticesCount);

			GL.LineWidth(thickness);
			GL.VertexAttrib4(1, new[] { borderColor.R, borderColor.G, borderColor.B, borderColor.A });
			GL.DrawArrays(PrimitiveType.LineLoop, 0, verticesCount);

			VAO.Unbind();
		}

		public void DrawCircle(Vector2 center, double radius, Color4 borderColor, Color4 fillColor, float thickness)
		{
			DrawPolygon(center, radius, 64, 0, borderColor, fillColor, thickness);
		}
	}
}
