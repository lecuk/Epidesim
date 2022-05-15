using Epidesim.Engine.Drawing.Types;
using OpenTK;
using System;

namespace Epidesim.Simulation.Epidemic
{
	class SectorBuilder
	{
		private readonly City city;

		public SectorBuilder(City city)
		{
			this.city = city;
		}

		public Sector Build(int col, int row, SectorInfo info)
		{
			Vector2 bottomLeftOfSector = new Vector2(col, row) * city.SectorSize;

			return new Sector()
			{
				MaxCreatures = (int)Math.Round(info.MaxCreatures.GetRandomValue()),

				Bounds = Rectangle.FromTwoPoints(
					bottomLeftOfSector + new Vector2(city.RoadWidth / 2),
					bottomLeftOfSector - new Vector2(city.RoadWidth / 2) + new Vector2(city.SectorSize)),

				Name = String.Format("Sector {0}{1}{2}", info.Name, col, row)
			};
		}
	}
}
