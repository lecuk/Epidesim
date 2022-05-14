namespace Epidesim.Simulation.Epidemic
{
	class Illness
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public IllnessStage InitialStage { get; set; }
	}
}
