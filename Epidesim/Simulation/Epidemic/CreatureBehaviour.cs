namespace Epidesim.Simulation.Epidemic
{
	class CreatureBehaviour
	{
		public float PreferenceToStayInSameSectorMultiplier { get; set; }
		public float PreferenceToStayInSameSectorWhenIllMultiplier { get; set; }
		public float QuarantineSpreadMultiplier { get; set; }

		public int QuarantineThreshold { get; set; }
		public int QuarantineCancelThreshold { get; set; }

		public ValueDistribution SelfQuarantineDelayDistribution { get; set; }
		public ValueDistribution SelfQuarantineCooldownDistribution { get; set; }
		public float SelfQuarantineSpreadMultiplier { get; set; }
	}
}
