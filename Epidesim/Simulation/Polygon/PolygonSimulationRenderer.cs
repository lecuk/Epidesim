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
		private readonly TextRenderer textRenderer;

		Texture2D haloTexture;

		public void Render(PolygonSimulation simulation)
		{
			primitiveRenderer.WireframeMode = simulation.WireframeMode;
			wireframeRenderer.Reset();
			primitiveRenderer.Reset();
			textureRenderer.Reset();
			selectionRectangleRenderer.Reset();
			textRenderer.Reset();

			var transformMatrix = simulation.CurrentCoordinateSystem.GetTransformationMatrix();

			primitiveRenderer.TransformMatrix = transformMatrix;
			textureRenderer.TransformMatrix = transformMatrix;
			wireframeRenderer.TransformMatrix = transformMatrix;
			selectionRectangleRenderer.TransformMatrix = transformMatrix;
			textRenderer.TransformMatrix = transformMatrix;

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

			var v2 = new Vector2();

			primitiveRenderer.AddRightPolygon(v2,
				simulation.PolygonToCreate.Radius,
				simulation.PolygonToCreate.Edges,
				simulation.PolygonToCreate.Rotation,
				Color4.White);

			textRenderer.AddString("hello world!", ref v2, Color4.White);

			primitiveRenderer.AddRightPolygon(v2,
				simulation.PolygonToCreate.Radius,
				simulation.PolygonToCreate.Edges,
				simulation.PolygonToCreate.Rotation,
				Color4.White);

			textRenderer.AddString("abcd\nefghijkl\nmnopqrstuvw\nxyz", ref v2, Color4.Lime);

			primitiveRenderer.AddRightPolygon(v2,
				simulation.PolygonToCreate.Radius,
				simulation.PolygonToCreate.Edges,
				simulation.PolygonToCreate.Rotation,
				Color4.White);

			if (simulation.WireframeMode)
			{
				wireframeRenderer.DrawHollowElements();
				primitiveRenderer.DrawHollowElements();
				textRenderer.DrawWireframe();
			}
			else
			{
				textureRenderer.DrawTexture(haloTexture);
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
			InitShaders();

			primitiveRenderer = new PrimitiveRenderer(600000, 1000000, 2000000) { WireframeMode = false };
			wireframeRenderer = new PrimitiveRenderer(200000, 400000, 600000) { WireframeMode = true };
			selectionRectangleRenderer = new PrimitiveRenderer(4, 2, 4) { WireframeMode = false };
			haloTexture = Texture2DLoader.LoadFromFile("Resources/halo.png");
			textureRenderer = new QuadTextureRenderer(60000, ShaderProgramManager.GetProgram("textureDefault"));
			textRenderer = new TextRenderer();

			string alphabet = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.,;/\\()<>{}[]+-=|!?\"\'";
			var font = TextureFontGenerator.Generate("Resources/consolas.ttf", alphabet.ToCharArray());
			textRenderer.LoadFont(font);
		}

		private void InitShaders()
		{
			ShaderProgramManager.AddProgram("primitive", new ShaderProgram(
				VertexShader.Load("Shaders/Simple/VertexShader.glsl"),
				FragmentShader.Load("Shaders/Simple/FragmentShader.glsl")));

			ShaderProgramManager.AddProgram("textureDefault", new ShaderProgram(
				VertexShader.Load("Shaders/Texture/VertexShader.glsl"),
				FragmentShader.Load("Shaders/Texture/FragmentShader.glsl")));

			ShaderProgramManager.AddProgram("textureText", new ShaderProgram(
				VertexShader.Load("Shaders/Text/VertexShader.glsl"),
				FragmentShader.Load("Shaders/Text/FragmentShader.glsl")));
		}
	}
}
