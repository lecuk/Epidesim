using OpenTK;
using System;

namespace Epidesim.Simulation.Epidemic
{
	class GaussianDistribution
	{
		public double Mean { get; set; } = 0;
		public double Deviation { get; set; } = 1;

		public double Min { get; set; } = Double.NegativeInfinity;
		public double Max { get; set; } = Double.PositiveInfinity;

		private readonly Random random;

		public GaussianDistribution(Random random = null)
		{
			this.random = random ?? new Random();
		}

		// using Box-Muller formula for Gaussian random distribution
		public double GetRandomValue()
		{
			double u1 = random.NextDouble();
			double u2 = random.NextDouble();
			double temp1 = Math.Sqrt(-2 * Math.Log(u1));
			double temp2 = 2 * Math.PI * u2;
			double value = Mean + Deviation * (temp1 * Math.Cos(temp2));

			return MathHelper.Clamp(value, Min, Max);
		}
	}
}
