using Epidesim.Engine.Drawing.Types;
using Epidesim.Engine.Drawing.Types.Shaders;
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
		protected readonly VertexArrayObject vao;
		protected readonly VertexBufferObject vboVertices;
		protected readonly VertexBufferObject vboColors;

		private readonly float[] vertexBuffer;
		private readonly float[] colorBuffer;
		private readonly int[] triangleIndexBuffer, lineIndexBuffer;
		private readonly int maxVertices, maxTriangles, maxLines;

		private int verticesCount, trianglesCount, linesCount;

		public PrimitiveRenderer(int maxVerticesCount, int maxTrianglesCount, int maxLinesCount)
			: base()
		{
			this.maxVertices = maxVerticesCount;
			this.maxTriangles = maxTrianglesCount;

			var vertexShader = new VertexShader(@"Shaders/Simple/VertexShader.glsl");
			var fragmentShader = new FragmentShader(@"Shaders/Simple/FragmentShader.glsl");
			this.program = new ShaderProgram(vertexShader, fragmentShader);
			
			vertexBuffer = new float[maxVerticesCount * 3];
			colorBuffer = new float[maxVerticesCount * 4];
			triangleIndexBuffer = new int[maxTrianglesCount * 3];
			lineIndexBuffer = new int[maxLinesCount * 2];

			vboVertices = new VertexBufferObject(VertexAttribPointerType.Float, sizeof(float), 3, false, BufferUsageHint.StaticDraw, BufferTarget.ArrayBuffer, GetPName.ArrayBufferBinding);
			vboColors = new VertexBufferObject(VertexAttribPointerType.Float, sizeof(float), 4, false, BufferUsageHint.StaticDraw, BufferTarget.ArrayBuffer, GetPName.ArrayBufferBinding);

			vao = new VertexArrayObject();
			vao.Bind();
			vao.SetVertexBuffer(vboVertices, 0);
			vao.SetVertexBuffer(vboColors, 1);
			vao.Unbind();
		}

		public void AddPolygon(Vector2 center, float z, float radius, int polygonVerticesCount, float rotation, Color4 color)
		{
			if (polygonVerticesCount < 3)
			{
				throw new ArgumentException("Invalid vertex count.");
			}

			if (this.verticesCount + polygonVerticesCount >= this.maxVertices)
			{
				throw new OverflowException("Max vertex count reached.");
			}

			int polygonTrianglesCount = polygonVerticesCount - 2;

			int pivotVertex = this.verticesCount;
			for (int i = 0; i < polygonVerticesCount; ++i)
			{
				double angle = Math.PI * 2 / polygonVerticesCount * i + rotation;

				float x = center.X + (float)Math.Cos(angle) * radius;
				float y = center.Y + (float)Math.Sin(angle) * radius;

				int i1 = pivotVertex + (i + 0) % polygonVerticesCount;
				int i2 = pivotVertex + (i + 1) % polygonVerticesCount;

				AddVertex(x, y, z, color.R, color.G, color.B, color.A);
				AddLineIndices(i1, i2);
			}

			for (int i = 0; i < polygonTrianglesCount; ++i)
			{
				int i1 = pivotVertex;
				int i2 = pivotVertex + i + 1;
				int i3 = pivotVertex + i + 2;

				AddTriangleIndices(i1, i2, i3);
			}
		}

		public void AddRectangle(float x1, float y1, float x2, float y2, Color4 color)
		{
		}

		public void AddTriangle(float x1, float y1, float x2, float y2, float x3, float y3, Color4 color)
		{
			this.triangleIndexBuffer[this.trianglesCount * 3 + 0] = this.trianglesCount * 3 + 0;
			this.triangleIndexBuffer[this.trianglesCount * 3 + 1] = this.trianglesCount * 3 + 1;
			this.triangleIndexBuffer[this.trianglesCount * 3 + 2] = this.trianglesCount * 3 + 2;

			this.trianglesCount++;
		}

		private void AddVertex(float x, float y, float z, float r, float g, float b, float a)
		{
			this.vertexBuffer[this.verticesCount * 3 + 0] = x;
			this.vertexBuffer[this.verticesCount * 3 + 1] = y;
			this.vertexBuffer[this.verticesCount * 3 + 2] = z;

			this.colorBuffer[this.verticesCount * 4 + 0] = r;
			this.colorBuffer[this.verticesCount * 4 + 1] = g;
			this.colorBuffer[this.verticesCount * 4 + 2] = b;
			this.colorBuffer[this.verticesCount * 4 + 3] = a;

			this.verticesCount++;
		}

		private void AddLineIndices(int i1, int i2)
		{
			this.lineIndexBuffer[this.linesCount * 2 + 0] = i1;
			this.lineIndexBuffer[this.linesCount * 2 + 1] = i2;

			linesCount++;
		}

		private void AddTriangleIndices(int i1, int i2, int i3)
		{
			if (this.trianglesCount >= this.maxTriangles)
			{
				throw new OverflowException("Max triangle count reached.");
			}

			this.triangleIndexBuffer[this.trianglesCount * 3 + 0] = i1;
			this.triangleIndexBuffer[this.trianglesCount * 3 + 1] = i2;
			this.triangleIndexBuffer[this.trianglesCount * 3 + 2] = i3;

			trianglesCount++;
		}

		public void DrawEverything()
		{
			program.UseProgram();

			vao.Bind();

			vboVertices.Bind();
			vboVertices.SetData(vertexBuffer, verticesCount * 3);
			vao.EnableVertexBuffer(0);
			vboVertices.Unbind();

			vboColors.Bind();
			vboColors.SetData(colorBuffer, verticesCount * 4);
			vao.EnableVertexBuffer(1);
			vboColors.Unbind();

			var transform = TransformMatrix;
			GL.UniformMatrix4(program.GetUniformIndex("transform"), false, ref transform);
			GL.DrawElements(PrimitiveType.Triangles, trianglesCount * 3, DrawElementsType.UnsignedInt, triangleIndexBuffer);

			vao.Unbind();
		}

		public void Reset()
		{
			verticesCount = 0;
			trianglesCount = 0;
		}

		public void AddCircle(Vector2 center, float zIndex, float radius, Color4 color)
		{
			AddPolygon(center, zIndex, radius, 64, 0, color);
		}

		public override void Dispose()
		{
			base.Dispose();

			program.Dispose();
		}
	}
}
