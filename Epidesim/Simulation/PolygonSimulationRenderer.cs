using Epidesim.Engine.Drawing;
using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics;
using System;

namespace Epidesim.Simulation
{
	class PolygonSimulationRenderer : ISimulationRenderer<PolygonSimulation>, IDisposable
	{
		private readonly PrimitiveRenderer renderer;
		private readonly PrimitiveRendererImmediateMode rendererImmediate;
		private readonly CircleRenderer circleRenderer;

		public void Render(PolygonSimulation simulation)
		{
			//OpenTK.Graphics.OpenGL.GL.UseProgram(0);
			foreach (var polygon in simulation.Polygons)
			{
				renderer.DrawPolygon(polygon.Position, polygon.Radius, polygon.Edges, polygon.Rotation, polygon.FillColor, polygon.BorderColor, polygon.BorderThickness);
				//rendererImmediate.DrawPolygon(new Vector2d(polygon.Position.X, polygon.Position.Y), polygon.Radius, polygon.Edges, polygon.Rotation, polygon.FillColor, polygon.BorderColor, polygon.BorderThickness);
			}
		}

		public void Dispose()
		{
			renderer.Dispose();
		}

		public PolygonSimulationRenderer(ShaderProgram program)
		{
			renderer = new PrimitiveRenderer(program);
			rendererImmediate = new PrimitiveRendererImmediateMode();
			circleRenderer = new CircleRenderer();
		}
	}
}
