using Epidesim.Engine.Drawing.Types;
using Epidesim.Engine.Drawing.Types.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace Epidesim.Engine.Drawing
{
	class SingleTextureRendererEngine : Renderer
	{
		private ShaderProgram program;
		protected readonly VertexArrayObject vao;
		protected readonly VertexBufferObject vboVertices;
		protected readonly VertexBufferObject vboColors;
		protected readonly VertexBufferObject vboTexCoords;

		private float[] vertexBuffer, colorBuffer, texCoordBuffer;
		private int[] triangleIndexBuffer;

		private Texture2D texture;
		private int maxQuadsCount, quadsCount;
		
		public SingleTextureRendererEngine(Texture2D texture, int maxQuads)
		{
			this.texture = texture;
			this.maxQuadsCount = maxQuads;

			var vertexShader = new VertexShader(@"Shaders/Texture/VertexShader.glsl");
			var fragmentShader = new FragmentShader(@"Shaders/Texture/FragmentShader.glsl");
			this.program = new ShaderProgram(vertexShader, fragmentShader);

			int maxVertices = maxQuads * 4;

			// quad = 4 vertices
			vertexBuffer = new float[maxVertices * 3];
			colorBuffer = new float[maxVertices * 4];
			texCoordBuffer = new float[maxVertices * 2];
			triangleIndexBuffer = new int[maxQuads * 6];

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

		public void AddTextureQuad(float x1, float y1, float x2, float y2, Color4 color)
		{
			int v = quadsCount * 4;
			SetVertex(v + 0, x1, y1, 0, color.R, color.G, color.B, color.A, 0, 1);
			SetVertex(v + 1, x1, y2, 0, color.R, color.G, color.B, color.A, 0, 0);
			SetVertex(v + 2, x2, y2, 0, color.R, color.G, color.B, color.A, 1, 0);
			SetVertex(v + 3, x2, y1, 0, color.R, color.G, color.B, color.A, 1, 1);

			int t = quadsCount * 2;
			SetTriangle(t + 0, v + 0, v + 1, v + 2);
			SetTriangle(t + 1, v + 0, v + 2, v + 3);

			quadsCount++;
		}

		private void SetVertex(int index, float x, float y, float z, float r, float g, float b, float a, float tx, float ty)
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

		private void SetTriangle(int index, int i1, int i2, int i3)
		{
			triangleIndexBuffer[index * 3 + 0] = i1;
			triangleIndexBuffer[index * 3 + 1] = i2;
			triangleIndexBuffer[index * 3 + 2] = i3;
		}

		public void Reset()
		{
			quadsCount = 0;
		}

		public void DrawAll()
		{
			vao.Bind();
			texture.Use(TextureUnit.Texture0);

			int verticesCount = quadsCount * 4;
			int indicesCount = quadsCount * 2 * 3;

			vboVertices.Bind();
			vboVertices.SetData(vertexBuffer, verticesCount * 3);
			vao.EnableVertexBuffer(0);
			vboVertices.Unbind();

			vboColors.Bind();
			vboColors.SetData(colorBuffer, verticesCount * 4);
			vao.EnableVertexBuffer(1);
			vboColors.Unbind();

			vboTexCoords.Bind();
			vboTexCoords.SetData(texCoordBuffer, verticesCount * 2);
			vao.EnableVertexBuffer(2);
			vboTexCoords.Unbind();

			program.UseProgram();
			program.SetUniform("texture0", 0);
			program.SetUniform("transform", TransformMatrix);
			GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, triangleIndexBuffer);

			vao.Unbind();
		}
	}
}
