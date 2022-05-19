using Epidesim.Engine.Drawing.Types;
using OpenTK;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Epidesim.Simulation.Epidemic
{
	class Sector : IEnumerable<Creature>
	{
		public string Name { get; set; }
		public int Col { get; set; }
		public int Row { get; set; }
		public int MaxCreatures { get; set; }
		public Rectangle Bounds { get; set; }
		public IReadOnlyList<Sector> NeighbourSectors { get; set; }
		public SectorType Type { get; set; }
		public bool IsQuarantined { get; set; }

		public bool AllowInside => !IsQuarantined || Type.AllowInsideOnQuarantine;
		public bool AllowOutside => !IsQuarantined || Type.AllowOutsideOnQuarantine;

		public readonly CreatureCollection Creatures;

		public Sector()
		{
			Creatures = new CreatureCollection();
		}

		public IEnumerator<Creature> GetEnumerator()
		{
			return Creatures.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Creatures.GetEnumerator();
		}

		public bool IsFull()
		{
			return Creatures.Count >= MaxCreatures;
		}
		
		public Vector2 GetRandomPoint()
		{
			float a = (float)Type.PositionDistribution.GetRandomValue();
			float b = (float)Type.PositionDistribution.GetRandomValue();

			return Bounds.Center + new Vector2(a, b) * new Vector2(Bounds.Width, Bounds.Height) / 2;
		}

		public float SectorCreaturePreference(Creature creature)
		{
			return (creature.IsIll) ? Type.PreferenceIllCreatures
				: (creature.IsImmune) ? Type.PreferenceImmuneCreatures
				: Type.PreferenceHealthyCreatures;
		}

		public float GetInfectionProbability(Illness illness, CreatureBehaviour behaviour)
		{
			int contagious = Creatures.Contagious.Count;
			int ill = Creatures.Ill.Count;
			int quarantined = Creatures.Ill.Count(cr => cr.IsQuarantined);
			int latent = contagious - ill;

			float quarantineMultiplier = IsQuarantined ? behaviour.QuarantineSpreadMultiplier : 1.0f;
			float weightedIll = quarantined * behaviour.SelfQuarantineSpreadMultiplier + ill;
			float illCountMultiplier = latent * illness.IncubationPeriodSpread + weightedIll * illness.IllnessPeriodSpread;
			float spreadProbabilityPerSecond = quarantineMultiplier * illCountMultiplier * Type.SpreadMultiplier;

			return spreadProbabilityPerSecond;
		}
	}
}
