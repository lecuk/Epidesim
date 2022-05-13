using Epidesim.Engine.Drawing.Types;
using Epidesim.Engine.Drawing.Types.Shaders;
using OpenTK.Graphics.OpenGL;
using System;

namespace Epidesim.Engine.Drawing
{
	internal class PrimitiveRendererEngine : Renderer
	{
		private ShaderProgram program;
		protected readonly VertexArrayObject vao;
		protected readonly VertexBufferObject vboVertices;
		protected readonly VertexBufferObject vboColors;

		private readonly float[] vertexBuffer;
		private readonly float[] colorBuffer;
		private readonly int[] triangleIndexBuffer, lineIndexBuffer;

		public int MaxVertices { get; private set; }
		public int MaxTriangles { get; private set; }
		public int MaxLines { get; private set; }

		public int Vertices { get; private set; }
		public int Triangles { get; private set; }
		public int Lines { get; private set; }

		public PrimitiveRendererEngine(int maxVerticesCount, int maxTrianglesCount, int maxLinesCount)
			: base()
		{
			MaxVertices = maxVerticesCount;
			MaxTriangles = maxTrianglesCount;
			MaxLines = maxLinesCount;

			var vertexShader = new VertexShader(@"Shaders/Simple/VertexShader.glsl");
			var fragmentShader = new FragmentShader(@"Shaders/Simple/FragmentShader.glsl");
			this.program = new ShaderProgram(vertexShader, fragmentShader);

			vertexBuffer = new float[maxVerticesCount * 3];
			colorBuffer = new float[maxVerticesCount * 4];
			triangleIndexBuffer = new int[maxTrianglesCount * 3];
			lineIndexBuffer = new int[maxLinesCount * 2];

			vboVertices = new DefaultVertexBufferObject(vertexBuffer.Length * sizeof(float), 3);
			vboColors = new DefaultVertexBufferObject(colorBuffer.Length * sizeof(float), 4);

			vao = new VertexArrayObject();
			vao.Bind();
			vao.SetVertexBuffer(vboVertices, 0);
			vao.SetVertexBuffer(vboColors, 1);
			vao.Unbind();

			Reset();
		}

		public void SetVertex(int index, float x, float y, float z, float r, float g, float b, float a)
		{
			this.vertexBuffer[index * 3 + 0] = x;
			this.vertexBuffer[index * 3 + 1] = y;
			this.vertexBuffer[index * 3 + 2] = z;

			this.colorBuffer[index * 4 + 0] = r;
			this.colorBuffer[index * 4 + 1] = g;
			this.colorBuffer[index * 4 + 2] = b;
			this.colorBuffer[index * 4 + 3] = a;
		}

		public void AddVertex(float x, float y, float z, float r, float g, float b, float a)
		{
			SetVertex(this.Vertices, x, y, z, r, g, b, a);
			this.Vertices++;
		}

		public void SetTriangle(int index, int i1, int i2, int i3)
		{
			this.triangleIndexBuffer[index * 3 + 0] = i1;
			this.triangleIndexBuffer[index * 3 + 1] = i2;
			this.triangleIndexBuffer[index * 3 + 2] = i3;
		}

		public void AddTriangle(int i1, int i2, int i3)
		{
			SetTriangle(this.Triangles, i1, i2, i3);
			Triangles++;
		}

		public void SetLine(int index, int i1, int i2)
		{
			this.lineIndexBuffer[this.Lines * 2 + 0] = i1;
			this.lineIndexBuffer[this.Lines * 2 + 1] = i2;
		}

		public void AddLine(int i1, int i2)
		{
			SetLine(this.Lines, i1, i2);
			this.Lines++;
		}

		public void DrawTriangles()
		{
			vao.Bind();

			vboVertices.Bind();
			vboVertices.SetData(vertexBuffer, Vertices * 3);
			vao.EnableVertexBuffer(0);
			vboVertices.Unbind();

			vboColors.Bind();
			vboColors.SetData(colorBuffer, Vertices * 4);
			vao.EnableVertexBuffer(1);
			vboColors.Unbind();

			program.UseProgram();
			program.SetUniform("transform", TransformMatrix);

			GL.DrawElements(PrimitiveType.Triangles, Triangles * 3, DrawElementsType.UnsignedInt, triangleIndexBuffer);

			vao.Unbind();
		}

		public void DrawLines()
		{
			vao.Bind();

			vboVertices.Bind();
			vboVertices.SetData(vertexBuffer, Vertices * 3);
			vao.EnableVertexBuffer(0);
			vboVertices.Unbind();

			vboColors.Bind();
			vboColors.SetData(colorBuffer, Vertices * 4);
			vao.EnableVertexBuffer(1);
			vboColors.Unbind();

			program.UseProgram();
			program.SetUniform("transform", TransformMatrix);

			GL.DrawElements(PrimitiveType.Lines, Lines * 2, DrawElementsType.UnsignedInt, lineIndexBuffer);

			vao.Unbind();
		}

		public void Reset()
		{
			Vertices = 0;
			Triangles = 0;
			Lines = 0;
		}

		public override void Dispose()
		{
			base.Dispose();

			program.Dispose();
		}
	}
}
