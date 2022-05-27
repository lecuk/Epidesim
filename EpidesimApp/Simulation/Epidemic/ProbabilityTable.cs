using System;
using System.Collections.Generic;
using System.Linq;

namespace Epidesim.Simulation.Epidemic
{
	class ProbabilityTable<T> 
	{
		private Dictionary<T, double> possibleOutcomes;
		private Random random;
		private double probabilitySum;

		public ProbabilityTable(Random random = null)
		{
			this.possibleOutcomes = new Dictionary<T, double>();
			this.random = random ?? new Random();
			this.probabilitySum = 0;
		}

		public void AddOutcome(T thing, double probability)
		{
			possibleOutcomes.Add(thing, probability);
			probabilitySum += probability;
		}

		public void RemoveOutcome(T thing)
		{
			possibleOutcomes.Remove(thing);
		}

		public IReadOnlyCollection<T> AllOutcomes => possibleOutcomes.Keys;
		public double GetOutcomeProbability(T thing) => possibleOutcomes[thing];
		public double GetNormalizedOutcomeProbability(T thing) => GetOutcomeProbability(thing) / probabilitySum;
		public double this[T thing] => GetOutcomeProbability(thing);

		public T GetRandomOutcome()
		{
			if (possibleOutcomes.Count == 0)
			{
				throw new Exception("No outcomes");
			}

			double prediction = random.NextDouble() * probabilitySum;
			double probability = 0;
			var lastThing = default(T);

			foreach (var outcome in possibleOutcomes)
			{
				lastThing = outcome.Key;
				probability += outcome.Value;

				if (probability >= prediction)
				{
					return lastThing;
				}
			}

			return lastThing;
		}
	}
}
