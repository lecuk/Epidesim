using Epidesim.Engine.Drawing.Types;
using Epidesim.Engine.Drawing.Types.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace Epidesim.Engine.Drawing
{
	internal class PolygonRenderer : Renderer
	{
		private ShaderProgram program;
		protected readonly VertexArrayObject vao;
		protected readonly VertexBufferObject vboVertices;
		protected readonly VertexBufferObject vboColors;

		private readonly float[] vertexBuffer;
		private readonly float[] colorBuffer;
		private readonly int[] triangleIndexBuffer, lineIndexBuffer;
		private readonly int maxVertices, maxTriangles;

		private int verticesCount, trianglesCount;

		public PolygonRenderer(int maxVerticesCount, int maxTrianglesCount)
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
			lineIndexBuffer = new int[maxVerticesCount * 2];

			vboVertices = new VertexBufferObject(VertexAttribPointerType.Float, sizeof(float), 3, false, BufferUsageHint.StaticDraw, BufferTarget.ArrayBuffer, GetPName.ArrayBufferBinding);
			vboColors = new VertexBufferObject(VertexAttribPointerType.Float, sizeof(float), 4, false, BufferUsageHint.StaticDraw, BufferTarget.ArrayBuffer, GetPName.ArrayBufferBinding);

			vao = new VertexArrayObject();
			vao.Bind();
			vao.SetVertexBuffer(vboVertices, 0);
			vao.SetVertexBuffer(vboColors, 1);
			vao.Unbind();
		}

		public void AddPolygon(Vector2 center, float zIndex, double radius, int polygonVerticesCount, float rotation, Color4 color)
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

			if (this.trianglesCount + polygonTrianglesCount >= this.maxTriangles)
			{
				throw new OverflowException("Max triangle count reached.");
			}

			int pivotVertex = this.verticesCount;
			for (int i = 0; i < polygonVerticesCount; ++i)
			{
				double angle = Math.PI * 2 / polygonVerticesCount * i + rotation;

				this.vertexBuffer[this.verticesCount * 3 + 0] = (float)(center.X + Math.Cos(angle) * radius);
				this.vertexBuffer[this.verticesCount * 3 + 1] = (float)(center.Y + Math.Sin(angle) * radius);
				this.vertexBuffer[this.verticesCount * 3 + 2] = zIndex;

				this.colorBuffer[this.verticesCount * 4 + 0] = color.R;
				this.colorBuffer[this.verticesCount * 4 + 1] = color.G;
				this.colorBuffer[this.verticesCount * 4 + 2] = color.B;
				this.colorBuffer[this.verticesCount * 4 + 3] = color.A;

				this.lineIndexBuffer[this.verticesCount * 2 + 0] = pivotVertex + (i + 0) % polygonVerticesCount;
				this.lineIndexBuffer[this.verticesCount * 2 + 1] = pivotVertex + (i + 1) % polygonVerticesCount;

				this.verticesCount++;
			}

			for (int i = 0; i < polygonTrianglesCount; ++i)
			{
				this.triangleIndexBuffer[this.trianglesCount * 3 + 0] = pivotVertex;
				this.triangleIndexBuffer[this.trianglesCount * 3 + 1] = pivotVertex + i + 1;
				this.triangleIndexBuffer[this.trianglesCount * 3 + 2] = pivotVertex + i + 2;

				this.trianglesCount++;
			}
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

			var identity = Matrix4.Identity;
			GL.UniformMatrix4(program.GetUniformIndex("transform"), false, ref identity);
			GL.DrawElements(PrimitiveType.Triangles, trianglesCount * 3, DrawElementsType.UnsignedInt, triangleIndexBuffer);

			vao.Unbind();
		}

		public void Reset()
		{
			verticesCount = 0;
			trianglesCount = 0;
		}

		public void AddCircle(Vector2 center, float zIndex, double radius, Color4 color)
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
