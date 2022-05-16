using Epidesim.Engine;
using Epidesim.Engine.Drawing;
using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics;
using System;
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

		public EpidemicSimulationRenderer()
		{
			this.cityRenderer = new PrimitiveRenderer(100000, 100000, 100000);
			this.creatureRenderer = new PrimitiveRenderer(1000000, 1000000, 1000000);
			this.selectionRenderer = new PrimitiveRenderer(1000000, 1000000, 1000000);
			this.sectorBoundsRenderer = new PrimitiveRenderer(100000, 100000, 100000);
			this.sectorTextRenderer = new TextRenderer(2000);

			sectorTextRenderer.LoadFont(ResourceManager.GetTextureFont("consolas"));
		}

		public void Render(EpidemicSimulation simulation)
		{
			cityRenderer.Reset();
			creatureRenderer.Reset();
			selectionRenderer.Reset();
			sectorBoundsRenderer.Reset();
			sectorTextRenderer.Reset();

			var transformMatrix = simulation.CoordinateSystem.GetTransformationMatrix();
			cityRenderer.TransformMatrix = transformMatrix;
			creatureRenderer.TransformMatrix = transformMatrix;
			selectionRenderer.TransformMatrix = transformMatrix;
			sectorBoundsRenderer.TransformMatrix = transformMatrix;
			sectorTextRenderer.TransformMatrix = transformMatrix;

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

					if (sector.Creatures.Ill.Count > 0)
					{
						sectorBoundsRenderer.AddRectangle(sectorBounds, Color4.Red);
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
								: creature.IsImmune
									? Color4.Cyan
									: Color4.White);
					}
				}

				sectorBoundsRenderer.AddLine(selectedCreature.Position, selectedCreature.TargetPoint, Color4.Lime);
			}

			int population = simulation.City.Count;
			int ill = simulation.City.Count(cr => !cr.IsDead && cr.IsIll);
			int immune = simulation.City.Count(cr => !cr.IsDead && cr.IsImmune);
			int died = simulation.City.Count(cr => cr.IsDead);

			string info = String.Format("Population: {0}\nIll: {1}\nImmune: {2}\nDead: {3}\nTime elapsed: {4}", 
				population, ill, immune, died, simulation.TotalTimeElapsed);

			sectorTextRenderer.AddString(info, 14, new Vector2(0, -20), Color4.Yellow);

			cityRenderer.DrawFilledElements();
			sectorBoundsRenderer.DrawHollowElements();
			creatureRenderer.DrawFilledElements();
			sectorTextRenderer.DrawAll();
			selectionRenderer.DrawHollowElements();
		}
	}
}
