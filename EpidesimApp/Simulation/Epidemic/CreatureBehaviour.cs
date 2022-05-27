using Epidesim.Simulation.Epidemic.Distributions;
using System;

namespace Epidesim.Simulation.Epidemic
{
	class CreatureBehaviour
	{
		public ValueDistribution MoveSpeedDistribution { get; set; }

		public float PreferenceToStayInSameSectorMultiplier { get; set; }
		public float PreferenceToStayInSameSectorWhenIllMultiplier { get; set; }
		public float QuarantineSpreadMultiplier { get; set; }

		public int QuarantineThreshold { get; set; }
		public int QuarantineCancelThreshold { get; set; }

		public ValueDistribution SelfQuarantineDelayDistribution { get; set; }
		public ValueDistribution SelfQuarantineCooldownDistribution { get; set; }
		public float SelfQuarantineSpreadMultiplier { get; set; }

		public static CreatureBehaviour Default(Random random) => new CreatureBehaviour()
		{
			MoveSpeedDistribution = new GaussianDistribution(random)
			{
				Mean = 7,
				Deviation = 3,
				Min = 4
			},
			QuarantineThreshold = 3,
			QuarantineCancelThreshold = 0,
			PreferenceToStayInSameSectorMultiplier = 2f,
			PreferenceToStayInSameSectorWhenIllMultiplier = 5f,
			QuarantineSpreadMultiplier = 0.33f,
			SelfQuarantineSpreadMultiplier = 0.15f,
			SelfQuarantineDelayDistribution = new GaussianDistribution(random)
			{
				Mean = 20,
				Deviation = 10,
				Min = 1
			},
			SelfQuarantineCooldownDistribution = new GaussianDistribution(random)
			{
				Mean = 300,
				Deviation = 60,
				Min = 60
			}
		};
	}
}
