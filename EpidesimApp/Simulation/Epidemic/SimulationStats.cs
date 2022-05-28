using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Simulation.Epidemic
{
	class SimulationStats
	{
		public class Period
		{
			public float StartTime { get; set; }
			public float EndTime { get; set; }

			public int NewInfections { get; set; }
			public int NewDeaths { get; set; }
			public int NewRecoveries { get; set; }
			public int NewConfirmedCases { get; set; }
			public int TotalInfections { get; set; }
			public int TotalDeaths { get; set; }
			public int TotalImmune { get; set; }
		}

		private readonly List<Period> periods;
		public IReadOnlyList<Period> Periods => periods;

		public SimulationStats()
		{
			periods = new List<Period>();
		}

		public void AddPeriod(Period period)
		{
			periods.Add(period);
		}

		public void Clear()
		{
			periods.Clear();
		}
	}
}
