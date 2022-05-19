using Epidesim.Simulation.Epidemic.Distributions;
using OpenTK.Graphics;
using System;

namespace Epidesim.Simulation.Epidemic.Sectors
{
	class HospitalSectorType : SectorType
	{
		public HospitalSectorType(Random random)
		{
			Name = "Hospital";

			SquareMetersPerCreature = 16f;

			IdleTimeDistribution = new GaussianDistribution(random)
			{
				Mean = 180,
				Deviation = 60,
				Min = 40
			};

			PositionDistribution = new FixedDistribution(random)
			{
				Min = -1,
				Max = 1
			};

			PreferenceHealthyCreatures = 0.05f;
			PreferenceIllCreatures = 999f;
			PreferenceImmuneCreatures = 0.75f;

			RecoveryMultiplier = 2f;
			DeathRateMultiplier = 0.2f;
			SpreadMultiplier = 0.5f;

			CanBeQuarantined = true;
			AllowInsideOnQuarantine = true;
			AllowOutsideOnQuarantine = false;
			CanBeSelfQuarantined = true;

			DisplayColor = Color4.DarkSlateBlue;
		}
	}
}
