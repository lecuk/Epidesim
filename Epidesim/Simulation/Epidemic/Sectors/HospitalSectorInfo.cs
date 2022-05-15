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

			SquareMetersPerCreature = 30f;

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
			PreferenceIllCreatures = 0.95f;
			PreferenceImmuneCreatures = 0.8f;
		}
	}
}
