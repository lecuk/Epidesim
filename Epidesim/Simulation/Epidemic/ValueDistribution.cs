using OpenTK;
using System;

namespace Epidesim.Simulation.Epidemic
{
	abstract class ValueDistribution
	{
		protected Random random;

		public double Min { get; set; } = Double.NegativeInfinity;
		public double Max { get; set; } = Double.PositiveInfinity;

		public ValueDistribution(Random random = null)
		{
			this.random = random ?? new Random();
		}

		public double GetRandomValue()
		{
			double value = ValueFunc();
			return MathHelper.Clamp(value, Min, Max);
		}

		public double GetProbabilityBetweenValues(double from, double to)
		{
			return Math.Abs(CumulativeFunc(to) - CumulativeFunc(from));
		}

		protected abstract double ValueFunc();
		protected abstract double CumulativeFunc(double x);
	}
}
