using Epidesim.Engine.Drawing;
using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics;
using System;

namespace Epidesim.Simulation.Polygon
{
	class PolygonSimulationRenderer : ISimulationRenderer<PolygonSimulation>, IDisposable
	{
		private readonly PrimitiveRenderer renderer;
		private readonly PrimitiveRendererImmediateMode rendererImmediate;
		private readonly CircleRenderer circleRenderer;
		private readonly SingleTextureRendererEngine textureRenderer;

		public float ScreenWidth { get; set; }
		public float ScreenHeight { get; set; }

		Texture2D osuTexture;
		Texture2D prometheusTexture;
		Texture2D haloTexture;

		public void Render(PolygonSimulation simulation)
		{
			float widthToHeight = ScreenWidth / ScreenHeight;

			renderer.WireframeMode = simulation.WireframeMode;
			renderer.Reset();
			textureRenderer.Reset();

			var transformMatrix = Matrix4.Identity
				* Matrix4.CreateTranslation(simulation.OffsetX, simulation.OffsetY, 0)
				* Matrix4.CreateScale(simulation.Scale, simulation.Scale, 0)
				* Matrix4.CreateScale(1.0f / ScreenWidth, 1.0f / ScreenHeight, 1.0f / 1000);

			renderer.TransformMatrix = transformMatrix;
			textureRenderer.TransformMatrix = transformMatrix;

			foreach (var polygon in simulation.Polygons)
			{
				renderer.AddRightPolygon(polygon.Position, polygon.Radius, polygon.Edges, polygon.Rotation, polygon.FillColor);
				Vector2 a = polygon.Position - new Vector2(polygon.Radius, polygon.Radius) * 2;
				Vector2 b = polygon.Position + new Vector2(polygon.Radius, polygon.Radius) * 2;
				textureRenderer.AddTextureQuad(a.X, a.Y, b.X, b.Y, polygon.BorderColor);
			}
			renderer.AddRightPolygon(simulation.ActualDirection * new Vector2(10, -10), 15, 3, (float)Math.Atan2(-simulation.ActualDirection.Y, simulation.ActualDirection.X), Color4.Red);
			renderer.AddRightPolygon(simulation.TargetDirection * new Vector2(10, -10), 20, 3, (float)Math.Atan2(-simulation.TargetDirection.Y, simulation.TargetDirection.X), Color4.Yellow);

			renderer.AddRectangle(new Vector2(0, 0), new Vector2(50, 150), Color4.Black);
			renderer.AddRectangle(new Vector2(0, 200), new Vector2(50, 350), Color4.Black);

			renderer.AddTriangle(new Vector2(50, 50), new Vector2(55, 55), new Vector2(50, 60), Color4.White);

			textureRenderer.DrawAll();

			if (simulation.WireframeMode)
			{
				renderer.DrawHollowElements();
			}
			else
			{
				renderer.DrawFilledElements();
			}
			
			System.Diagnostics.Debug.WriteLine(String.Format("polygons: {0}", simulation.Polygons.Count));
		}

		public void Dispose()
		{
			renderer.Dispose();
		}

		public PolygonSimulationRenderer()
		{
			renderer = new PrimitiveRenderer(600000, 1000000, 2000000);
			rendererImmediate = new PrimitiveRendererImmediateMode();
			circleRenderer = new CircleRenderer();
			osuTexture = Texture2D.Load("Resources/osu.png");
			prometheusTexture = Texture2D.Load("Resources/prometheus.jpg");
			haloTexture = Texture2D.Load("Resources/halo.png");
			textureRenderer = new SingleTextureRendererEngine(haloTexture, 40000);
		}
	}
}
