using Epidesim.Engine.Drawing.Types;
using System.Collections;
using System.Collections.Generic;

namespace Epidesim.Simulation.Epidemic
{
	class Sector : IEnumerable<Creature>
	{
		public string Name { get; set; }
		public int MaxCreatures { get; set; }
		public Rectangle Bounds { get; set; }

		public readonly LinkedList<Creature> Creatures;

		public Sector()
		{
			Creatures = new LinkedList<Creature>();
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
	}
}
