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
				Name = String.Format("Sector {0}{1}{2}", info.Name, col, row),

				Col = col,
				Row = row,

				MaxCreatures = (int)Math.Round(info.MaxCreatures.GetRandomValue()),

				Bounds = Rectangle.FromTwoPoints(
					bottomLeftOfSector + new Vector2(city.RoadWidth / 2),
					bottomLeftOfSector - new Vector2(city.RoadWidth / 2) + new Vector2(city.SectorSize)),

				IdleTime = info.IdleTime,

				PositionDistribution = new GaussianDistribution()
				{
					Mean = 0,
					Deviation = city.SectorSize / 5,
					Min = city.RoadWidth / 2 - city.SectorSize / 2,
					Max = city.SectorSize / 2 - city.RoadWidth / 2
				}
			};
		}

		public void InitSectorNeighbours(City city, Sector sector)
		{
			int col = sector.Col;
			int row = sector.Row;

			int fromCol = (col > 0) ? (col - 1) : col;
			int toCol = (col < city.Cols - 1) ? (col + 1) : col;
			int fromRow = (row > 0) ? (row - 1) : row;
			int toRow = (row < city.Rows - 1) ? (row + 1) : row;
			int width = toCol - fromCol + 1;
			int height = toRow - fromRow + 1;

			Sector[] neighbours = new Sector[width * height - 1];

			int i = 0;
			for (int r = fromRow; r <= toRow; ++r)
			{
				for (int c = fromCol; c <= toCol; ++c)
				{
					if (r == row && c == col) continue;

					neighbours[i++] = city[c, r];
				}
			}

			sector.NeighbourSectors = neighbours;
		}
	}
}
