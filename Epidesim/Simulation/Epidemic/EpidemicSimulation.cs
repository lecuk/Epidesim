using Epidesim.Engine;
using Epidesim.Engine.Drawing.Types;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Epidesim.Simulation.Epidemic
{
	class EpidemicSimulation : ISimulation
	{
		public float WorldSize { get; private set; }

		float a = 0;

		public List<Creature> Creatures { get; private set; }
		public LinkedList<Creature> HealthyCreatures { get; private set; }
		public LinkedList<Creature> IllCreatures { get; private set; }

		public CoordinateSystem CoordinateSystem { get; private set; }

		public Vector2 MousePosition { get; private set; }
		public Vector2 WorldMousePosition { get; private set; }

		public GaussianDistribution positionDistribution;

		public EpidemicSimulation(float worldSize, int numberOfCreatures)
		{
			Creatures = new List<Creature>();
			HealthyCreatures = new LinkedList<Creature>();
			IllCreatures = new LinkedList<Creature>();
			positionDistribution = new GaussianDistribution(worldSize / 2, worldSize / 5);

			WorldSize = worldSize;

			CoordinateSystem = new CoordinateSystem()
			{
				ViewRectangle = Rectangle.FromTwoPoints(Vector2.Zero, new Vector2(WorldSize, WorldSize))
			};

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

		public void SetScreenSize(float screenWidth, float screenHeight)
		{
			float curRatio = CoordinateSystem.ViewRectangle.Width / CoordinateSystem.ViewRectangle.Height;
			float targetRatio = screenWidth / screenHeight;

			if (screenWidth > screenHeight)
			{
				CoordinateSystem.ViewRectangle = CoordinateSystem.ViewRectangle.Scale(new Vector2(1, curRatio / targetRatio));
			}
			else
			{
				CoordinateSystem.ViewRectangle = CoordinateSystem.ViewRectangle.Scale(new Vector2(targetRatio / curRatio, 1));
			}

			CoordinateSystem.ScreenWidth = screenWidth;
			CoordinateSystem.ScreenHeight = screenHeight;
		}

		public void Start()
		{
			// patient zero
			MakeIll(Creatures[0]);
		}

		public void Update(double deltaTime)
		{
			float fDeltaTime = (float)deltaTime;

			MousePosition = Input.GetMouseLocalPosition();
			WorldMousePosition = CoordinateSystem.ScreenCoordinateToWorldCoordinate(MousePosition);
			Debug.WriteLine(CoordinateSystem.WorldCoordinateToScreenCoordinate(Vector2.Zero));

			if (Input.IsKeyDown(OpenTK.Input.Key.Up))
			{
				CoordinateSystem.ViewRectangle = CoordinateSystem.ViewRectangle.Translate(
					new Vector2(0, CoordinateSystem.ViewRectangle.Height * fDeltaTime));
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Down))
			{
				CoordinateSystem.ViewRectangle = CoordinateSystem.ViewRectangle.Translate(
					new Vector2(0, -CoordinateSystem.ViewRectangle.Height * fDeltaTime));
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Left))
			{
				CoordinateSystem.ViewRectangle = CoordinateSystem.ViewRectangle.Translate(
					new Vector2(-CoordinateSystem.ViewRectangle.Width * fDeltaTime, 0));
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Right))
			{
				CoordinateSystem.ViewRectangle = CoordinateSystem.ViewRectangle.Translate(
					new Vector2(CoordinateSystem.ViewRectangle.Width * fDeltaTime, 0));
			}
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