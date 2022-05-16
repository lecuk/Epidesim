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

		private SectorInfo emptySector = new EmptySectorInfo();
		private SectorInfo livingSector = new LivingSectorInfo();
		private SectorInfo hospitalSector = new HospitalSectorInfo();
		private SectorInfo socialSector = new SocialSectorInfo();

		public City Build(int cols, int rows)
		{
			var city = new City(SectorSize, RoadWidth, cols, rows);
			var builder = new SectorBuilder(city);
			var random = new Random();

			for (int r = 0; r < rows; ++r)
			{
				for (int c = 0; c < cols; ++c)
				{
					SectorInfo info = null;

					if (random.Next() % 4 == 0)
					{
						info = livingSector;
					}
					else if (random.Next() % 8 == 0)
					{
						info = socialSector;
					}
					else if (random.Next() % 16 == 0)
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

			return city;
		}
	}
}
