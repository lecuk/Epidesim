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

		public float ScreenWidth { get; set; }
		public float ScreenHeight { get; set; }

		public void Render(PolygonSimulation simulation)
		{
			float widthToHeight = ScreenWidth / ScreenHeight;

			renderer.WireframeMode = simulation.WireframeMode;
			renderer.Reset();

			renderer.TransformMatrix = Matrix4.Identity
				* Matrix4.CreateTranslation(simulation.OffsetX, simulation.OffsetY, 0)
				* Matrix4.CreateScale(simulation.Scale, simulation.Scale, 0)
				* Matrix4.CreateScale(1.0f / ScreenWidth, 1.0f / ScreenHeight, 1.0f / 1000);

			foreach (var polygon in simulation.Polygons)
			{
				renderer.AddRightPolygon(polygon.Position, polygon.ZIndex, polygon.Radius, polygon.Edges, polygon.Rotation, polygon.FillColor);
			}
			renderer.AddRightPolygon(simulation.ActualDirection * new Vector2(10, -10), 998, 15, 3, (float)Math.Atan2(-simulation.ActualDirection.Y, simulation.ActualDirection.X), Color4.Red);
			renderer.AddRightPolygon(simulation.TargetDirection * new Vector2(10, -10), 999, 20, 3, (float)Math.Atan2(-simulation.TargetDirection.Y, simulation.TargetDirection.X), Color4.Yellow);

			renderer.AddRectangle(0, 0, 50, 150, 2, Color4.Black);
			renderer.AddRectangle(0, 200, 50, 350, 2, Color4.Black);

			renderer.AddTriangle(50, 50, 55, 55, 50, 60, 1, Color4.White);

			if (simulation.WireframeMode)
			{
				renderer.DrawHollowElements();
			}
			else
			{
				renderer.DrawFilledElements();
			}
		}

		public void Dispose()
		{
			renderer.Dispose();
		}

		public PolygonSimulationRenderer()
		{
			renderer = new PrimitiveRenderer(400000, 2000000, 2000000);
			rendererImmediate = new PrimitiveRendererImmediateMode();
			circleRenderer = new CircleRenderer();
		}
	}
}
