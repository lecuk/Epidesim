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

		public float TimeScale { get; set; }
		public float TotalTimeElapsed { get; set; }

		public Creature SelectedCreature { get; private set; }

		public Illness IllnessInfo { get; private set; }
		public CreatureBehaviour CreatureBehaviour { get; private set; }

		private Random random;

		public EpidemicSimulation()
		{
			this.random = new Random();

			var builder = new CityBuilder(random)
			{
				SectorSize = 25f,
				RoadWidth = 4f
			};

			City = builder.Build(30, 20);
			NumberOfCreatures = 8000;
			TimeScale = 2f;

			CoordinateSystem = new CoordinateSystem()
			{
				ViewRectangle = City.Bounds
			};

			IllnessInfo = new Illness()
			{
				Name = "Test illness",
				Description = "description",
				IncubationPeriodSpread = 0.0015f,
				IllnessPeriodSpread = 0.004f,
				FatalityRate = 0.00033f,
				ImmunityRate = 0.0f,
				UnsymptomaticRate = 0.05f,
				IncubationPeriodDuration = new GaussianDistribution(random)
				{
					Mean = 33,
					Deviation = 16,
					Min = 15
				},
				IllnessPeriodDuration = new GaussianDistribution(random)
				{
					Mean = 70,
					Deviation = 20,
					Min = 30
				},
				TemporaryImmunityDuration = new GaussianDistribution(random)
				{
					Mean = 150,
					Deviation = 30,
					Min = 60
				},
			};

			CreatureBehaviour = new CreatureBehaviour()
			{
				QuarantineThreshold = 4,
				QuarantineCancelThreshold = 0,
				PreferenceToStayInSameSectorMultiplier = 2f,
				PreferenceToStayInSameSectorWhenIllMultiplier = 6f,
				QuarantineSpreadMultiplier = 0.33f,
				SelfQuarantineSpreadMultiplier = 0.15f,
				SelfQuarantineDelayDistribution = new GaussianDistribution(random)
				{
					Mean = 20,
					Deviation = 10,
					Min = 0
				},
				SelfQuarantineCooldownDistribution = new GaussianDistribution(random)
				{
					Mean = 200,
					Deviation = 60,
					Min = 60
				}
			};
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
				var position = sector.GetRandomPoint();

				var creature = new Creature(random)
				{
					Name = name,
					Position = position,
					CurrentSector = sector,
					TargetPoint = position,
					TargetSector = sector,
					MoveSpeed = (float)speedDeviation.GetRandomValue(),
					City = City,
					Illness = null,
					Behaviour = CreatureBehaviour
				};

				creature.StoppedIdling += (cr) =>
				{
					SetNextRandomTargetForCreature(cr);
				};

				creature.Recovered += (cr) =>
				{
					cr.StopIdling();
				};

				City.CreateCreature(creature);
			}

			int firstIllCreatures = 5;
			foreach (var creature in City)
			{
				//patient zero
				creature.Contaminate(IllnessInfo);
				--firstIllCreatures;
				if (firstIllCreatures <= 0) break;
			}
		}

		public void Update(double deltaTime)
		{
			float scaledDeltaTime = (float)deltaTime * TimeScale;

			TotalTimeElapsed += scaledDeltaTime;

			if (Input.IsKeyDown(OpenTK.Input.Key.Up))
			{
				TranslateCamera(0, Camera.Height * scaledDeltaTime);
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Down))
			{
				TranslateCamera(0, -Camera.Height * scaledDeltaTime);
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Left))
			{
				TranslateCamera(-Camera.Width * scaledDeltaTime, 0);
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Right))
			{
				TranslateCamera(Camera.Width * scaledDeltaTime, 0);
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
				TimeScale *= MathHelper.Clamp(1.0f + (float)deltaTime * 2.5f, 0.1f, 2f);
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Minus))
			{
				TimeScale /= MathHelper.Clamp(1.0f + (float)deltaTime * 2.5f, 0.1f, 2f);
			}

			MousePosition = Input.GetMouseLocalPosition();
			WorldMousePosition = CoordinateSystem.ScreenCoordinateToWorldCoordinate(MousePosition);

			MouseDelta = Input.GetMouseDelta();
			WorldMouseDelta = CoordinateSystem.ScreenDeltaToWorldDelta(MouseDelta);

			if (Input.WasMouseButtonJustPressed(OpenTK.Input.MouseButton.Left))
			{
				SelectedCreature = null;
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

				if (creature.IsDead)
				{
					continue;
				}

				creature.Update(scaledDeltaTime);

				if (!creature.IsIdle)
				{
					float distanceToMove = creature.MoveSpeed * scaledDeltaTime;

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
					int quarantined = sector.Creatures.Ill.Count(cr => cr.IsQuarantined);
					int latent = contagious - ill;
					
					if (contagious > 0)
					{
						var vulnerableCreatures = new List<Creature>();
						vulnerableCreatures.AddRange(sector.Creatures.Vulnerable);

						foreach (var possibleIllCreature in vulnerableCreatures)
						{
							float quarantineMultiplier = sector.IsQuarantined ? CreatureBehaviour.QuarantineSpreadMultiplier : 1.0f;
							float weightedIll = quarantined * CreatureBehaviour.SelfQuarantineSpreadMultiplier + ill;
							float sqrlWeightedIll = (float)Math.Sqrt(weightedIll);
							float illCountMultiplier = latent * IllnessInfo.IncubationPeriodSpread + weightedIll * IllnessInfo.IllnessPeriodSpread;
							float spreadProbabilityPerSecond = quarantineMultiplier * illCountMultiplier * sector.SpreadMultiplier;
							float spreadPossibility = (float)random.NextDouble();

							if (spreadPossibility < spreadProbabilityPerSecond * scaledDeltaTime)
							{
								possibleIllCreature.Contaminate(IllnessInfo);
							}
						}
					}

					if (ill >= CreatureBehaviour.QuarantineThreshold && sector.CanBeQuarantined)
					{
						sector.IsQuarantined = true;
					}
					else if (ill <= CreatureBehaviour.QuarantineCancelThreshold)
					{
						sector.IsQuarantined = false;
					}
				}
			}
		}

		List<Creature> FindNeighbouringVulnerableCreatures(Creature creature)
		{
			var sector = creature.CurrentSector;
			var neighbourSectors = sector.NeighbourSectors;
			List<Creature> neighbours = new List<Creature>(sector.Creatures.Vulnerable);
			return neighbours;
		}

		void SetNextRandomTargetForCreature(Creature creature)
		{
			var sector = creature.CurrentSector;
			var neighbours = sector.NeighbourSectors;
			
			var possibleTargets = new ProbabilityTable<Sector>();
			
			float stayIdlePreference = (creature.IsIll) 
				? CreatureBehaviour.PreferenceToStayInSameSectorWhenIllMultiplier 
				: CreatureBehaviour.PreferenceToStayInSameSectorMultiplier;

			possibleTargets.AddOutcome(sector, stayIdlePreference * sector.SectorCreaturePreference(creature));

			if (creature.IsImmune || sector.AllowOutside)
			{
				foreach (var neighbourSector in neighbours)
				{
					if (!neighbourSector.AllowInside || neighbourSector.Creatures.Count >= neighbourSector.MaxCreatures)
					{
						continue;
					}

					float preferenceCoefficient = neighbourSector.SectorCreaturePreference(creature);
					possibleTargets.AddOutcome(neighbourSector, preferenceCoefficient);
				}
			}

			var targetSector = possibleTargets.GetRandomOutcome();
			creature.TargetPoint = targetSector.GetRandomPoint();
			creature.TargetSector = targetSector;
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
	}
}