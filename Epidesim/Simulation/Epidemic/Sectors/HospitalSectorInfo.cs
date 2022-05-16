using Epidesim.Simulation.Epidemic.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Simulation.Epidemic.Sectors
{
	class HospitalSectorInfo : SectorInfo
	{
		public HospitalSectorInfo()
		{
			Name = "Hospital";

			SquareMetersPerCreature = 16f;

			IdleTimeDistribution = new GaussianDistribution()
			{
				Mean = 180,
				Deviation = 60,
				Min = 40
			};

			PositionDistribution = new FixedDistribution()
			{
				Min = -1,
				Max = 1
			};

			PreferenceHealthyCreatures = 0.05f;
			PreferenceIllCreatures = 2.5f;
			PreferenceImmuneCreatures = 0.75f;

			RecoveryMultiplier = 2f;
			DeathRateMultiplier = 0.2f;
			SpreadMultiplier = 0.5f;

			CanBeQuarantined = false;
		}
	}
}
