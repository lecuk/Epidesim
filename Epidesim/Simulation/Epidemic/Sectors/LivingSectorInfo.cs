using Epidesim.Simulation.Epidemic.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Simulation.Epidemic.Sectors
{
	class LivingSectorInfo : SectorInfo
	{
		public LivingSectorInfo()
		{
			Name = "Living";

			SquareMetersPerCreature = 20f;

			IdleTimeDistribution = new GaussianDistribution()
			{
				Mean = 60,
				Deviation = 30,
				Min = 10
			};

			PositionDistribution = new GaussianDistribution()
			{
				Min = -1,
				Max = 1,
				Mean = 0,
				Deviation = 0.333
			};

			PreferenceHealthyCreatures = 1f;
			PreferenceIllCreatures = 0.5f;
			PreferenceImmuneCreatures = 0.9f;

			RecoveryMultiplier = 1f;
			DeathRateMultiplier = 0.8f;
			SpreadMultiplier = 1f;

			CanBeQuarantined = true;
		}
	}
}
