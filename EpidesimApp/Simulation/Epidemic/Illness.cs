using Epidesim.Simulation.Epidemic.Distributions;
using System;

namespace Epidesim.Simulation.Epidemic
{
	class Illness
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public ValueDistribution IncubationPeriodDuration { get; set; }
		public ValueDistribution IllnessPeriodDuration { get; set; }
		public ValueDistribution TemporaryImmunityDuration { get; set; }

		public float IncubationPeriodSpread { get; set; }
		public float IllnessPeriodSpread { get; set; }

		public float UnsymptomaticRate { get; set; }
		public float FatalityRate { get; set; }
		public float ImmunityRate { get; set; }

		public static Illness Default(Random random) => new Illness()
		{
			Name = "Test illness",
			Description = "description",
			IncubationPeriodSpread = 0.002f,
			IllnessPeriodSpread = 0.005f,
			FatalityRate = 0.00033f,
			ImmunityRate = 0.5f,
			UnsymptomaticRate = 0.05f,
			IncubationPeriodDuration = new GaussianDistribution(random)
			{
				Mean = 60,
				Deviation = 20,
				Min = 15
			},
			IllnessPeriodDuration = new GaussianDistribution(random)
			{
				Mean = 180,
				Deviation = 80,
				Min = 60
			},
			TemporaryImmunityDuration = new GaussianDistribution(random)
			{
				Mean = 400,
				Deviation = 100,
				Min = 120
			},
		};
	}
}
