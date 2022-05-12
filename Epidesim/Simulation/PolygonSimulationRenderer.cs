using Epidesim.Engine.Drawing;
using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics;
using System;

namespace Epidesim.Simulation
{
	class PolygonSimulationRenderer : ISimulationRenderer<PolygonSimulation>, IDisposable
	{
		private readonly PolygonRenderer renderer;
		private readonly PrimitiveRendererImmediateMode rendererImmediate;
		private readonly CircleRenderer circleRenderer;

		public float ScreenWidth { get; set; }
		public float ScreenHeight { get; set; }

		public void Render(PolygonSimulation simulation)
		{
			float widthToHeight = ScreenWidth / ScreenHeight;
			renderer.Reset();
			renderer.TransformMatrix = Matrix4.Identity
				* Matrix4.CreateTranslation(simulation.OffsetX, simulation.OffsetY, 0)
				* Matrix4.CreateScale(simulation.Scale, simulation.Scale, 0)
				* Matrix4.CreateScale(1.0f / ScreenWidth, 1.0f / ScreenHeight, 1.0f / 1000);
			foreach (var polygon in simulation.Polygons)
			{
				renderer.AddPolygon(polygon.Position, polygon.ZIndex, polygon.Radius, polygon.Edges, polygon.Rotation, polygon.FillColor);
			}
			renderer.DrawEverything();
		}

		public void Dispose()
		{
			renderer.Dispose();
		}

		public PolygonSimulationRenderer(ShaderProgram program)
		{
			renderer = new PolygonRenderer(400000, 2000000);
			rendererImmediate = new PrimitiveRendererImmediateMode();
			circleRenderer = new CircleRenderer();
		}
	}
}
