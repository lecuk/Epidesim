using OpenTK;
using System;

namespace Epidesim.Simulation.Epidemic.Distributions
{
	class GaussianDistribution : ValueDistribution
	{
		public double Mean { get; set; } = 0;
		public double Deviation { get; set; } = 1;

		public GaussianDistribution(Random random = null)
			: base(random) {  }

		// using Box-Muller formula for Gaussian random distribution
		protected override double ValueFunc()
		{
			double u1 = random.NextDouble();
			double u2 = random.NextDouble();
			double temp1 = Math.Sqrt(-2 * Math.Log(u1));
			double temp2 = 2 * Math.PI * u2;
			return Mean + Deviation * (temp1 * Math.Cos(temp2));
		}
	}
}
