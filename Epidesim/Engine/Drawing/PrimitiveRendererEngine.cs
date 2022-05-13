using Epidesim.Engine.Drawing.Types;
using Epidesim.Engine.Drawing.Types.Shaders;
using System;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics;

namespace Epidesim.Engine.Drawing
{
	class PrimitiveRendererEngine : Renderer
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

		public PrimitiveRendererEngine(int maxVerticesCount, int maxTrianglesCount, int maxLinesCount)
			: base()
		{
			this.maxVertices = maxVerticesCount;
			this.maxTriangles = maxTrianglesCount;
			this.maxLines = maxLinesCount;

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
		
		public void AddVertex(float x, float y, float z, float r, float g, float b, float a)
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

		public void AddLineIndices(int i1, int i2)
		{
			this.lineIndexBuffer[this.linesCount * 2 + 0] = i1;
			this.lineIndexBuffer[this.linesCount * 2 + 1] = i2;

			linesCount++;
		}

		public void AddTriangleIndices(int i1, int i2, int i3)
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

		public void DrawFilledElements()
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
			
			program.SetUniform("transform", TransformMatrix);
			GL.DrawElements(PrimitiveType.Triangles, trianglesCount * 3, DrawElementsType.UnsignedInt, triangleIndexBuffer);

			vao.Unbind();
		}

		public void DrawHollowElements()
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
			GL.DrawElements(PrimitiveType.Lines, linesCount * 2, DrawElementsType.UnsignedInt, lineIndexBuffer);
			vao.Unbind();
		}

		public void Reset()
		{
			verticesCount = 0;
			trianglesCount = 0;
			linesCount = 0;
		}

		public override void Dispose()
		{
			base.Dispose();

			program.Dispose();
		}

		public int GetNextVertexIndex()
		{
			return verticesCount;
		}

		public int GetLastVertexIndex()
		{
			return verticesCount - 1;
		}

		public string GetDiagnosticDataString()
		{
			return String.Format("ver: {0}/{1}, tri: {2}/{3}, lin: {4}/{5}", verticesCount, maxVertices, trianglesCount, maxTriangles, linesCount, maxLines);
		}
	}
}
