using Epidesim.Simulation.Epidemic.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Simulation.Epidemic.Sectors
{
	class SocialSectorInfo : SectorInfo
	{
		public SocialSectorInfo()
		{
			Name = "Social";

			SquareMetersPerCreature = 12f;

			IdleTimeDistribution = new GaussianDistribution()
			{
				Mean = 60,
				Deviation = 20,
				Min = 10
			};

			PositionDistribution = new GaussianDistribution()
			{
				Mean = 0,
				Deviation = 0.4,
				Min = -1,
				Max = 1
			};

			PreferenceHealthyCreatures = 1f;
			PreferenceIllCreatures = 0.15f;
			PreferenceImmuneCreatures = 1f;

			RecoveryMultiplier = 1f;
			DeathRateMultiplier = 1f;
			SpreadMultiplier = 1.25f;

			CanBeQuarantined = true;
		}
	}
}
