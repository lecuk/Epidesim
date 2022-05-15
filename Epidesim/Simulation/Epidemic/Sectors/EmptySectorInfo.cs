namespace Epidesim.Simulation.Epidemic.Sectors
{
	class EmptySectorInfo : SectorInfo
	{
		public override string Name => "Empty";

		public override GaussianDistribution MaxCreatures => new GaussianDistribution()
		{
			Mean = 100,
			Deviation = 40,
			Min = 0
		};

		public override GaussianDistribution TimeSpent => new GaussianDistribution()
		{
			Mean = 20,
			Deviation = 20,
			Min = 5
		};
	}
}
