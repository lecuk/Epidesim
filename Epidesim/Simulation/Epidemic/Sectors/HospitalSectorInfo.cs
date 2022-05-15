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
		public override string Name => "Hospital";

		public override ValueDistribution SquareMetersPerCreature => new GaussianDistribution()
		{
			Mean = 50,
			Deviation = 10,
			Min = 5
		};

		public override ValueDistribution IdleTime => new GaussianDistribution()
		{
			Mean = 120,
			Deviation = 60,
			Min = 40
		};

		public override ValueDistribution PositionDistribution => new FixedDistribution()
		{
			Min = -1,
			Max = 1
		};

		public override float AllowHealthyCreatures => 0.1f;
		public override float AllowIllCreatures => 0.95f;
		public override float AllowImmuneCreatures => 1f;
	}
}
