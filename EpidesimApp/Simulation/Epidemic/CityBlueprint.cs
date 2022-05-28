using Epidesim.Simulation.Epidemic;
using Epidesim.Simulation.Epidemic.Sectors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Simulation.Epidemic
{
	class CityBlueprint
	{
		public float SectorSize { get; set; }
		public float RoadWidth { get; set; }
		public int SectorColumns { get; set; }
		public int SectorRows { get; set; }
		public int Population { get; set; }

		public ObservableCollection<SectorType> SectorTypes { get; set; }

		public static CityBlueprint Default(Random random) => new CityBlueprint()
		{
			SectorSize = 40,
			RoadWidth = 5,
			SectorColumns = 30,
			SectorRows = 20,
			Population = 5000,
			SectorTypes = new ObservableCollection<SectorType>()
			{
				new EmptySectorType(random),
				new LivingSectorType(random),
				new SocialSectorType(random),
				new HospitalSectorType(random)
			}
		};
	}
}
