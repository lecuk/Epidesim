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

		public void Render(PolygonSimulation simulation)
		{
			renderer.Reset();
			float zIndex = 0;
			foreach (var polygon in simulation.Polygons)
			{
				//rendererImmediate.DrawPolygon(new Vector2d(polygon.Position.X, polygon.Position.Y), polygon.Radius, polygon.Edges, polygon.Rotation, polygon.FillColor, polygon.BorderColor, polygon.BorderThickness);
				renderer.AddPolygon(polygon.Position, zIndex, polygon.Radius, polygon.Edges, polygon.Rotation, polygon.FillColor);
				zIndex += 0.01f;
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
