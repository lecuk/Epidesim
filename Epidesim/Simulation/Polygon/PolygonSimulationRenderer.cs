using Epidesim.Engine;
using Epidesim.Engine.Drawing;
using Epidesim.Engine.Drawing.Types;
using Epidesim.Engine.Drawing.Types.Shaders;
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
		private readonly QuadTextureRenderer textureRenderer;
		private readonly QuadTextureRenderer backgroundRenderer;
		private readonly TextRenderer textRenderer;

		public void Render(PolygonSimulation simulation)
		{
			primitiveRenderer.WireframeMode = simulation.WireframeMode;
			wireframeRenderer.Reset();
			primitiveRenderer.Reset();
			textureRenderer.Reset();
			selectionRectangleRenderer.Reset();
			textRenderer.Reset();
			backgroundRenderer.Reset();

			var transformMatrix = simulation.CurrentCoordinateSystem.GetTransformationMatrix();
			var space = ResourceManager.GetTexture("space");

			primitiveRenderer.TransformMatrix = transformMatrix;
			textureRenderer.TransformMatrix = transformMatrix;
			wireframeRenderer.TransformMatrix = transformMatrix;
			selectionRectangleRenderer.TransformMatrix = transformMatrix;
			textRenderer.TransformMatrix = transformMatrix;
			backgroundRenderer.TransformMatrix = transformMatrix;

			backgroundRenderer.AddQuad(Rectangle.FromTwoPoints(-new Vector2(space.Width, space.Height) / 2, new Vector2(space.Width, space.Height) * 2), Color4.White);

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
				primitiveRenderer.AddRightPolygon(simulation.WorldMousePosition, 
					simulation.PolygonToCreate.Radius, 
					simulation.PolygonToCreate.Edges, 
					simulation.PolygonToCreate.Rotation, 
					Color4.White);
			}

			var v2 = new Vector2();

			textRenderer.AddString("hello world!", 40, v2 - new Vector2(2), Color4.Black);
			textRenderer.AddString("hello world!", 40, v2, Color4.White);
			textRenderer.AddString("abcd\nefghijkl\nmnopqrstuvw\nxyz", 40, v2 - new Vector2(2, 42), Color4.Black);
			textRenderer.AddString("abcd\nefghijkl\nmnopqrstuvw\nxyz", 40, v2 - new Vector2(0, 40), Color4.Lime);

			for (int i = 0; i < 17; ++i)
			{
				primitiveRenderer.AddRightPolygon(new Vector2(-50, i * 20),
					8,
					3 + i,
					0,
					Color4.White);

				primitiveRenderer.AddRightPolygon(new Vector2(-100, i * 20),
					8,
					4,
					(i * 15 + 45) * (float)Math.PI / 180,
					Color4.White);
			}

			backgroundRenderer.DrawTexture(space);

			if (simulation.WireframeMode)
			{
				wireframeRenderer.DrawHollowElements();
				primitiveRenderer.DrawHollowElements();
				textRenderer.DrawWireframe();
			}
			else
			{
				textureRenderer.DrawTexture(ResourceManager.GetTexture("halo"));
				primitiveRenderer.DrawFilledElements();
				textRenderer.DrawAll();
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
			wireframeRenderer = new PrimitiveRenderer(200000, 400000, 600000) { WireframeMode = true };
			selectionRectangleRenderer = new PrimitiveRenderer(4, 2, 4) { WireframeMode = false };
			textureRenderer = new QuadTextureRenderer(60000, ResourceManager.GetProgram("textureDefault"));
			textRenderer = new TextRenderer(1000);
			backgroundRenderer = new QuadTextureRenderer(60000, ResourceManager.GetProgram("textureDefault"));
			textRenderer.LoadFont(ResourceManager.GetTextureFont("consolas"));
		}
	}
}
