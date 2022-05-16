using Epidesim.Engine.Drawing.Types;
using Epidesim.Engine.Drawing.Types.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace Epidesim.Engine.Drawing
{
	class TextureRendererEngine : Renderer
	{
		private ShaderProgram program;
		protected readonly VertexArrayObject vao;
		protected readonly VertexBufferObject vboVertices;
		protected readonly VertexBufferObject vboColors;
		protected readonly VertexBufferObject vboTexCoords;

		private float[] vertexBuffer, colorBuffer, texCoordBuffer;
		private int[] triangleIndexBuffer;

		public int MaxVertices { get; private set; }
		public int MaxTriangles { get; private set; }

		public int Vertices { get; private set; }
		public int Triangles { get; private set; }

		public TextureRendererEngine(int maxVertices, int maxTriangles, ShaderProgram program)
		{
			MaxVertices = maxVertices;
			MaxTriangles = maxTriangles;
			this.program = program;
			
			vertexBuffer = new float[maxVertices * 3];
			colorBuffer = new float[maxVertices * 4];
			texCoordBuffer = new float[maxVertices * 2];
			triangleIndexBuffer = new int[maxTriangles * 3];

			vboVertices = new DefaultVertexBufferObject(vertexBuffer.Length * sizeof(float), 3);
			vboColors = new DefaultVertexBufferObject(colorBuffer.Length * sizeof(float), 4);
			vboTexCoords = new DefaultVertexBufferObject(texCoordBuffer.Length * sizeof(float), 2);

			vao = new VertexArrayObject();
			vao.Bind();
			vao.SetVertexBuffer(vboVertices, 0);
			vao.SetVertexBuffer(vboColors, 1);
			vao.SetVertexBuffer(vboTexCoords, 2);
			vao.Unbind();
		}

		public void SetVertex(int index, float x, float y, float z, float r, float g, float b, float a, float tx, float ty)
		{
			vertexBuffer[index * 3 + 0] = x;
			vertexBuffer[index * 3 + 1] = y;
			vertexBuffer[index * 3 + 2] = z;

			colorBuffer[index * 4 + 0] = r;
			colorBuffer[index * 4 + 1] = g;
			colorBuffer[index * 4 + 2] = b;
			colorBuffer[index * 4 + 3] = a;

			texCoordBuffer[index * 2 + 0] = tx;
			texCoordBuffer[index * 2 + 1] = ty;
		}

		public void AddVertex(float x, float y, float z, float r, float g, float b, float a, float tx, float ty)
		{
			SetVertex(Vertices, x, y, z, r, g, b, a, tx, ty);
			Vertices++;
		}

		public void SetTriangle(int index, int i1, int i2, int i3)
		{
			triangleIndexBuffer[index * 3 + 0] = i1;
			triangleIndexBuffer[index * 3 + 1] = i2;
			triangleIndexBuffer[index * 3 + 2] = i3;
		}

		public void AddTriangle(int i1, int i2, int i3)
		{
			SetTriangle(Triangles, i1, i2, i3);
			Triangles++;
		}

		public override void Reset()
		{
			Vertices = 0;
			Triangles = 0;
		}

		public void DrawTexture(Texture2D texture)
		{
			vao.Bind();
			texture.Use(TextureUnit.Texture0);

			vboVertices.Bind();
			vboVertices.SetData(vertexBuffer, Vertices * 3);
			vao.EnableVertexBuffer(0);
			vboVertices.Unbind();

			vboColors.Bind();
			vboColors.SetData(colorBuffer, Vertices * 4);
			vao.EnableVertexBuffer(1);
			vboColors.Unbind();

			vboTexCoords.Bind();
			vboTexCoords.SetData(texCoordBuffer, Vertices * 2);
			vao.EnableVertexBuffer(2);
			vboTexCoords.Unbind();

			program.UseProgram();
			program.SetUniform("texture0", 0);
			program.SetUniform("transform", TransformMatrix);
			GL.DrawElements(PrimitiveType.Triangles, Triangles * 3, DrawElementsType.UnsignedInt, triangleIndexBuffer);

			vao.Unbind();
		}
	}
}
