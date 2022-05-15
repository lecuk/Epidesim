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

		public override GaussianDistribution MaxCreatures => new GaussianDistribution()
		{
			Mean = 40,
			Deviation = 10,
			Min = 0
		};

		public override GaussianDistribution TimeSpent => new GaussianDistribution()
		{
			Mean = 60,
			Deviation = 30,
			Min = 20
		};
	}
}
