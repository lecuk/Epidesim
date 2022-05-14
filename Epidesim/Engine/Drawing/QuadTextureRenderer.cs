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

		public QuadTextureRenderer(int maxQuads, ShaderProgram program)
		{
			MaxQuads = maxQuads;

			engine = new TextureRendererEngine(maxQuads * 4, maxQuads * 2, program);
		}

		public override Matrix4 TransformMatrix
		{
			get => engine.TransformMatrix;
			set
			{
				engine.TransformMatrix = value;
			}
		}

		public void AddQuad(Rectangle rect, Color4 color)
		{
			engine.AddVertex(rect.Lft, rect.Bot, 0, color.R, color.G, color.B, color.A, 0, 1);
			engine.AddVertex(rect.Lft, rect.Top, 0, color.R, color.G, color.B, color.A, 0, 0);
			engine.AddVertex(rect.Rgt, rect.Top, 0, color.R, color.G, color.B, color.A, 1, 0);
			engine.AddVertex(rect.Rgt, rect.Bot, 0, color.R, color.G, color.B, color.A, 1, 1);

			int v = engine.Vertices;
			engine.AddTriangle(v - 4, v - 3, v - 2);
			engine.AddTriangle(v - 4, v - 2, v - 1);

			Quads++;
		}

		public void AddRotatedQuad(Vector2 center, float width, float height, float rotation, Color4 color)
		{
			Vector2 half = new Vector2(width, height) / 2;
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
