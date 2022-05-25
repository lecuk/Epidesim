using System;
using System.Collections.Generic;

namespace Epidesim.Simulation.Epidemic
{
	class NormalizedProbabilityTable<T>
	{
		class Outcome
		{
			public double Probability { get; set; }
			public T Thing { get; set; }
		}

		private T defaultOutcome;
		private List<Outcome> possibleOutcomes;
		private Random random;
		double probabilityOfDefaultOutcome;

		public NormalizedProbabilityTable(T defaultOutcome = default(T))
		{
			this.defaultOutcome = defaultOutcome;

			possibleOutcomes = new List<Outcome>();
			random = new Random();
			probabilityOfDefaultOutcome = 1.0;
		}

		public void AddOutcome(T thing, double probability)
		{
			if (probability > probabilityOfDefaultOutcome)
			{
				throw new Exception("Total probability is more than 1.0");
			}

			probabilityOfDefaultOutcome -= probability;

			possibleOutcomes.Add(new Outcome()
			{
				Probability = probability,
				Thing = thing
			});
		}

		public T GetRandomOutcome()
		{
			double prediction = random.NextDouble();
			double probability = 0;

			foreach (Outcome outcome in possibleOutcomes)
			{
				probability += outcome.Probability;

				if (probability >= prediction)
				{
					return outcome.Thing;
				}
			}

			return defaultOutcome;
		}
	}
}
