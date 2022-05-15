using OpenTK;

namespace Epidesim.Simulation.Epidemic
{
	class Creature
	{
		public string Name { get; set; }
		public float MoveSpeed { get; set; }
		public Vector2 Position { get; set; }
		public Sector CurrentSector { get; set; }
		public Vector2 TargetPoint { get; set; }
		public Sector TargetSector { get; set; }
		public float IdleTime { get; set; }
		public bool IsIll { get; set; }
		public bool IsIdle { get; set; }
	}
}
