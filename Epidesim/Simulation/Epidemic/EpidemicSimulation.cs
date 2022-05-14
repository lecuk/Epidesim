using Epidesim.Engine;
using Epidesim.Engine.Drawing.Types;
using OpenTK;
using System;
using System.Collections.Generic;

namespace Epidesim.Simulation.Epidemic
{
	class EpidemicSimulation : ISimulation
	{
		public float ScreenWidth { get; set; }
		public float ScreenHeight { get; set; }

		public float WorldSize { get; private set; }

		float a = 0;

		public List<Creature> Creatures { get; private set; }
		public LinkedList<Creature> HealthyCreatures { get; private set; }
		public LinkedList<Creature> IllCreatures { get; private set; }

		public CoordinateSystem CoordinateSystem { get; private set; }

		public GaussianDistribution positionDistribution;

		public EpidemicSimulation(float worldSize, int numberOfCreatures)
		{
			Creatures = new List<Creature>();
			HealthyCreatures = new LinkedList<Creature>();
			IllCreatures = new LinkedList<Creature>();
			positionDistribution = new GaussianDistribution(worldSize / 2, worldSize / 5);

			WorldSize = worldSize;
			CoordinateSystem = new CoordinateSystem();

			for (int i = 0; i < numberOfCreatures; ++i)
			{
				AddCreature(new Creature()
				{
					Name = String.Format("Creature {0}", i + 1),
					Position = new OpenTK.Vector2((float)positionDistribution.GetRandomValue(), (float)positionDistribution.GetRandomValue()),
					IsIll = false
				});
			}
		}

		public void Start()
		{
			// patient zero
			MakeIll(Creatures[0]);
		}

		public void Update(double deltaTime)
		{
			CoordinateSystem = new CoordinateSystem()
			{
				ScreenWidth = ScreenWidth,
				ScreenHeight = ScreenHeight,
				ViewRectangle = Rectangle.FromTwoPoints(new Vector2(a), new Vector2(WorldSize, WorldSize / ScreenWidth * ScreenHeight) + new Vector2(a))
			};

			//a += (float)deltaTime;
		}

		void AddCreature(Creature creature)
		{
			Creatures.Add(creature);
			HealthyCreatures.AddLast(creature);
		}

		void MakeIll(Creature creature)
		{
			creature.IsIll = true;
			HealthyCreatures.Remove(creature);
			IllCreatures.AddLast(creature);
		}
	}
}