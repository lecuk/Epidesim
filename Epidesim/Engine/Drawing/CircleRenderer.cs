using Epidesim.Engine.Drawing.Types;
using Epidesim.Engine.Drawing.Types.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace Epidesim.Engine.Drawing
{
	class CircleRenderer
	{
		private VertexShader vertexShader;
		private FragmentShader fragmentShader;
		private ShaderProgram shaderProgram;

		private VertexArrayObject VAO;
		private VertexBufferObject VBO_Vertices;
		private VertexBufferObject VBO_Colors;
		private VertexBufferObject VBO_Radiuses;

		public CircleRenderer()
		{
			vertexShader = new VertexShader(@"Shaders/Circle/VertexShader.glsl");
			fragmentShader = new FragmentShader(@"Shaders/Circle/FragmentShader.glsl");
			shaderProgram = new ShaderProgram(vertexShader, fragmentShader);

			VBO_Vertices = new DefaultVertexBufferObject(1000000, 2);
			VBO_Colors = new DefaultVertexBufferObject(1000000, 4);
			VBO_Radiuses = new DefaultVertexBufferObject(1000000, 1);

			VAO = new VertexArrayObject();
			VAO.SetVertexBuffer(VBO_Vertices, 0);
			VAO.SetVertexBuffer(VBO_Colors, 1);
			VAO.SetVertexBuffer(VBO_Radiuses, 2);
		}

		public void DrawCircle(Vector2 center, double radius, Color4 fillColor)
		{
			shaderProgram.UseProgram();
		}

		public void DrawCircles(Vector2[] centers, float[] radiuses, Color4[] fillColors)
		{
			shaderProgram.UseProgram();

			float[] centerFloats = new float[centers.Length * 2];
			for (int i = 0; i < centers.Length; ++i)
			{
				centerFloats[i * 2 + 0] = centers[i].X;
				centerFloats[i * 2 + 1] = centers[i].Y;
			}

			float[] colorFloats = new float[fillColors.Length * 4];
			for (int i = 0; i < centers.Length; ++i)
			{
				colorFloats[i * 4 + 0] = fillColors[i].R;
				colorFloats[i * 4 + 1] = fillColors[i].G;
				colorFloats[i * 4 + 2] = fillColors[i].B;
				colorFloats[i * 4 + 3] = fillColors[i].A;
			}

			VBO_Vertices.Bind();
			VBO_Vertices.SetData(centerFloats);
			VAO.EnableVertexBuffer(0);
			VBO_Vertices.Unbind();

			VBO_Colors.Bind();
			VBO_Colors.SetData(colorFloats);
			VAO.EnableVertexBuffer(1);
			VBO_Colors.Unbind();

			VBO_Radiuses.Bind();
			VBO_Radiuses.SetData(radiuses);
			VAO.EnableVertexBuffer(2);
			VBO_Radiuses.Unbind();

			VAO.Bind();
			GL.DrawArrays(PrimitiveType.Points, 0, centers.Length);
			VAO.Unbind();
		}
	}
}
