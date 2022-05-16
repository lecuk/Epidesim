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
		private readonly LinkedList<Creature> contagiousCreatures;
		private readonly LinkedList<Creature> vulnerableCreatures;

		public CreatureCollection()
		{ 
			allCreatures = new LinkedList<Creature>();
			illCreatures = new LinkedList<Creature>();
			contagiousCreatures = new LinkedList<Creature>();
			vulnerableCreatures = new LinkedList<Creature>();
		}
		
		public IReadOnlyCollection<Creature> Ill => illCreatures;
		public IReadOnlyCollection<Creature> Contagious => contagiousCreatures;
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
			contagiousCreatures.Remove(creature);
			vulnerableCreatures.Remove(creature);
		}

		public void UpdateCreatureSubCollections(Creature creature)
		{
			UpdateCreatureIsAlive(creature);

			if (!creature.IsDead)
			{
				UpdateCreatureIsHealthy(creature);
				UpdateCreatureIsImmune(creature);
			}
		}

		private void UpdateCreatureIsAlive(Creature creature)
		{
			if (creature.IsDead)
			{
				illCreatures.Remove(creature);
				contagiousCreatures.Remove(creature);
				vulnerableCreatures.Remove(creature);
			}
		}

		private void UpdateCreatureIsHealthy(Creature creature)
		{
			illCreatures.Remove(creature);
			contagiousCreatures.Remove(creature);

			if (creature.IsIll)
			{
				illCreatures.AddLast(creature);
			}

			if (creature.IsContagious)
			{
				contagiousCreatures.AddLast(creature);
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
