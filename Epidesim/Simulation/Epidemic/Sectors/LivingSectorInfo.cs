using Epidesim.Simulation.Epidemic.Distributions;
using System;

namespace Epidesim.Simulation.Epidemic.Sectors
{
	class LivingSectorInfo : SectorInfo
	{
		public LivingSectorInfo(Random random)
		{
			Name = "Living";

			SquareMetersPerCreature = 20f;

			IdleTimeDistribution = new GaussianDistribution(random)
			{
				Mean = 60,
				Deviation = 30,
				Min = 10
			};

			PositionDistribution = new GaussianDistribution(random)
			{
				Min = -1,
				Max = 1,
				Mean = 0,
				Deviation = 0.333
			};

			PreferenceHealthyCreatures = 1f;
			PreferenceIllCreatures = 0.33f;
			PreferenceImmuneCreatures = 0.9f;

			RecoveryMultiplier = 1f;
			DeathRateMultiplier = 0.8f;
			SpreadMultiplier = 1f;

			CanBeQuarantined = true;
			AllowInsideOnQuarantine = true;
			AllowOutsideOnQuarantine = false;
			CanBeSelfQuarantined = true;
		}
	}
}
