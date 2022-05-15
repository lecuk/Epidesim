namespace Epidesim.Simulation.Epidemic
{
	abstract class SectorInfo
	{
		public abstract string Name { get; }
		public abstract ValueDistribution SquareMetersPerCreature { get; }
		public abstract ValueDistribution IdleTime { get; }
		public abstract ValueDistribution PositionDistribution { get; }

		public abstract float AllowHealthyCreatures { get; }
		public abstract float AllowIllCreatures { get; }
		public abstract float AllowImmuneCreatures { get; }
	}
}
