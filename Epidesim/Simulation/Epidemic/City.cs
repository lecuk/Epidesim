using Epidesim.Engine.Drawing.Types;
using Epidesim.Simulation.Epidemic.Sectors;
using OpenTK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Simulation.Epidemic
{
	class City : IEnumerable<Creature>
	{
		public readonly float SectorSize;
		public readonly float RoadWidth;
		public readonly int Cols, Rows;

		private readonly Sector[,] sectors;
		private readonly List<Creature> allCreatures;

		public Rectangle Bounds => Rectangle.FromTwoPoints(Vector2.Zero, new Vector2(Cols, Rows) * SectorSize);

		public City(float sectorSize, float roadWidth, int sectorCols, int sectorRows)
		{
			this.SectorSize = sectorSize;
			this.RoadWidth = roadWidth;
			this.Cols = sectorCols;
			this.Rows = sectorRows;
			this.sectors = new Sector[sectorRows, sectorCols];
			this.allCreatures = new List<Creature>();
		}

		public void SetSector(int col, int row, Sector sector)
		{
			this.sectors[row, col] = sector;
		}

		public Sector GetSector(int col, int row)
		{
			return this.sectors[row, col];
		}

		public Sector this[int col, int row] => GetSector(col, row);

		public void CreateCreature(Creature creature)
		{
			allCreatures.Add(creature);

			var sector = creature.CurrentSector;
			sector.Creatures.Add(creature);
		}
		
		public Sector GetSectorAtLocation(Vector2 location)
		{
			int col = (int)Math.Floor(location.X / SectorSize);
			int row = (int)Math.Floor(location.Y / SectorSize);

			if (col >= Cols) col = Cols - 1;
			if (col < 0) col = 0;

			if (row >= Rows) row = Rows - 1;
			if (row < 0) row = 0;

			return GetSector(col, row);
		}

		public void UpdateCreatureSectorFromPosition(Creature creature)
		{
			Vector2 position = creature.Position;
			var sector = GetSectorAtLocation(position);
			SetCreatureSector(creature, sector);
		}

		public void UpdateCreature(Creature creature)
		{
			creature.CurrentSector.Creatures.UpdateCreatureSubCollections(creature);
		}

		private void SetCreatureSector(Creature creature, Sector newSector)
		{
			var oldSector = creature.CurrentSector;
			if (oldSector != newSector)
			{
				if (oldSector != null)
				{
					oldSector.Creatures.Remove(creature);
				}

				creature.CurrentSector = newSector;
				newSector.Creatures.Add(creature);
				UpdateCreature(creature);
			}
		}

		public IEnumerator<Creature> GetEnumerator()
		{
			return this.allCreatures.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.allCreatures.GetEnumerator();
		}
	}
}
