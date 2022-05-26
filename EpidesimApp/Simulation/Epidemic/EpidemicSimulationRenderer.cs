using Epidesim.Engine;
using Epidesim.Engine.Drawing;
using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Epidesim.Simulation.Epidemic
{
	class EpidemicSimulationRenderer : ISimulationRenderer<EpidemicSimulation>
	{
		private readonly PrimitiveRenderer creatureRenderer;
		private readonly PrimitiveRenderer cityRenderer;
		private readonly PrimitiveRenderer selectionRenderer;
		private readonly PrimitiveRenderer sectorBoundsRenderer;
		private readonly TextRenderer sectorTextRenderer;
		private readonly QuadTextureRenderer haloRenderer;
		private readonly PrimitiveRenderer uiPanelRenderer;
		private readonly TextRenderer uiTextRenderer;

		private IEnumerable<Renderer> AllRenderers => new Renderer[]
		{
			creatureRenderer, cityRenderer, selectionRenderer, sectorBoundsRenderer, sectorTextRenderer, haloRenderer, uiPanelRenderer, uiTextRenderer
		};

		private IEnumerable<Renderer> WorldRenderers => new Renderer[]
		{
			creatureRenderer, cityRenderer, selectionRenderer, sectorBoundsRenderer, sectorTextRenderer, haloRenderer
		};

		private IEnumerable<Renderer> UIRenderers => new Renderer[]
		{
			uiPanelRenderer, uiTextRenderer
		};

		public EpidemicSimulationRenderer()
		{
			this.cityRenderer = new PrimitiveRenderer(100000, 100000, 100000);
			this.creatureRenderer = new PrimitiveRenderer(1000000, 1000000, 1000000);
			this.selectionRenderer = new PrimitiveRenderer(1000000, 1000000, 1000000);
			this.sectorBoundsRenderer = new PrimitiveRenderer(100000, 100000, 100000);
			this.sectorTextRenderer = new TextRenderer(2000);
			this.haloRenderer = new QuadTextureRenderer(30000, ResourceManager.GetProgram("textureDefault"));
			this.uiPanelRenderer = new PrimitiveRenderer(10000, 10000, 10000);
			this.uiTextRenderer = new TextRenderer(200);

			sectorTextRenderer.LoadFont(ResourceManager.GetTextureFont("consolas"));
			uiTextRenderer.LoadFont(ResourceManager.GetTextureFont("consolas"));
			
			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.AlphaTest);
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Multisample);

			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			GL.Hint(HintTarget.MultisampleFilterHintNv, HintMode.Fastest);
		}

		public void Render(EpidemicSimulation simulation)
		{
			var worldTransformMatrix = simulation.CoordinateSystem.GetTransformationMatrix();
			foreach (var renderer in WorldRenderers)
			{
				renderer.Reset();
				renderer.TransformMatrix = worldTransformMatrix;
			}

			var uiTransformMatrix = Matrix4.Identity;
			foreach (var renderer in UIRenderers)
			{
				renderer.Reset();
				renderer.TransformMatrix = uiTransformMatrix;
			}

			var city = simulation.City;
			var cityBounds = city.Bounds;

			cityRenderer.AddRectangle(cityBounds, Color4.DimGray);
			
			for (int r = 0; r < city.Rows; ++r)
			{
				for (int c = 0; c < city.Cols; ++c)
				{
					Sector sector = city[c, r];
					var sectorBounds = sector.Bounds;
					cityRenderer.AddRectangle(sectorBounds, sector.Type.DisplayColor);

					if (sector.Creatures.Contagious.Count > 0)
					{
						if (sector.IsQuarantined)
						{
							sectorBoundsRenderer.AddRectangle(sectorBounds, Color4.Orange);
						}
						else
						{
							sectorBoundsRenderer.AddRectangle(sectorBounds, Color4.Red);
						}
					}
				}
			}

			foreach (var creature in city)
			{
				if (creature.IsDead)
				{
					creatureRenderer.AddRectangle(Rectangle.FromCenterAndSize(creature.Position, new Vector2(1)),
						Color4.Black);
				}
			}

			foreach (var creature in city)
			{
				if (!creature.IsDead)
				{
					creatureRenderer.AddRectangle(Rectangle.FromCenterAndSize(creature.Position, new Vector2(1)),
					creature.IsIll
						? Color4.Red
						: creature.IsImmune
							? Color4.Cyan
							: Color4.White);
				}

				if (creature.IsInfected && !creature.IsQuarantined)
				{
					haloRenderer.AddQuad(Rectangle.FromCenterAndSize(creature.Position, new Vector2(4)), Color4.Red);
				}
			}

			if (simulation.SelectedCreature != null)
			{
				var selectedCreature = simulation.SelectedCreature;
				selectionRenderer.AddRectangle(Rectangle.FromCenterAndSize(selectedCreature.Position, new Vector2(3f)),
					Color4.Cyan);

				var sector = selectedCreature.CurrentSector;
				sectorBoundsRenderer.AddRectangle(sector.Bounds, Color4.Yellow);

				var vector = new Vector2(sector.Bounds.Lft, sector.Bounds.Bot);
				string message = String.Format("{0}/{1}", sector.Creatures.Count, sector.MaxCreatures);
				sectorTextRenderer.AddString(message, 4, vector, Color4.White);

				foreach (var creature in sector)
				{
					if (!creature.IsDead)
					{
						creatureRenderer.AddRectangle(Rectangle.FromCenterAndSize(creature.Position, new Vector2(1.5f)),
							creature.IsIll
								? Color4.Red
								: creature.IsPermanentlyImmune
									? Color4.Cyan
									: Color4.White);
					}
				}

				sectorBoundsRenderer.AddLine(selectedCreature.Position, selectedCreature.TargetPoint, Color4.Lime);
			}

			int population = simulation.City.Count;
			int maxPopulation = simulation.City.MaxPopulation;
			int ill = simulation.City.Count(cr => !cr.IsDead && cr.IsInfected);
			int totalIll = simulation.City.Count(cr => cr.WasIllAtSomePoint);
			int immune = simulation.City.Count(cr => !cr.IsDead && cr.IsImmune);
			int died = simulation.City.Count(cr => cr.IsDead);

			string info = String.Format("Population: {0}/{1}\nCurrent cases: {2}\nAffected population: {3}\nImmune: {4}\nDead: {5}\nTime elapsed: {6}", 
				population, maxPopulation, ill, totalIll, immune, died, simulation.TotalTimeElapsed);

			sectorTextRenderer.AddString(info, 14, new Vector2(0, -20), Color4.Yellow);

			string debug = String.Format("Real: {0}  World: {1}", simulation.MousePosition, simulation.WorldMousePosition);
			uiTextRenderer.AddString(debug, 0.04f, new Vector2(-0.98f, 0.95f), Color4.White);
			GL.ClearColor(Color4.DarkBlue);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			cityRenderer.DrawFilledElements();
			sectorBoundsRenderer.DrawHollowElements();
			haloRenderer.DrawTexture(ResourceManager.GetTexture("halo"));
			creatureRenderer.DrawFilledElements();
			sectorTextRenderer.DrawAll();
			selectionRenderer.DrawHollowElements();
			uiPanelRenderer.DrawFilledElements();
			uiTextRenderer.DrawAll();
		}
	}
}
