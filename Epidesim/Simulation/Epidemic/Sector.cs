using Epidesim.Engine.Drawing.Types;
using OpenTK;
using System.Collections;
using System.Collections.Generic;

namespace Epidesim.Simulation.Epidemic
{
	class Sector : IEnumerable<Creature>
	{
		public string Name { get; set; }
		public int Col { get; set; }
		public int Row { get; set; }
		public int MaxCreatures { get; set; }
		public Rectangle Bounds { get; set; }
		public ValueDistribution IdleTime { get; set; }
		public ValueDistribution PositionDistribution { get; set; }
		public IReadOnlyList<Sector> NeighbourSectors { get; set; }

		public readonly CreatureCollection Creatures;

		public Sector()
		{
			Creatures = new CreatureCollection();
		}

		public IEnumerator<Creature> GetEnumerator()
		{
			return Creatures.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Creatures.GetEnumerator();
		}

		public bool IsFull()
		{
			return Creatures.Count >= MaxCreatures;
		}
		
		public Vector2 GetRandomPoint()
		{
			float a = (float)PositionDistribution.GetRandomValue();
			float b = (float)PositionDistribution.GetRandomValue();

			return Bounds.Center + new Vector2(a, b) * new Vector2(Bounds.Width, Bounds.Height) / 2;
		}
	}
}
