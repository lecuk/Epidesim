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
		public override string Name => "Living";

		public override ValueDistribution SquareMetersPerCreature => new GaussianDistribution()
		{
			Mean = 12,
			Deviation = 2,
			Min = 2
		};

		public override ValueDistribution IdleTime => new GaussianDistribution()
		{
			Mean = 60,
			Deviation = 30,
			Min = 20
		};
		
		public override ValueDistribution PositionDistribution => new GaussianDistribution()
		{
			Min = -1,
			Max = 1,
			Mean = 0,
			Deviation = 0.333
		};

		public override float AllowHealthyCreatures => 1f;
		public override float AllowIllCreatures => 0.033f;
		public override float AllowImmuneCreatures => 0.75f;
	}
}
