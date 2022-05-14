using System;

namespace Epidesim.Simulation.Epidemic
{
	class GaussianDistribution
	{
		public double Mean { get; set; }
		public double Deviation { get; set; }

		private readonly Random random;

		public GaussianDistribution(double mean = 0, double deviation = 1, Random random = null)
		{
			Mean = mean;
			Deviation = deviation;

			this.random = random ?? new Random();
		}

		// Box-Muller formula for Gaussian random distribution
		public double GetRandomValue()
		{
			double u1 = random.NextDouble();
			double u2 = random.NextDouble();
			double temp1 = Math.Sqrt(-2 * Math.Log(u1));
			double temp2 = 2 * Math.PI * u2;

			return Mean + Deviation * (temp1 * Math.Cos(temp2));
		}
	}
}
