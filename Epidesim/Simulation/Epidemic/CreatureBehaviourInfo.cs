using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Simulation.Epidemic
{
	class CreatureBehaviourInfo
	{
		public float PreferenceToStayInSameSectorMultiplier { get; set; }
		public float PreferenceToStayInSameSectorWhenIllMultiplier { get; set; }
		public float QuarantineSpreadMultiplier { get; set; }

		public int QuarantineThreshold { get; set; }
		public int QuarantineCancelThreshold { get; set; }
	}
}
