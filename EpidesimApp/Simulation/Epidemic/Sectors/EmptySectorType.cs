using Epidesim.Simulation.Epidemic.Distributions;
using OpenTK.Graphics;
using System;

namespace Epidesim.Simulation.Epidemic.Sectors
{
	class EmptySectorType : SectorType
	{
		public EmptySectorType(Random random)
		{
			Name = "Empty";

			SquareMetersPerCreature = 40f;

			IdleTimeDistribution = new GaussianDistribution(random)
			{
				Mean = 14,
				Deviation = 8,
				Min = 2
			};

			PositionDistribution = new FixedDistribution(random)
			{
				Min = -1,
				Max = 1
			};

			PreferenceHealthyCreatures = 0.75f;
			PreferenceIllCreatures = 0.1f;
			PreferenceImmuneCreatures = 0.75f;

			RecoveryMultiplier = 0.5f;
			DeathRateMultiplier = 1f;
			SpreadMultiplier = 0.33f;

			CanBeQuarantined = false;
			CanBeSelfQuarantined = false;

			DisplayColor = Color4.Transparent;
			ProbabilityToAppear = 64;
		}
	}
}
