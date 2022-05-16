using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics;

using System;

namespace Epidesim.Engine.Drawing
{
	internal class PrimitiveRenderer : Renderer
	{
		private PrimitiveRendererEngine engine;

		public bool WireframeMode { get; set; }

		public PrimitiveRenderer(int maxVerticesCount, int maxTrianglesCount, int maxLinesCount)
		{
			engine = new PrimitiveRendererEngine(maxVerticesCount, maxTrianglesCount, maxLinesCount);
		}

		public override Matrix4 TransformMatrix
		{
			get => engine.TransformMatrix;
			set
			{
				engine.TransformMatrix = value;
			}
		}

		public void AddLine(Vector2 a, Vector2 b, Color4 color)
		{
			int p = engine.Vertices;

			engine.AddVertex(a.X, a.Y, 0, color.R, color.G, color.B, color.A);
			engine.AddVertex(b.X, b.Y, 0, color.R, color.G, color.B, color.A);

			engine.AddLine(p, p + 1);
		}

		public void AddRightPolygon(Vector2 center, float radius, int polygonVerticesCount, float rotation, Color4 color)
		{
			if (polygonVerticesCount < 3)
			{
				throw new ArgumentException("Invalid vertex count.");
			}

			int pivot = engine.Vertices;

			double anglePerVertex = Math.PI * 2 / polygonVerticesCount;
			for (int i = 0; i < polygonVerticesCount; ++i)
			{
				double angle = anglePerVertex * i + rotation;

				float sin = (float)Math.Sin(angle);
				float cos = (float)Math.Cos(angle);

				float x = center.X + cos * radius;
				float y = center.Y + sin * radius;

				int i0 = pivot + (i + 0) % polygonVerticesCount;
				int i1 = pivot + (i + 1) % polygonVerticesCount;

				engine.AddVertex(x, y, 0, color.R, color.G, color.B, color.A);

				if (!WireframeMode)
				{
					engine.AddLine(i0, i1);
				}
			}

			int polygonTrianglesCount = polygonVerticesCount - 2;

			for (int i = 0; i < polygonTrianglesCount; ++i)
			{
				int i0 = pivot;
				int i1 = pivot + i + 1;
				int i2 = pivot + i + 2;

				engine.AddTriangle(i0, i1, i2);

				if (WireframeMode)
				{
					engine.AddLine(i0, i1);
					engine.AddLine(i1, i2);
					engine.AddLine(i2, i0);
				}
			}
		}

		public void AddRectangle(Rectangle rect, Color4 color)
		{
			int p = engine.Vertices;

			engine.AddVertex(rect.Lft, rect.Bot, 0, color.R, color.G, color.B, color.A);
			engine.AddVertex(rect.Lft, rect.Top, 0, color.R, color.G, color.B, color.A);
			engine.AddVertex(rect.Rgt, rect.Top, 0, color.R, color.G, color.B, color.A);
			engine.AddVertex(rect.Rgt, rect.Bot, 0, color.R, color.G, color.B, color.A);

			engine.AddTriangle(p + 0, p + 1, p + 2);
			engine.AddTriangle(p + 0, p + 2, p + 3);
			
			engine.AddLine(p + 0, p + 1);
			engine.AddLine(p + 1, p + 2);
			engine.AddLine(p + 2, p + 3);
			engine.AddLine(p + 3, p + 0);
			
			if (WireframeMode)
			{
				engine.AddLine(p + 0, p + 2);
			}
		}

		public void AddTriangle(Vector2 v1, Vector2 v2, Vector2 v3, Color4 color)
		{
			int p = engine.Vertices;

			engine.AddVertex(v1.X, v1.Y, 0, color.R, color.G, color.B, color.A);
			engine.AddVertex(v2.X, v2.Y, 0, color.R, color.G, color.B, color.A);
			engine.AddVertex(v3.X, v3.Y, 0, color.R, color.G, color.B, color.A);

			engine.AddTriangle(p + 0, p + 1, p + 2);

			engine.AddLine(p + 0, p + 1);
			engine.AddLine(p + 1, p + 2);
			engine.AddLine(p + 2, p + 0);
		}

		public void AddCircle(Vector2 center, float radius, Color4 color)
		{
			AddRightPolygon(center, radius, 32, 0, color);
		}

		public void DrawFilledElements()
		{
			engine.DrawTriangles();
		}

		public void DrawHollowElements()
		{
			engine.DrawLines();
		}

		public override void Reset()
		{
			engine.Reset();
		}

		public override void Dispose()
		{
			base.Dispose();
			engine.Dispose();
		}
	}
}
