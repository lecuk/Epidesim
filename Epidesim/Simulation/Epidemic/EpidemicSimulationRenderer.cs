using Epidesim.Engine;
using Epidesim.Engine.Drawing;
using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics;
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

		private IEnumerable<Renderer> AllRenderers => new Renderer[]
		{
			creatureRenderer, cityRenderer, selectionRenderer, sectorBoundsRenderer, sectorTextRenderer, haloRenderer
		};

		public EpidemicSimulationRenderer()
		{
			this.cityRenderer = new PrimitiveRenderer(100000, 100000, 100000);
			this.creatureRenderer = new PrimitiveRenderer(1000000, 1000000, 1000000);
			this.selectionRenderer = new PrimitiveRenderer(1000000, 1000000, 1000000);
			this.sectorBoundsRenderer = new PrimitiveRenderer(100000, 100000, 100000);
			this.sectorTextRenderer = new TextRenderer(2000);
			this.haloRenderer = new QuadTextureRenderer(30000, ResourceManager.GetProgram("textureDefault"));

			sectorTextRenderer.LoadFont(ResourceManager.GetTextureFont("consolas"));
		}

		public void Render(EpidemicSimulation simulation)
		{
			var transformMatrix = simulation.CoordinateSystem.GetTransformationMatrix();
			foreach (var renderer in AllRenderers)
			{
				renderer.Reset();
				renderer.TransformMatrix = transformMatrix;
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
					if (sector.Name.StartsWith("Sector Living"))
					{
						cityRenderer.AddRectangle(sectorBounds, Color4.DarkGreen);
					}
					else if (sector.Name.StartsWith("Sector Hospital"))
					{
						cityRenderer.AddRectangle(sectorBounds, Color4.DarkSlateGray);
					}
					else if (sector.Name.StartsWith("Sector Social"))
					{
						cityRenderer.AddRectangle(sectorBounds, Color4.Olive);
					}

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

				if (creature.IsContagious)
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
			int ill = simulation.City.Count(cr => !cr.IsDead && cr.IsContagious);
			int totalIll = simulation.City.Count(cr => cr.WasIllAtSomePoint);
			int immune = simulation.City.Count(cr => !cr.IsDead && cr.IsImmune);
			int died = simulation.City.Count(cr => cr.IsDead);

			string info = String.Format("Population: {0}\nCurrent cases: {1}\nAffected population: {2}\nImmune: {3}\nDead: {4}\nTime elapsed: {5}", 
				population, ill, totalIll, immune, died, simulation.TotalTimeElapsed);

			sectorTextRenderer.AddString(info, 14, new Vector2(0, -20), Color4.Yellow);

			cityRenderer.DrawFilledElements();
			sectorBoundsRenderer.DrawHollowElements();
			haloRenderer.DrawTexture(ResourceManager.GetTexture("halo"));
			creatureRenderer.DrawFilledElements();
			sectorTextRenderer.DrawAll();
			selectionRenderer.DrawHollowElements();
		}
	}
}
