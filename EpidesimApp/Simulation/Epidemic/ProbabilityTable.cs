using System;
using System.Collections.Generic;

namespace Epidesim.Simulation.Epidemic
{
	class ProbabilityTable<T>
	{
		class Outcome
		{
			public double Probability { get; set; }
			public T Thing { get; set; }
		}
		
		private List<Outcome> possibleOutcomes;
		private Random random;
		private double probabilitySum;

		public ProbabilityTable()
		{
			possibleOutcomes = new List<Outcome>();
			random = new Random();
			probabilitySum = 0;
		}

		public void AddOutcome(T thing, double probability)
		{
			possibleOutcomes.Add(new Outcome()
			{
				Probability = probability,
				Thing = thing
			});

			probabilitySum += probability;
		}

		public T GetRandomOutcome()
		{
			if (possibleOutcomes.Count == 0)
			{
				throw new Exception("No outcomes");
			}

			double prediction = random.NextDouble() * probabilitySum;
			double probability = 0;

			foreach (Outcome outcome in possibleOutcomes)
			{
				probability += outcome.Probability;

				if (probability >= prediction)
				{
					return outcome.Thing;
				}
			}

			return possibleOutcomes[0].Thing;
		}
	}
}
