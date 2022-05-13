using Epidesim.Engine.Drawing;
using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Diagnostics;

namespace Epidesim.Simulation.Polygon
{
	class PolygonSimulationRenderer : ISimulationRenderer<PolygonSimulation>, IDisposable
	{
		private readonly PrimitiveRenderer primitiveRenderer;
		private readonly PrimitiveRenderer selectionRectangleRenderer;
		private readonly PrimitiveRenderer wireframeRenderer;
		private readonly PrimitiveRendererImmediateMode rendererImmediate;
		private readonly CircleRenderer circleRenderer;
		private readonly QuadTextureRenderer textureRenderer;

		Texture2D osuTexture;
		Texture2D prometheusTexture;
		Texture2D haloTexture;

		public void Render(PolygonSimulation simulation)
		{
			primitiveRenderer.WireframeMode = simulation.WireframeMode;
			wireframeRenderer.Reset();
			primitiveRenderer.Reset();
			textureRenderer.Reset();
			selectionRectangleRenderer.Reset();

			var transformMatrix = simulation.CurrentCoordinateSystem.GetTransformationMatrix();

			primitiveRenderer.TransformMatrix = transformMatrix;
			textureRenderer.TransformMatrix = transformMatrix;
			wireframeRenderer.TransformMatrix = transformMatrix;
			selectionRectangleRenderer.TransformMatrix = transformMatrix;

			foreach (var polygon in simulation.UnSelectedPolygons)
			{
				primitiveRenderer.AddRightPolygon(polygon.Position, polygon.Radius, polygon.Edges, polygon.Rotation, polygon.FillColor);
			}

			foreach (var polygon in simulation.SelectedPolygons)
			{
				primitiveRenderer.AddRightPolygon(polygon.Position, polygon.Radius, polygon.Edges, polygon.Rotation, Color4.White);
			}

			foreach (var polygon in simulation.SelectedPolygons)
			{
				var rect = Rectangle.FromCenterAndSize(polygon.Position, new Vector2(polygon.Radius * 4));
				textureRenderer.AddQuad(rect, polygon.FillColor);
				wireframeRenderer.AddRectangle(rect, polygon.FillColor);
			}
			
			if (simulation.IsSelecting)
			{
				selectionRectangleRenderer.AddRectangle(simulation.SelectionRectangle, Color4.Lime);
			}
			
			if (simulation.CreateMode)
			{
				primitiveRenderer.AddRightPolygon(simulation.MouseWorldPosition, 
					simulation.PolygonToCreate.Radius, 
					simulation.PolygonToCreate.Edges, 
					simulation.PolygonToCreate.Rotation, 
					Color4.White);
			}

			if (simulation.WireframeMode)
			{
				wireframeRenderer.DrawHollowElements();
				primitiveRenderer.DrawHollowElements();
			}
			else
			{
				textureRenderer.DrawTexture(haloTexture);
				primitiveRenderer.DrawFilledElements();
			}

			selectionRectangleRenderer.DrawHollowElements();
		}

		public void Dispose()
		{
			primitiveRenderer.Dispose();
			textureRenderer.Dispose();
			wireframeRenderer.Dispose();
		}

		public PolygonSimulationRenderer()
		{
			primitiveRenderer = new PrimitiveRenderer(600000, 1000000, 2000000) { WireframeMode = false };
			wireframeRenderer = new PrimitiveRenderer(600000, 1000000, 2000000) { WireframeMode = true };
			selectionRectangleRenderer = new PrimitiveRenderer(4, 2, 4) { WireframeMode = false };
			rendererImmediate = new PrimitiveRendererImmediateMode();
			circleRenderer = new CircleRenderer();
			osuTexture = Texture2D.Load("Resources/osu.png");
			prometheusTexture = Texture2D.Load("Resources/prometheus.jpg");
			haloTexture = Texture2D.Load("Resources/halo.png");
			textureRenderer = new QuadTextureRenderer(60000);
		}
	}
}
