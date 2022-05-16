using Epidesim.Engine.Drawing.Types;
using OpenTK;
using System;
using System.Collections.Generic;

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

				MaxCreatures = (int)Math.Round(city.SectorSize * city.SectorSize / info.SquareMetersPerCreature),

				PreferenceHealthy = info.PreferenceHealthyCreatures,
				PreferenceIll = info.PreferenceIllCreatures,
				PreferenceImmune = info.PreferenceImmuneCreatures,

				RecoveryMultiplier = info.RecoveryMultiplier,
				DeathRateMultiplier = info.DeathRateMultiplier,
				SpreadMultiplier = info.SpreadMultiplier,

				CanBeQuarantined = info.CanBeQuarantined,
				CanBeSelfQuarantined = info.CanBeSelfQuarantined,
				AllowInsideOnQuarantine = info.AllowInsideOnQuarantine,
				AllowOutsideOnQuarantine = info.AllowOutsideOnQuarantine,

				IsQuarantined = false,

				Bounds = Rectangle.FromTwoPoints(
					bottomLeftOfSector + new Vector2(city.RoadWidth / 2),
					bottomLeftOfSector - new Vector2(city.RoadWidth / 2) + new Vector2(city.SectorSize)),

				IdleTimeDistribution = info.IdleTimeDistribution,
				PositionDistribution = info.PositionDistribution
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

			var neighbours = new List<Sector>();

			if (fromCol != col) neighbours.Add(city[fromCol, row]);
			if (toCol != col) neighbours.Add(city[toCol, row]);
			if (fromRow != col) neighbours.Add(city[col, fromRow]);
			if (toRow != col) neighbours.Add(city[col, toRow]);

			sector.NeighbourSectors = neighbours;
		}
	}
}
