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
		public float SectorSize { get; set; }
		public float RoadWidth { get; set; }

		private Random random;

		public CityBuilder(Random random)
		{
			this.random = random;
		}

		public City Build(int cols, int rows)
		{
			var city = new City(SectorSize, RoadWidth, cols, rows);
			var builder = new SectorBuilder(city);

			SectorType emptySector = new EmptySectorType(random);
			SectorType livingSector = new LivingSectorType(random);
			SectorType hospitalSector = new HospitalSectorType(random);
			SectorType socialSector = new SocialSectorType(random);

			for (int r = 0; r < rows; ++r)
			{
				for (int c = 0; c < cols; ++c)
				{
					SectorType info = null;

					if (random.Next() % 7 == 0)
					{
						info = livingSector;
					}
					else if (random.Next() % 12 == 0)
					{
						info = socialSector;
					}
					else if (random.Next() % 24 == 0)
					{
						info = hospitalSector;
					}
					else
					{
						info = emptySector;
					}

					var sector = builder.Build(c, r, info);
					city.SetSector(c, r, sector);
				}
			}

			for (int r = 0; r < rows; ++r)
			{
				for (int c = 0; c < cols; ++c)
				{
					builder.InitSectorNeighbours(city, city[c, r]);
				}
			}

			city.RecalculateMaxPopulation();

			return city;
		}
	}
}
