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

		public EpidemicSimulationRenderer()
		{
			this.creatureRenderer = new PrimitiveRenderer(100000, 100000, 100000);
		}

		public void Render(EpidemicSimulation simulation)
		{
			creatureRenderer.Reset();

			var transformMatrix = simulation.CoordinateSystem.GetTransformationMatrix();
			creatureRenderer.TransformMatrix = transformMatrix;

			foreach (var creature in simulation.HealthyCreatures)
			{
				creatureRenderer.AddRectangle(Rectangle.FromCenterAndSize(creature.Position, new Vector2(1)), Color4.White);
			}

			foreach (var creature in simulation.IllCreatures)
			{
				creatureRenderer.AddRectangle(Rectangle.FromCenterAndSize(creature.Position, new Vector2(1)), Color4.Red);
			}

			creatureRenderer.AddRectangle(Rectangle.FromCenterAndSize(new Vector2(0), new Vector2(1)), Color4.Yellow);
			creatureRenderer.AddRectangle(Rectangle.FromCenterAndSize(new Vector2(simulation.WorldSize, 0), new Vector2(1)), Color4.Yellow);
			creatureRenderer.AddRectangle(Rectangle.FromCenterAndSize(new Vector2(0, simulation.WorldSize), new Vector2(1)), Color4.Yellow);
			creatureRenderer.AddRectangle(Rectangle.FromCenterAndSize(new Vector2(simulation.WorldSize), new Vector2(1)), Color4.Yellow);

			creatureRenderer.AddRectangle(Rectangle.FromCenterAndSize(simulation.WorldMousePosition, new Vector2(1)), Color4.Lime);

			creatureRenderer.DrawFilledElements();

			creatureRenderer.Reset();
			creatureRenderer.AddRectangle(Rectangle.FromTwoPoints(new Vector2(0), new Vector2(simulation.WorldSize)), Color4.Yellow);
			creatureRenderer.DrawHollowElements();
		}
	}
}
