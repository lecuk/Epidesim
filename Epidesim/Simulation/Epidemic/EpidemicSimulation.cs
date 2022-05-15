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
		public City City { get; private set; }
		public int NumberOfCreatures { get; private set; }

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

		private Random random;

		public EpidemicSimulation()
		{
			var builder = new CityBuilder()
			{
				SectorSize = 40f,
				RoadWidth = 5f
			};

			City = builder.Build(12, 8);
			NumberOfCreatures = 12000;

			CoordinateSystem = new CoordinateSystem()
			{
				ViewRectangle = City.Bounds
			};

			random = new Random();
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
			var startIdleDeviation = new GaussianDistribution(random)
			{
				Mean = 60,
				Deviation = 60,
				Min = 0
			};

			var speedDeviation = new GaussianDistribution(random)
			{
				Mean = 7,
				Deviation = 3,
				Min = 4
			};

			for (int i = 0; i < NumberOfCreatures; ++i)
			{
				int randomCol = random.Next(City.Cols);
				int randomRow = random.Next(City.Rows);
				Sector sector = City[randomCol, randomRow];

				var name = String.Format("Creature {0}", i + 1);
				var position = GetRandomPointInSector(sector);

				City.CreateCreature(new Creature()
				{
					Name = name,
					Position = position,
					CurrentSector = sector,
					TargetPoint = position,
					TargetSector = sector,
					IsIll = false,
					IsIdle = true,
					MoveSpeed = (float)speedDeviation.GetRandomValue(),
					IdleTime = (float)startIdleDeviation.GetRandomValue(),
				});
			}

			foreach (var creature in City)
			{
				if (random.NextDouble() < 0.02)
				{
					creature.IsIll = true;
				}
			}
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
			
			foreach (var creature in City)
			{
				if (creature.IsIdle)
				{
					creature.IdleTime -= fDeltaTime;

					if (creature.IdleTime <= 0)
					{
						SetNextRandomTargetForCreature(creature);
						creature.IsIdle = false;
					}
				}
				else
				{
					float distanceToMove = creature.MoveSpeed * fDeltaTime;

					if (Vector2.DistanceSquared(creature.TargetPoint, creature.Position) < distanceToMove * distanceToMove)
					{
						creature.Position = creature.TargetPoint;
						creature.IdleTime = (float)creature.TargetSector.IdleTime.GetRandomValue();
						creature.IsIdle = true;
					}
					else
					{
						Vector2 direction = creature.TargetPoint - creature.Position;
						direction.NormalizeFast();
						creature.Position += direction * distanceToMove;
					}

					City.UpdateCreatureSectorFromPosition(creature);
				}
			}
		}

		void SetNextRandomTargetForCreature(Creature creature)
		{
			var neighbours = creature.CurrentSector.NeighbourSectors;
			int targetSectorIndex = random.Next(neighbours.Count);
			var targetSector = neighbours[targetSectorIndex];

			creature.TargetPoint = GetRandomPointInSector(targetSector);
			creature.TargetSector = targetSector;
		}

		Vector2 GetRandomPointInSector(Sector sector)
		{
			return sector.Bounds.Center + new Vector2(
				(float)sector.PositionDistribution.GetRandomValue(), 
				(float)sector.PositionDistribution.GetRandomValue());
		}

		void TranslateCamera(float offsetX, float offsetY)
		{
			TranslateCamera(new Vector2(offsetX, offsetY));
		}

		void TranslateCamera(Vector2 offset)
		{
			Vector2 limitedOffset = offset;

			if (Camera.Center.X + offset.X > City.Bounds.Width)
			{
				limitedOffset.X = City.Bounds.Width - Camera.Center.X;
			}

			if (Camera.Center.X + offset.X < 0)
			{
				limitedOffset.X = -Camera.Center.X;
			}

			if (Camera.Center.Y + offset.Y > City.Bounds.Height)
			{
				limitedOffset.Y = City.Bounds.Height - Camera.Center.Y;
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

			if (Camera.Width * scale.X < City.Bounds.Width * cameraLimit ||
				Camera.Height * scale.Y < City.Bounds.Height * cameraLimit)
			{
				Camera = Camera.Scale(scale);
			}
		}

		void MakeIll(Creature creature)
		{
			creature.IsIll = true;
		}
	}
}