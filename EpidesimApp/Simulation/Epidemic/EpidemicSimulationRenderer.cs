using Epidesim.Engine;
using Epidesim.Engine.Drawing;
using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Epidesim.Simulation.Epidemic
{
	internal class EpidemicSimulationRenderer : ISimulationRenderer<EpidemicSimulation>
	{
		private readonly PrimitiveRenderer creatureRenderer;
		private readonly PrimitiveRenderer cityRenderer;
		private readonly PrimitiveRenderer selectionRenderer;
		private readonly PrimitiveRenderer sectorBoundsRenderer;
		private readonly TextRenderer sectorTextRenderer;
		private readonly QuadTextureRenderer haloRenderer;
		private readonly PrimitiveRenderer uiPanelRenderer;
		private readonly TextRenderer uiTextRenderer;

		private IEnumerable<Renderer> AllRenderers => new Renderer[]
		{
			creatureRenderer, cityRenderer, selectionRenderer, sectorBoundsRenderer, sectorTextRenderer, haloRenderer, uiPanelRenderer, uiTextRenderer
		};

		private IEnumerable<Renderer> WorldRenderers => new Renderer[]
		{
			creatureRenderer, cityRenderer, selectionRenderer, sectorBoundsRenderer, sectorTextRenderer, haloRenderer
		};

		private IEnumerable<Renderer> UIRenderers => new Renderer[]
		{
			uiPanelRenderer, uiTextRenderer
		};

		public EpidemicSimulationRenderer()
		{
			this.cityRenderer = new PrimitiveRenderer(100000, 100000, 100000);
			this.creatureRenderer = new PrimitiveRenderer(1000000, 1000000, 1000000);
			this.selectionRenderer = new PrimitiveRenderer(1000000, 1000000, 1000000);
			this.sectorBoundsRenderer = new PrimitiveRenderer(100000, 100000, 100000);
			this.sectorTextRenderer = new TextRenderer(2000);
			this.haloRenderer = new QuadTextureRenderer(30000, ResourceManager.GetProgram("textureDefault"));
			this.uiPanelRenderer = new PrimitiveRenderer(10000, 10000, 10000);
			this.uiTextRenderer = new TextRenderer(200);

			sectorTextRenderer.LoadFont(ResourceManager.GetTextureFont("consolas"));
			uiTextRenderer.LoadFont(ResourceManager.GetTextureFont("consolas"));

			GL.Enable(EnableCap.Blend);
			GL.Enable(EnableCap.AlphaTest);
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Multisample);

			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			GL.Hint(HintTarget.MultisampleFilterHintNv, HintMode.Fastest);
		}

		public void Render(EpidemicSimulation simulation)
		{
			var worldTransformMatrix = simulation.CoordinateSystem.GetTransformationMatrix();
			foreach (var renderer in WorldRenderers)
			{
				renderer.Reset();
				renderer.TransformMatrix = worldTransformMatrix;
			}

			var uiTransformMatrix = simulation.ScreenCoordinateSystem.GetTransformationMatrix();
			foreach (var renderer in UIRenderers)
			{
				renderer.Reset();
				renderer.TransformMatrix = uiTransformMatrix;
			}

			var city = simulation.City;
			var cityBounds = city.Bounds;

			cityRenderer.AddRectangle(cityBounds, Color4.DimGray);

			for (int r = 0; r < city.Rows; ++r)
			{
				for (int c = 0; c < city.Cols; ++c)
				{
					Sector sector = city[c, r];
					var sectorBounds = sector.Bounds;
					cityRenderer.AddRectangle(sectorBounds, sector.Type.DisplayColor);

					if (sector.Creatures.Contagious.Count > 0)
					{
						if (sector.IsQuarantined)
						{
							sectorBoundsRenderer.AddRectangle(sectorBounds, Color4.Orange);
						}
						else
						{
							sectorBoundsRenderer.AddRectangle(sectorBounds, Color4.Red);
						}
					}
				}
			}

			foreach (var creature in city)
			{
				if (creature.IsDead)
				{
					creatureRenderer.AddRectangle(Rectangle.FromCenterAndSize(creature.Position, new Vector2(1)),
						Color4.Black);
				}
			}

			foreach (var creature in city)
			{
				if (!creature.IsDead)
				{
					creatureRenderer.AddRectangle(Rectangle.FromCenterAndSize(creature.Position, new Vector2(1)),
					creature.IsIll
						? Color4.Red
						: creature.IsImmune
							? Color4.Cyan
							: Color4.White);
				}

				if (creature.IsInfected && !creature.IsQuarantined)
				{
					haloRenderer.AddQuad(Rectangle.FromCenterAndSize(creature.Position, new Vector2(4)), Color4.Red);
				}
			}
			
			if (simulation.SelectedCreature != null)
			{
				var selectedCreature = simulation.SelectedCreature;

				sectorBoundsRenderer.AddLine(selectedCreature.Position, selectedCreature.TargetPoint, Color4.Lime);

				var possibleSectors = selectedCreature.GetPossibleSectorTargets();
				foreach (var sector in possibleSectors.AllOutcomes)
				{
					double probability = 100 * possibleSectors.GetNormalizedOutcomeProbability(sector);
					Vector2 position = new Vector2(sector.Bounds.Lft, sector.Bounds.Bot) + new Vector2(2);
					string probabilityString = String.Format("{0:0.00}%", probability);

					sectorTextRenderer.AddString(probabilityString, 4, position, Color4.Yellow);
					sectorBoundsRenderer.AddRectangle(sector.Bounds, Color4.Yellow);
				}

				selectionRenderer.AddRectangle(Rectangle.FromCenterAndSize(selectedCreature.Position, new Vector2(3f)),
					Color4.White);

				string creatureInfo = String.Empty;
				if (selectedCreature.IsDead)
				{
					creatureInfo = String.Format("{0} (Dead)", selectedCreature.Name);
				}
				else
				{
					creatureInfo = String.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}",
					selectedCreature.Name,
					selectedCreature.IsIdle ? String.Format("Idle ({0:0.0}s)", selectedCreature.IdleDuration) : "Moving",
					selectedCreature.IsLatent
						? String.Format("Latent for {0:0.0}s", selectedCreature.IncubationDuration)
						: selectedCreature.IsIll
							? String.Format("Ill for {0:0.0}s", selectedCreature.IllnessDuration)
							: selectedCreature.IsImmune && !selectedCreature.IsPermanentlyImmune
								? String.Format("Immune for {0:0.0}s", selectedCreature.ImmunityDuration)
								: selectedCreature.IsPermanentlyImmune
									? "Permamently immune"
									: "Healthy",
					selectedCreature.IsRestingFromSelfQuarantine
						? String.Format("Not able to quarantine for {0:0.0}s", selectedCreature.SelfQuarantineCooldown)
						: "Able to self-quarantine",
					selectedCreature.IsWaitingForQuarantine
						? String.Format("Quarantine in {0:0.0}s", selectedCreature.SelfQuarantineWaiting)
						: selectedCreature.IsQuarantined
							? "In quarantine"
							: "Not in quarantine",
					selectedCreature.WasIllAtSomePoint
						? "Was ill at least once"
						: "Never was ill");
				}

				uiTextRenderer.AddString(creatureInfo, 18f, new Vector2(simulation.ScreenSize.X - 250 - 1, 180 - 1), Color4.Black);
				uiTextRenderer.AddString(creatureInfo, 18f, new Vector2(simulation.ScreenSize.X - 250, 180), Color4.White);
			}

			if (simulation.SelectedSector != null)
			{
				var selectedSector = simulation.SelectedSector;
				sectorBoundsRenderer.AddRectangle(selectedSector.Bounds, Color4.White);
				float infectionProbabilityPerSecond = selectedSector.GetInfectionProbability(simulation.Illness, simulation.CreatureBehaviour);
				
				string sectorInfo = String.Format("{0}\n{1}\n{2}\n{3}", 
					selectedSector.Name,
					String.Format("Capacity {0}/{1} creatures", selectedSector.Creatures.Count, selectedSector.MaxCreatures),
					String.Format("Infection chance {0:0.000}%", 100f * infectionProbabilityPerSecond),
					selectedSector.IsQuarantined
						? "In quarantine"
						: "Not in quarantine");

				uiTextRenderer.AddString(sectorInfo, 18f, new Vector2(simulation.ScreenSize.X - 250 - 1, 64 - 1), Color4.Black);
				uiTextRenderer.AddString(sectorInfo, 18f, new Vector2(simulation.ScreenSize.X - 250, 64), Color4.White);
			}

			int population = simulation.City.Count;
			int maxPopulation = simulation.City.MaxPopulation;
			int ill = simulation.City.Count(cr => !cr.IsDead && cr.IsInfected);
			int totalIll = simulation.City.Count(cr => cr.WasIllAtSomePoint);
			int immune = simulation.City.Count(cr => !cr.IsDead && cr.IsImmune);
			int died = simulation.City.Count(cr => cr.IsDead);

			string info = String.Format("Population: {0}/{1}\nCurrent cases: {2}\nAffected population: {3}\nImmune: {4}\nDead: {5}\nTime elapsed: {6:0.0}",
				population, maxPopulation, ill, totalIll, immune, died, simulation.TotalTimeElapsed);
			uiTextRenderer.AddString(info, 14f, new Vector2(5 - 1, simulation.ScreenSize.Y - 16 - 1), Color4.Black);
			uiTextRenderer.AddString(info, 14f, new Vector2(5, simulation.ScreenSize.Y - 16), Color4.White);

			string fps = String.Format("FPS: {0:0.00}\nSpeed: {1:0.00}x", simulation.FPS, simulation.TimeScale);
			uiTextRenderer.AddString(fps, 14f, new Vector2(2 - 1, 16 - 1), Color4.Black);
			uiTextRenderer.AddString(fps, 14f, new Vector2(2, 16), Color4.White);
			GL.ClearColor(Color4.DarkBlue);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			if (simulation.IsIncreasingSpeed)
			{
				uiPanelRenderer.AddRightPolygon(new Vector2(20, 50), 16, 3, 0, Color4.Lime);
			}

			if (simulation.IsDecreasingSpeed)
			{
				uiPanelRenderer.AddRightPolygon(new Vector2(20, 50), 16, 3, (float)Math.PI, Color4.Red);
			}

			if (simulation.IsPaused)
			{
				uiTextRenderer.AddString("Paused", 20f, new Vector2(simulation.ScreenSize.X / 2 - 50, simulation.ScreenSize.Y - 20), Color4.Orange);
			}

			cityRenderer.DrawFilledElements();
			sectorBoundsRenderer.DrawHollowElements();
			haloRenderer.DrawTexture(ResourceManager.GetTexture("halo"));
			creatureRenderer.DrawFilledElements();
			sectorTextRenderer.DrawAll();
			selectionRenderer.DrawHollowElements();
			uiPanelRenderer.DrawFilledElements();
			uiTextRenderer.DrawAll();
		}
	}
}
