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
	}
}
