using Epidesim.Engine.Drawing;
using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics;
using System;

namespace Epidesim.Simulation.Epidemic
{
	class EpidemicSimulationRenderer : ISimulationRenderer<EpidemicSimulation>
	{
		private readonly PrimitiveRenderer creatureRenderer;
		private readonly PrimitiveRenderer cityRenderer;
		private readonly PrimitiveRenderer selectionRenderer;

		public EpidemicSimulationRenderer()
		{
			this.cityRenderer = new PrimitiveRenderer(100000, 100000, 100000);
			this.creatureRenderer = new PrimitiveRenderer(100000, 100000, 100000);
			this.selectionRenderer = new PrimitiveRenderer(10, 10, 10);
		}

		public void Render(EpidemicSimulation simulation)
		{
			cityRenderer.Reset();
			creatureRenderer.Reset();
			selectionRenderer.Reset();

			var transformMatrix = simulation.CoordinateSystem.GetTransformationMatrix();
			cityRenderer.TransformMatrix = transformMatrix;
			creatureRenderer.TransformMatrix = transformMatrix;
			selectionRenderer.TransformMatrix = transformMatrix;

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
				}
			}

			foreach (var creature in city)
			{
				creatureRenderer.AddRectangle(Rectangle.FromCenterAndSize(creature.Position, new Vector2(1)),
					creature.IsIll
					? Color4.Red
					: Color4.White);
			}

			if (simulation.SelectedCreature != null)
			{
				selectionRenderer.AddRectangle(Rectangle.FromCenterAndSize(simulation.SelectedCreature.Position, new Vector2(2.5f)),
					simulation.SelectedCreature.IsIll
					? Color4.Orange
					: Color4.Yellow);
			}

			cityRenderer.DrawFilledElements();
			creatureRenderer.DrawFilledElements();
			selectionRenderer.DrawHollowElements();
		}
	}
}
