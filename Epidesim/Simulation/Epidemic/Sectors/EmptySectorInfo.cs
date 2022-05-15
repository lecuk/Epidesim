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

			PreferenceHealthyCreatures = 0.05f;
			PreferenceIllCreatures = 0.95f;
			PreferenceImmuneCreatures = 0.8f;
		}
	}
}
