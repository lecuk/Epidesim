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
		
		public List<Creature> Creatures { get; private set; }
		public LinkedList<Creature> HealthyCreatures { get; private set; }
		public LinkedList<Creature> IllCreatures { get; private set; }

		public CoordinateSystem CoordinateSystem { get; private set; }
		public Rectangle Camera
		{
			get => CoordinateSystem.ViewRectangle;
			set
			{
				CoordinateSystem.ViewRectangle = value;
			}
		}

		public Vector2 MousePosition { get; private set; }
		public Vector2 WorldMousePosition { get; private set; }

		public Vector2 MouseDelta { get; private set; }
		public Vector2 WorldMouseDelta { get; private set; }

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
					Position = new Vector2((float)positionDistribution.GetRandomValue(), (float)positionDistribution.GetRandomValue()),
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
				ScaleCamera(1, curRatio / targetRatio);
			}
			else
			{
				ScaleCamera(targetRatio / curRatio, 1);
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

			if (Input.IsKeyDown(OpenTK.Input.Key.Up))
			{
				TranslateCamera(0, Camera.Height * fDeltaTime);
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Down))
			{
				TranslateCamera(0, -Camera.Height * fDeltaTime);
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Left))
			{
				TranslateCamera(-Camera.Width * fDeltaTime, 0);
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Right))
			{
				TranslateCamera(Camera.Width * fDeltaTime, 0);
			}

			if (Input.IsMouseButtonDown(OpenTK.Input.MouseButton.Right))
			{
				TranslateCamera(-WorldMouseDelta);
			}

			if (Input.GetMouseWheelDelta() > 0)
			{
				ScaleCamera(1 / 1.1f);
			}

			if (Input.GetMouseWheelDelta() < 0)
			{
				ScaleCamera(1.1f);
			}

			MousePosition = Input.GetMouseLocalPosition();
			WorldMousePosition = CoordinateSystem.ScreenCoordinateToWorldCoordinate(MousePosition);

			MouseDelta = Input.GetMouseDelta();
			WorldMouseDelta = CoordinateSystem.ScreenDeltaToWorldDelta(MouseDelta);

			AddCreature(new Creature()
			{
				Name = String.Format("Creature {0}", Creatures.Count),
				Position = new Vector2((float)positionDistribution.GetRandomValue(), (float)positionDistribution.GetRandomValue()),
				IsIll = false
			});
		}

		void TranslateCamera(float offsetX, float offsetY)
		{
			TranslateCamera(new Vector2(offsetX, offsetY));
		}

		void TranslateCamera(Vector2 offset)
		{
			Vector2 limitedOffset = offset;

			if (Camera.Center.X + offset.X > WorldSize)
			{
				limitedOffset.X = WorldSize - Camera.Center.X;
			}

			if (Camera.Center.X + offset.X < 0)
			{
				limitedOffset.X = -Camera.Center.X;
			}

			if (Camera.Center.Y + offset.Y > WorldSize)
			{
				limitedOffset.Y = WorldSize - Camera.Center.Y;
			}

			if (Camera.Center.Y + offset.Y < 0)
			{
				limitedOffset.Y = -Camera.Center.Y;
			}

			Camera = Camera.Translate(limitedOffset);
		}

		void ScaleCamera(float scale)
		{
			ScaleCamera(new Vector2(scale));
		}

		void ScaleCamera(float scaleX, float scaleY)
		{
			ScaleCamera(new Vector2(scaleX, scaleY));
		}

		void ScaleCamera(Vector2 scale)
		{
			float cameraLimit = 1.25f;

			if (Camera.Height * scale.Y < WorldSize * cameraLimit || 
				Camera.Width * scale.X < WorldSize * cameraLimit)
			{
				Camera = Camera.Scale(scale);
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