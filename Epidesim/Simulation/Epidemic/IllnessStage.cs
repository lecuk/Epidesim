namespace Epidesim.Simulation.Epidemic
{
	class IllnessStage
	{
		public GaussianDistribution SpreadDistance { get; set; }
		public GaussianDistribution SpreadProbability { get; set; }
		public GaussianDistribution Duration { get; set; }
		public ProbabilityTable<IllnessStage> NextStages { get; set; }
	}
}
