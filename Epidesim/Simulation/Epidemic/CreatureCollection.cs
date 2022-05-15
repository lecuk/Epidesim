using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Simulation.Epidemic
{
	class CreatureCollection : IReadOnlyCollection<Creature>
	{
		private readonly LinkedList<Creature> allCreatures;
		private readonly LinkedList<Creature> illCreatures;
		private readonly LinkedList<Creature> vulnerableCreatures;

		public CreatureCollection()
		{ 
			allCreatures = new LinkedList<Creature>();
			illCreatures = new LinkedList<Creature>();
			vulnerableCreatures = new LinkedList<Creature>();
		}
		
		public IReadOnlyCollection<Creature> Ill => illCreatures;
		public IReadOnlyCollection<Creature> Vulnerable => vulnerableCreatures;

		public int Count => this.allCreatures.Count;

		public void Add(Creature creature)
		{
			allCreatures.AddLast(creature);
			UpdateCreatureSubCollections(creature);
		}

		public void Remove(Creature creature)
		{
			allCreatures.Remove(creature);
			illCreatures.Remove(creature);
			vulnerableCreatures.Remove(creature);
		}

		public void UpdateCreatureSubCollections(Creature creature)
		{
			UpdateCreatureIsHealthy(creature);

			if (!creature.IsIll)
			{
				UpdateCreatureIsImmune(creature);
			}
		}

		private void UpdateCreatureIsHealthy(Creature creature)
		{
			illCreatures.Remove(creature);

			if (creature.IsIll)
			{
				vulnerableCreatures.Remove(creature);
				illCreatures.AddLast(creature);
			}
		}

		private void UpdateCreatureIsImmune(Creature creature)
		{
			vulnerableCreatures.Remove(creature);

			if (!creature.IsIll && !creature.IsImmune)
			{
				vulnerableCreatures.AddLast(creature);
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
