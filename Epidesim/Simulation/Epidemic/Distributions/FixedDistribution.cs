using System;

namespace Epidesim.Simulation.Epidemic.Distributions
{
	class FixedDistribution : ValueDistribution
	{
		public FixedDistribution(Random random = null) 
			: base(random) {  }

		protected override double ValueFunc()
		{
			return Min + (Max - Min) * random.NextDouble();
		}

		protected override double CumulativeFunc(double x)
		{
			return Min + x * (Max - Min);
		}
	}
}
