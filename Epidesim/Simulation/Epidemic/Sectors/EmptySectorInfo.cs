using Epidesim.Simulation.Epidemic.Distributions;

namespace Epidesim.Simulation.Epidemic.Sectors
{
	class EmptySectorInfo : SectorInfo
	{
		public EmptySectorInfo()
		{
			Name = "Empty";

			SquareMetersPerCreature = 40f;

			IdleTimeDistribution = new GaussianDistribution()
			{
				Mean = 20,
				Deviation = 10,
				Min = 2
			};

			PositionDistribution = new FixedDistribution()
			{
				Min = -1,
				Max = 1
			};

			PreferenceHealthyCreatures = 1f;
			PreferenceIllCreatures = 0.1f;
			PreferenceImmuneCreatures = 1f;

			RecoveryMultiplier = 0.5f;
			DeathRateMultiplier = 1f;
			SpreadMultiplier = 1f;
		}
	}
}
