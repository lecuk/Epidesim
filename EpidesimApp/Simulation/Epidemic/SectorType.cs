using OpenTK.Graphics;

namespace Epidesim.Simulation.Epidemic
{
	abstract class SectorType
	{
		public string Name { get; set; }
		public Color4 DisplayColor { get; set; }
		public float ProbabilityToAppear { get; set; }

		public ValueDistribution IdleTimeDistribution { get; set; }
		public ValueDistribution PositionDistribution { get; set; }

		public float SquareMetersPerCreature { get; set; }
		public float PreferenceHealthyCreatures { get; set; }
		public float PreferenceIllCreatures { get; set; }
		public float PreferenceImmuneCreatures { get; set; }
		public float RecoveryMultiplier { get; set; }
		public float DeathRateMultiplier { get; set; }
		public float SpreadMultiplier { get; set; }

		public bool CanBeQuarantined { get; set; }
		public bool AllowInsideOnQuarantine { get; set; }
		public bool AllowOutsideOnQuarantine { get; set; }
		public bool CanBeSelfQuarantined { get; set; }
	}
}
