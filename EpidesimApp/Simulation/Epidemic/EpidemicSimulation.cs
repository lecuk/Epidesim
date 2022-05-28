using Epidesim.Engine;
using Epidesim.Engine.Drawing.Types;
using Epidesim.Simulation.Epidemic.Distributions;

using OpenTK;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Epidesim.Simulation.Epidemic
{
	class EpidemicSimulation : ISimulation
	{
		public bool IsPaused { get; set; }
		public float TimeScale { get; set; }
		public float PeriodSize { get; set; }

		public CityBlueprint CityBlueprint { get; set; }
		public Illness Illness { get; set; }
		public CreatureBehaviour CreatureBehaviour { get; set; }

		public City City { get; private set; }

		public CoordinateSystem CoordinateSystem { get; private set; }
		public CoordinateSystem ScreenCoordinateSystem { get; private set; }

		public Rectangle Camera
		{
			get => CoordinateSystem.ViewRectangle;
			set
			{
				CoordinateSystem.ViewRectangle = value;
			}
		}

		public Vector2 ScreenSize => new Vector2(CoordinateSystem.ScreenWidth, CoordinateSystem.ScreenHeight);

		public Vector2 MousePosition { get; private set; }
		public Vector2 WorldMousePosition { get; private set; }

		public Vector2 MouseDelta { get; private set; }
		public Vector2 WorldMouseDelta { get; private set; }

		public float DeltaTime { get; private set; }
		public float ScaledDeltaTime { get; private set; }
		public float TotalScaledTimeElapsed { get; private set; }
		public float TotalTimeElapsed { get; private set; }

		private Queue<float> FPSList { get; set; }
		public float FPS => FPSList.Average();

		public bool IsIncreasingSpeed { get; private set; }
		public bool IsDecreasingSpeed { get; private set; }

		public Creature SelectedCreature { get; private set; }
		public Sector SelectedSector { get; private set; }

		public SimulationStats Stats { get; private set; }
		public SimulationStats.Period CurrentPeriod { get; private set; }
		public float NextPeriodUpdateAt { get; private set; }

		private Random random;

		public EpidemicSimulation()
		{
			this.random = new Random();

			FPSList = new Queue<float>();

			CoordinateSystem = new CoordinateSystem()
			{
				ViewRectangle = Rectangle.FromTwoPoints(Vector2.Zero, Vector2.One),
			};

			ScreenCoordinateSystem = new CoordinateSystem()
			{
				ViewRectangle = Rectangle.FromTwoPoints(Vector2.Zero, Vector2.One)
			};

			CityBlueprint = CityBlueprint.Default(random);
			Illness = Illness.Default(random);
			CreatureBehaviour = CreatureBehaviour.Default(random);
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

			ScreenCoordinateSystem.ScreenWidth = screenWidth;
			ScreenCoordinateSystem.ScreenHeight = screenHeight;

			ScreenCoordinateSystem.ViewRectangle = Rectangle.FromTwoPoints(Vector2.Zero, new Vector2(screenWidth, screenHeight));
		}

		public void Reset()
		{
			TotalTimeElapsed = 0;
			TotalScaledTimeElapsed = 0;
			TimeScale = 1;
			PeriodSize = 10;
			NextPeriodUpdateAt = PeriodSize;
			IsPaused = true;
			Stats = new SimulationStats();
		}

		public void Start()
		{
			Reset();
			ResetPeriod();

			var builder = new CityBuilder(random);

			City = builder.Build(CityBlueprint);
			CoordinateSystem.ViewRectangle = City.Bounds;

			var startIdleDeviation = new GaussianDistribution(random)
			{
				Mean = 40,
				Deviation = 20,
				Min = 0
			};

			for (int i = 0; i < CityBlueprint.Population; ++i)
			{
				int randomCol = random.Next(City.Cols);
				int randomRow = random.Next(City.Rows);
				Sector sector = City[randomCol, randomRow];

				var name = String.Format("Creature {0}", i + 1);
				var position = sector.GetRandomPoint();

				var creature = new Creature(random)
				{
					Name = name,
					Position = position,
					CurrentSector = sector,
					TargetPoint = position,
					TargetSector = sector,
					MoveSpeed = (float)CreatureBehaviour.MoveSpeedDistribution.GetRandomValue(),
					City = City,
					Illness = null,
					Behaviour = CreatureBehaviour
				};

				creature.Infected += (cr) =>
				{
					CurrentPeriod.NewInfections++;
				};

				creature.ShownSymptoms += (cr) =>
				{
					CurrentPeriod.NewConfirmedCases++;
				};

				creature.Died += (cr) =>
				{
					CurrentPeriod.NewDeaths++;
				};

				creature.StoppedIdling += (cr) =>
				{
					cr.SelectNextTarget();
				};

				creature.Recovered += (cr) =>
				{
					CurrentPeriod.NewRecoveries++;
					cr.StopIdling();
				};

				City.CreateCreature(creature);
			}

			int firstIllCreatures = 5;
			foreach (var creature in City)
			{
				//patient zero
				creature.Contaminate(Illness);
				--firstIllCreatures;
				if (firstIllCreatures <= 0) break;
			}
		}

		public void Update(double deltaTime)
		{
			DeltaTime = (float)deltaTime;
			ScaledDeltaTime = DeltaTime * TimeScale;

			HandleMouseAndCamera(DeltaTime);

			if (FPSList.Count > 10)
			{
				FPSList.Dequeue();
			}

			FPSList.Enqueue(1.0f / DeltaTime);

			if (Input.WasMouseButtonJustPressed(OpenTK.Input.MouseButton.Left))
			{
				SelectedCreature = null;
				SelectedSector = City.GetSectorAtLocation(WorldMousePosition);
			}

			foreach (var creature in City)
			{
				if (Input.WasMouseButtonJustPressed(OpenTK.Input.MouseButton.Left))
				{
					float sqrDistance = Vector2.DistanceSquared(WorldMousePosition, creature.Position);

					if (sqrDistance < 2f)
					{
						if (SelectedCreature == null)
						{
							SelectedCreature = creature;
						}
						else
						{
							float curSqrDistance = Vector2.DistanceSquared(WorldMousePosition, SelectedCreature.Position);

							if (sqrDistance < curSqrDistance)
							{
								SelectedCreature = creature;
							}
						}
					}
				}
			}

			if (SelectedCreature != null)
			{
				SelectedSector = SelectedCreature.CurrentSector;
			}

			TotalTimeElapsed += DeltaTime;

			if (IsPaused)
			{
				return;
			}

			TotalScaledTimeElapsed += ScaledDeltaTime;

			foreach (var creature in City)
			{
				if (creature.IsDead)
				{
					continue;
				}

				creature.Update(ScaledDeltaTime);

				if (!creature.IsIdle)
				{
					float distanceToMove = creature.MoveSpeed * ScaledDeltaTime;

					if (Vector2.DistanceSquared(creature.TargetPoint, creature.Position) < distanceToMove * distanceToMove)
					{
						creature.Position = creature.TargetPoint;
						creature.StartIdling();
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

			for (int r = 0; r < City.Rows; ++r)
			{
				for (int c = 0; c < City.Cols; ++c)
				{
					Sector sector = City[c, r];
					int contagious = sector.Creatures.Contagious.Count;
					int ill = sector.Creatures.Ill.Count;
					
					if (contagious > 0)
					{
						var vulnerableCreatures = new List<Creature>();
						vulnerableCreatures.AddRange(sector.Creatures.Vulnerable);
						float spreadProbabilityPerSecond = sector.GetInfectionProbability(Illness, CreatureBehaviour);

						foreach (var possibleIllCreature in vulnerableCreatures)
						{
							float spreadPossibility = (float)random.NextDouble();

							if (spreadPossibility < spreadProbabilityPerSecond * ScaledDeltaTime)
							{
								possibleIllCreature.Contaminate(Illness);
							}
						}
					}

					if (ill >= CreatureBehaviour.QuarantineThreshold && sector.Type.CanBeQuarantined)
					{
						sector.IsQuarantined = true;
					}
					else if (ill <= CreatureBehaviour.QuarantineCancelThreshold)
					{
						sector.IsQuarantined = false;
					}
				}
			}

			if (TotalScaledTimeElapsed > NextPeriodUpdateAt)
			{
				NextPeriodUpdateAt = TotalScaledTimeElapsed + PeriodSize;

				FixDataAndAddCurrentPeriod();
				ResetPeriod();
			}
		}

		List<Creature> FindNeighbouringVulnerableCreatures(Creature creature)
		{
			var sector = creature.CurrentSector;
			var neighbourSectors = sector.NeighbourSectors;
			List<Creature> neighbours = new List<Creature>(sector.Creatures.Vulnerable);
			return neighbours;
		}

		void HandleMouseAndCamera(float deltaTime)
		{
			if (Input.IsKeyDown(OpenTK.Input.Key.Up))
			{
				TranslateCamera(0, Camera.Height * deltaTime);
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Down))
			{
				TranslateCamera(0, -Camera.Height * deltaTime);
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Left))
			{
				TranslateCamera(-Camera.Width * deltaTime, 0);
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Right))
			{
				TranslateCamera(Camera.Width * deltaTime, 0);
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

			if (Input.IsKeyDown(OpenTK.Input.Key.Plus))
			{
				TimeScale *= MathHelper.Clamp(1.0f + (float)deltaTime, 0.1f, 2f);
				IsIncreasingSpeed = true;
			}
			else
			{
				IsIncreasingSpeed = false;
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Minus))
			{
				TimeScale /= MathHelper.Clamp(1.0f + (float)deltaTime, 0.1f, 2f);
				IsDecreasingSpeed = true;
			}
			else
			{
				IsDecreasingSpeed = false;
			}

			if (Input.WasKeyJustPressed(OpenTK.Input.Key.Space))
			{
				IsPaused = !IsPaused;
			}

			MousePosition = Input.GetMouseLocalPosition();
			WorldMousePosition = CoordinateSystem.ScreenCoordinateToWorldCoordinate(MousePosition);

			MouseDelta = Input.GetMouseDelta();
			WorldMouseDelta = CoordinateSystem.ScreenDeltaToWorldDelta(MouseDelta);
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

		void ResetPeriod()
		{
			CurrentPeriod = new SimulationStats.Period()
			{
				StartTime = TotalScaledTimeElapsed,
				NewConfirmedCases = 0,
				NewDeaths = 0,
				NewInfections = 0,
				NewRecoveries = 0
			};
		}

		void FixDataAndAddCurrentPeriod()
		{
			CurrentPeriod.EndTime = TotalScaledTimeElapsed;
			CurrentPeriod.TotalInfections = City.Count(creature => creature.IsInfected);
			CurrentPeriod.TotalDeaths = City.Count(creature => creature.IsDead);
			CurrentPeriod.TotalImmune = City.Count(creature => creature.IsImmune);

			Stats.AddPeriod(CurrentPeriod);
		}
	}
}