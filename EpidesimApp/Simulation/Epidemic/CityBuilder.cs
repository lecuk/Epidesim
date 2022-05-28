using Epidesim.Simulation.Epidemic.Sectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Simulation.Epidemic
{
	class CityBuilder
	{
		private Random random;

		public CityBuilder(Random random)
		{
			this.random = random;
		}

		public City Build(CityBlueprint blueprint)
		{
			var city = new City(blueprint.SectorSize, blueprint.RoadWidth, blueprint.SectorColumns, blueprint.SectorRows);
			var builder = new SectorBuilder(city);
			var possibleTypes = new ProbabilityTable<SectorType>(random);

			foreach (var sectorType in blueprint.SectorTypes)
			{
				possibleTypes.AddOutcome(sectorType, sectorType.ProbabilityToAppear);
			}

			for (int r = 0; r < blueprint.SectorRows; ++r)
			{
				for (int c = 0; c < blueprint.SectorColumns; ++c)
				{
					SectorType sectorType = possibleTypes.GetRandomOutcome();
					var sector = builder.Build(c, r, sectorType);
					city.SetSector(c, r, sector);
				}
			}

			for (int r = 0; r < blueprint.SectorRows; ++r)
			{
				for (int c = 0; c < blueprint.SectorColumns; ++c)
				{
					builder.InitSectorNeighbours(city, city[c, r]);
				}
			}

			city.RecalculateMaxPopulation();

			return city;
		}
	}
}
