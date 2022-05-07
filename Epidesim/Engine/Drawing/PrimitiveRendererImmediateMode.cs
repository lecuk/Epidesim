using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace Epidesim.Engine.Drawing
{
	internal class PrimitiveRendererImmediateMode : Renderer
	{
		public PrimitiveRendererImmediateMode()
			: base() { }

		public void DrawLine(Vector2d A, Vector2d B, Color4 color, float thickness = 1.0f)
		{
			GL.Begin(PrimitiveType.Lines);
			{
				GL.LineWidth(thickness);
				GL.Color4(color);
				GL.Vertex2(A);
				GL.Vertex2(B);
			}
			GL.End();
		}

		public void DrawPolygon(Vector2d center, double radius, int verticesCount, float rotation, Color4 borderColor, Color4 fillColor, float thickness)
		{
			double[] vertices = new double[verticesCount * 3];
			for (int i = 0; i < verticesCount; ++i)
			{
				double angle = Math.PI * 2 / verticesCount * i + rotation;
				vertices[i * 3 + 0] = center.X + Math.Cos(angle) * radius;
				vertices[i * 3 + 1] = center.Y + Math.Sin(angle) * radius;
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

			GL.Begin(PrimitiveType.Polygon);
			{
				GL.Color4(fillColor);
				for (int i = 0; i < verticesCount; ++i)
				{
					GL.Vertex2(vertices[i * 3 + 0], vertices[i * 3 + 1]);
				}
			}
			GL.End();

			GL.LineWidth(thickness);
			GL.Begin(PrimitiveType.LineLoop);
			{
				GL.Color4(borderColor);
				for (int i = 0; i < verticesCount; ++i)
				{
					GL.Vertex2(vertices[i * 3 + 0], vertices[i * 3 + 1]);
				}
			}
			GL.End();
		}

		public void DrawCircle(Vector2d center, double radius, Color4 borderColor, Color4 fillColor, float thickness)
		{
			DrawPolygon(center, radius, 64, 0, borderColor, fillColor, thickness);
		}
	}
}
