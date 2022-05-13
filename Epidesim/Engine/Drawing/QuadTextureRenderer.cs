using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics;

namespace Epidesim.Engine.Drawing
{
	class QuadTextureRenderer : Renderer
	{
		private TextureRendererEngine engine;

		public int Quads { get; private set; }
		public int MaxQuads { get; private set; }

		public QuadTextureRenderer(int maxQuads)
		{
			MaxQuads = maxQuads;

			engine = new TextureRendererEngine(maxQuads * 4, maxQuads * 2);
		}

		public override Matrix4 TransformMatrix
		{
			get => engine.TransformMatrix;
			set
			{
				engine.TransformMatrix = value;
			}
		}

		public void AddQuad(Vector2 a, Vector2 b, Color4 color)
		{
			engine.AddVertex(a.X, a.Y, 0, color.R, color.G, color.B, color.A, 0, 1);
			engine.AddVertex(a.X, b.Y, 0, color.R, color.G, color.B, color.A, 0, 0);
			engine.AddVertex(b.X, b.Y, 0, color.R, color.G, color.B, color.A, 1, 0);
			engine.AddVertex(b.X, a.Y, 0, color.R, color.G, color.B, color.A, 1, 1);

			int v = engine.Vertices;
			engine.AddTriangle(v - 4, v - 3, v - 2);
			engine.AddTriangle(v - 4, v - 2, v - 1);

			Quads++;
		}
		
		public void AddQuad(Vector2 center, float width, float height, Color4 color)
		{
			Vector2 half = new Vector2(width, height) / 2;

			AddQuad(center - half, center + half, color);
		}

		public void AddRotatedQuad(Vector2 center, float width, float height, float rotation, Color4 color)
		{
			Vector2 half = new Vector2(width, height) / 2;

			AddQuad(center - half, center + half, color);
		}

		public void Reset()
		{
			engine.Reset();
			Quads = 0;
		}

		public void DrawTexture(Texture2D texture)
		{
			engine.DrawTexture(texture);
		}
	}
}
