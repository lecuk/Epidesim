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

		public EpidemicSimulationRenderer()
		{
			this.cityRenderer = new PrimitiveRenderer(10000, 10000, 10000);
			this.creatureRenderer = new PrimitiveRenderer(100000, 100000, 100000);
			this.selectionRenderer = new PrimitiveRenderer(100000, 100000, 100000);
			this.sectorBoundsRenderer = new PrimitiveRenderer(10000, 10000, 10000);
		}

		public void Render(EpidemicSimulation simulation)
		{
			cityRenderer.Reset();
			creatureRenderer.Reset();
			selectionRenderer.Reset();
			sectorBoundsRenderer.Reset();

			var transformMatrix = simulation.CoordinateSystem.GetTransformationMatrix();
			cityRenderer.TransformMatrix = transformMatrix;
			creatureRenderer.TransformMatrix = transformMatrix;
			selectionRenderer.TransformMatrix = transformMatrix;
			sectorBoundsRenderer.TransformMatrix = transformMatrix;

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
						: Color4.White);

					if (creature.IsImmune)
					{
						selectionRenderer.AddRectangle(Rectangle.FromCenterAndSize(creature.Position, new Vector2(2f)),
							Color4.GreenYellow);
					}
				}
			}

			if (simulation.SelectedCreature != null)
			{
				var selectedCreature = simulation.SelectedCreature;
				selectionRenderer.AddRectangle(Rectangle.FromCenterAndSize(selectedCreature.Position, new Vector2(3f)),
					Color4.Cyan);

				var sector = selectedCreature.CurrentSector;
				sectorBoundsRenderer.AddRectangle(sector.Bounds, Color4.Yellow);

				foreach (var creature in sector)
				{
					if (!creature.IsDead)
					{
						creatureRenderer.AddRectangle(Rectangle.FromCenterAndSize(creature.Position, new Vector2(1.5f)),
							creature.IsIll
							? Color4.Red
							: Color4.Lime);
					}
				}
			}

			cityRenderer.DrawFilledElements();
			sectorBoundsRenderer.DrawHollowElements();
			creatureRenderer.DrawFilledElements();
			selectionRenderer.DrawHollowElements();
		}
	}
}
