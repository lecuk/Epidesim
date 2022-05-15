using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Simulation.Epidemic
{
	abstract class SectorInfo
	{
		public abstract string Name { get; }
		public abstract GaussianDistribution MaxCreatures { get; }
		public abstract GaussianDistribution IdleTime { get; }
	}
}
