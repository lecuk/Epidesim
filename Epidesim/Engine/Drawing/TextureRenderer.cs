using Epidesim.Engine.Drawing.Types;
using Epidesim.Engine.Drawing.Types.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace Epidesim.Engine.Drawing
{
	class TextureRenderer : Renderer
	{
		private ShaderProgram program;
		protected readonly VertexArrayObject vao;
		protected readonly VertexBufferObject vboVertices;
		protected readonly VertexBufferObject vboTexCoords;

		private readonly float[] vertexBuffer, texCoordBuffer;
		private readonly int maxVertices;

		public TextureRenderer(int maxVerticesCount)
		{
			this.maxVertices = maxVerticesCount;

			var vertexShader = new VertexShader(@"Shaders/Texture/VertexShader.glsl");
			var fragmentShader = new FragmentShader(@"Shaders/Texture/FragmentShader.glsl");
			this.program = new ShaderProgram(vertexShader, fragmentShader);

			vertexBuffer = new float[maxVerticesCount * 3];
			texCoordBuffer = new float[maxVerticesCount * 2];

			vboVertices = new VertexBufferObject(VertexAttribPointerType.Float, sizeof(float), 3, false, BufferUsageHint.StaticDraw, BufferTarget.ArrayBuffer, GetPName.ArrayBufferBinding);
			vboTexCoords = new VertexBufferObject(VertexAttribPointerType.Float, sizeof(float), 2, false, BufferUsageHint.StaticDraw, BufferTarget.ArrayBuffer, GetPName.ArrayBufferBinding);

			vao = new VertexArrayObject();
			vao.Bind();
			vao.SetVertexBuffer(vboVertices, 0);
			vao.SetVertexBuffer(vboTexCoords, 1);
			vao.Unbind();
		}

		public void DrawTexture(Texture2D texture, float x1, float y1, float x2, float y2, float z)
		{
			float[] vertices = new float[]
			{
				x1, y1, z,
				x1, y2,	z,
				x2, y2,	z,
				x2, y1,	z,
			};

			float[] texCoords = new float[]
			{
				0, 1,
				0, 0,
				1, 0,
				1, 1
			};

			int[] indices = new int[]
			{
				0, 1, 2,
				0, 2, 3
			};

			vao.Bind();
			texture.Use(TextureUnit.Texture0);

			vboVertices.Bind();
			vboVertices.SetData(vertices, vertices.Length);
			vao.EnableVertexBuffer(0);
			vboVertices.Unbind();

			vboTexCoords.Bind();
			vboTexCoords.SetData(texCoords, texCoords.Length);
			vao.EnableVertexBuffer(1);
			vboTexCoords.Unbind();

			program.UseProgram();
			program.SetUniform("texture0", 0);
			program.SetUniform("transform", TransformMatrix);
			GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, indices);
			
			vao.Unbind();
		}
	}
}
