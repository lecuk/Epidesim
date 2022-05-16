﻿using OpenTK;
using System;

namespace Epidesim.Simulation.Epidemic
{
	class Creature
	{
		public string Name { get; set; }
		public City City { get; set; }
		public float MoveSpeed { get; set; }
		public Vector2 Position { get; set; }
		public Sector CurrentSector { get; set; }
		public Vector2 TargetPoint { get; set; }
		public Sector TargetSector { get; set; }
		public Illness Illness { get; set; }

		public bool IsDead { get; private set; }
		public bool WasIllAtSomePoint { get; private set; }

		public float IdleDuration { get; private set; }
		public float IncubationDuration { get; private set; }
		public float IllnessDuration { get; private set; }
		public float ImmunityDuration { get; private set; }

		public bool IsIdle => IdleDuration > 0;
		public bool IsImmune => ImmunityDuration > 0;
		public bool IsInfected => Illness != null;
		public bool IsPermanentlyImmune => ImmunityDuration == Single.PositiveInfinity;
		public bool IsIll => IllnessDuration > 0;
		public bool IsLatent => IncubationDuration > 0;
		public bool IsContagious => IsInfected;

		public delegate void CreatureEventHandler(Creature creature);

		public event CreatureEventHandler StoppedIdling;
		public event CreatureEventHandler StartedIdling;
		public event CreatureEventHandler Contaminated;
		public event CreatureEventHandler ShownSymptoms;
		public event CreatureEventHandler Recovered;
		public event CreatureEventHandler ImmunityVanished;
		public event CreatureEventHandler Died;

		private readonly Random random;

		public Creature(Random random)
		{
			this.random = random;
		}

		public void Update(float deltaTime)
		{
			if (IsIdle)
			{
				if (IdleDuration <= deltaTime)
				{
					StopIdling();
				}
				else IdleDuration -= deltaTime;
			}

			if (IsImmune && !IsPermanentlyImmune)
			{
				if (ImmunityDuration <= deltaTime)
				{
					RemoveImmunity();
				}
				else ImmunityDuration -= deltaTime;
			}

			if (IsInfected)
			{
				if (IsLatent)
				{
					if (IncubationDuration <= deltaTime)
					{
						ShowSymptoms();
					}
					else IncubationDuration -= deltaTime;
				}

				if (IsIll)
				{
					float recoveryRate = deltaTime * CurrentSector.RecoveryMultiplier;
					if (IllnessDuration <= recoveryRate)
					{
						Recover();
					}
					else
					{
						IllnessDuration -= recoveryRate;

						double deathPossibility = random.NextDouble();
						double deathProbabilityPerSecond = Illness.FatalityRate * CurrentSector.DeathRateMultiplier;

						if (deathPossibility < deathProbabilityPerSecond * deltaTime)
						{
							Die();
						}
					}
				}
			}
		}

		public void StartIdling()
		{
			IdleDuration = (float)CurrentSector.IdleTimeDistribution.GetRandomValue();
			StartedIdling?.Invoke(this);
		}

		public void StopIdling()
		{
			IdleDuration = 0;
			StoppedIdling?.Invoke(this);
		}

		public void RemoveImmunity()
		{
			ImmunityDuration = 0;
			City.UpdateCreature(this);
			ImmunityVanished?.Invoke(this);
		}

		public void Contaminate(Illness illness)
		{
			Illness = illness;
			WasIllAtSomePoint = true;
			IncubationDuration = (float)Illness.IncubationPeriodDuration.GetRandomValue();
			City.UpdateCreature(this);
			Contaminated?.Invoke(this);
		}

		private void ShowSymptoms()
		{
			IncubationDuration = 0;
			IllnessDuration = (float)Illness.IncubationPeriodDuration.GetRandomValue();
			City.UpdateCreature(this);
			ShownSymptoms?.Invoke(this);
		}

		private void Recover()
		{
			IncubationDuration = 0;
			IllnessDuration = 0;

			double immunityPossibility = random.NextDouble();
			double immunityProbability = Illness.ImmunityRate;
			if (immunityPossibility < immunityProbability)
			{
				ImmunityDuration = Single.PositiveInfinity;
			}
			else
			{
				ImmunityDuration = (float)Illness.TemporaryImmunityDuration.GetRandomValue();
			}
			
			Illness = null;
			City.UpdateCreature(this);
			Recovered?.Invoke(this);
		}

		public void Die()
		{
			ImmunityDuration = 0;
			IdleDuration = 0;
			IncubationDuration = 0;
			IllnessDuration = 0;
			Illness = null;
			IsDead = true;
			City.UpdateCreature(this);
			Died?.Invoke(this);
		}
	}
}
