using OpenTK;
using OpenTK.Graphics;

using System;

namespace Epidesim.Engine.Drawing
{
	internal class PrimitiveRenderer : Renderer
	{
		private PrimitiveRendererEngine engine;

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

		public void AddRightPolygon(Vector2 center, float z, float radius, int polygonVerticesCount, float rotation, Color4 color)
		{
			if (polygonVerticesCount < 3)
			{
				throw new ArgumentException("Invalid vertex count.");
			}

			int pivot = engine.GetNextVertexIndex();

			for (int i = 0; i < polygonVerticesCount; ++i)
			{
				double angle = Math.PI * 2 / polygonVerticesCount * i + rotation;

				float x = center.X + (float)Math.Cos(angle) * radius;
				float y = center.Y + (float)Math.Sin(angle) * radius;

				int i1 = pivot + (i + 0) % polygonVerticesCount;
				int i2 = pivot + (i + 1) % polygonVerticesCount;

				engine.AddVertex(x, y, z, color.R, color.G, color.B, color.A);
				engine.AddLineIndices(i1, i2);
			}

			int polygonTrianglesCount = polygonVerticesCount - 2;

			for (int i = 0; i < polygonTrianglesCount; ++i)
			{
				int i1 = pivot;
				int i2 = pivot + i + 1;
				int i3 = pivot + i + 2;

				engine.AddTriangleIndices(i1, i2, i3);
			}
		}

		public void AddRectangle(float x1, float y1, float x2, float y2, float z, Color4 color)
		{
			int p = engine.GetNextVertexIndex();

			engine.AddVertex(x1, y1, z, color.R, color.G, color.B, color.A);
			engine.AddVertex(x1, y2, z, color.R, color.G, color.B, color.A);
			engine.AddVertex(x2, y2, z, color.R, color.G, color.B, color.A);
			engine.AddVertex(x2, y1, z, color.R, color.G, color.B, color.A);

			engine.AddTriangleIndices(p + 0, p + 1, p + 2);
			engine.AddTriangleIndices(p + 0, p + 2, p + 3);
			
			engine.AddLineIndices(p + 0, p + 1);
			engine.AddLineIndices(p + 1, p + 2);
			engine.AddLineIndices(p + 2, p + 3);
			engine.AddLineIndices(p + 3, p + 0);
		}

		public void AddTriangle(float x1, float y1, float x2, float y2, float x3, float y3, float z, Color4 color)
		{
			int p = engine.GetNextVertexIndex();

			engine.AddVertex(x1, y1, z, color.R, color.G, color.B, color.A);
			engine.AddVertex(x2, y2, z, color.R, color.G, color.B, color.A);
			engine.AddVertex(x3, y3, z, color.R, color.G, color.B, color.A);

			engine.AddTriangleIndices(p + 0, p + 1, p + 2);

			engine.AddLineIndices(p + 0, p + 1);
			engine.AddLineIndices(p + 1, p + 2);
			engine.AddLineIndices(p + 2, p + 0);
		}

		public void AddCircle(Vector2 center, float zIndex, float radius, Color4 color)
		{
			AddRightPolygon(center, zIndex, radius, 64, 0, color);
		}

		public void DrawFilledElements()
		{
			engine.DrawFilledElements();
		}

		public void DrawHollowElements()
		{
			engine.DrawHollowElements();
		}

		public void Reset()
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
