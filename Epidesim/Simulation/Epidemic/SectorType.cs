using OpenTK.Graphics;

namespace Epidesim.Simulation.Epidemic
{
	abstract class SectorType
	{
		public string Name { get; protected set; }
		public ValueDistribution IdleTimeDistribution { get; protected set; }
		public ValueDistribution PositionDistribution { get; protected set; }

		public float SquareMetersPerCreature { get; protected set; }
		public float PreferenceHealthyCreatures { get; protected set; }
		public float PreferenceIllCreatures { get; protected set; }
		public float PreferenceImmuneCreatures { get; protected set; }
		public float RecoveryMultiplier { get; protected set; }
		public float DeathRateMultiplier { get; protected set; }
		public float SpreadMultiplier { get; protected set; }

		public bool CanBeQuarantined { get; protected set; }
		public bool AllowInsideOnQuarantine { get; protected set; }
		public bool AllowOutsideOnQuarantine { get; protected set; }
		public bool CanBeSelfQuarantined { get; protected set; }

		public Color4 DisplayColor { get; protected set; }
	}
}
