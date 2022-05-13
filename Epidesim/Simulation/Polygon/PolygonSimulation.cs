using Epidesim.Engine;
using Epidesim.Engine.Drawing.Types;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Simulation.Polygon
{
	class PolygonSimulation : ISimulation
	{
		public Rectangle CameraRectangle { get; set; }

		public float ScreenWidth { get; set; }
		public float ScreenHeight { get; set; }

		public Vector2 TargetDirection { get; set; }
		public Vector2 ActualDirection { get; set; }
		public bool WireframeMode { get; set; }

		public List<Polygon> Polygons { get; private set; }

		public CoordinateSystem CurrentCoordinateSystem { get; set; }

		private double timeElapsed;
		private double timeLastPolygonAdded;

		private Random random;

		public PolygonSimulation(float width, float height)
		{
			CameraRectangle = Rectangle.FromCenterAndSize(new Vector2(), new Vector2(width, height));
			Polygons = new List<Polygon>();
			random = new Random();
			timeElapsed = 0;
			timeLastPolygonAdded = 0;
		}

		private Color4[] colors = new Color4[] {
			Color4.Red, Color4.Purple, Color4.Green, Color4.Yellow, Color4.Magenta, Color4.Blue, Color4.Cyan, Color4.Orange
		};

		public void Start()
		{
			TargetDirection = new Vector2();
			ActualDirection = new Vector2();

			for (int i = 0; i < 100; ++i)
			{
				Polygons.Add(CreateRandomPolygon());
			}
		}

		public void Update(double deltaTime)
		{
			CurrentCoordinateSystem = new CoordinateSystem()
			{
				ScreenWidth = ScreenWidth,
				ScreenHeight = ScreenHeight,
				ViewRectangle = CameraRectangle
			};

			timeElapsed += deltaTime;
			float fDeltaTime = (float)deltaTime;

			if (Input.IsKeyDown(OpenTK.Input.Key.Up))
			{
				CameraRectangle = CameraRectangle.Translate(new Vector2(0, -CameraRectangle.Height * fDeltaTime));
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Down))
			{
				CameraRectangle = CameraRectangle.Translate(new Vector2(0, CameraRectangle.Height * fDeltaTime));
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Left))
			{
				CameraRectangle = CameraRectangle.Translate(new Vector2(CameraRectangle.Width * fDeltaTime, 0));
			}

			if (Input.IsKeyDown(OpenTK.Input.Key.Right))
			{
				CameraRectangle = CameraRectangle.Translate(new Vector2(-CameraRectangle.Width * fDeltaTime, 0));
			}

			if (Input.IsMouseButtonDown(OpenTK.Input.MouseButton.Right))
			{
				CameraRectangle = CameraRectangle.Translate(TargetDirection);
			}

			if (Input.IsMouseButtonDown(OpenTK.Input.MouseButton.Left))
			{
				var position = Input.GetMouseLocalPosition();

				Polygon polygon = CreateSpawnedPolygon();
				polygon.Position = CurrentCoordinateSystem.ScreenCoordinateToWorldCoordinate(position);
				polygon.Speed.X = ActualDirection.X * 10;
				polygon.Speed.Y = ActualDirection.Y * 10;
				Polygons.Add(polygon);
			}

			if (Input.WasKeyJustPressed(OpenTK.Input.Key.W))
			{
				WireframeMode = !WireframeMode;
			}

			var mouseDelta = Input.GetMouseDelta();
			TargetDirection = 2 * mouseDelta * new Vector2(CameraRectangle.Width / ScreenWidth, CameraRectangle.Height / ScreenHeight);
			ActualDirection = ActualDirection + (TargetDirection - ActualDirection) * 6f * fDeltaTime;

			float wheelDelta = Input.GetMouseWheelDelta();
			if (wheelDelta > 0)
			{
				CameraRectangle = CameraRectangle.Scale(new Vector2(1 / 1.07f));
			}

			if (wheelDelta < 0)
			{
				CameraRectangle = CameraRectangle.Scale(new Vector2(1.07f));
			}

			var toRemove = new List<Polygon>();

			foreach (Polygon polygon in Polygons)
			{
				polygon.Position += polygon.Speed * fDeltaTime * 1.5f;
				polygon.Rotation += polygon.RotationSpeed * fDeltaTime;
				
				if (Math.Abs(polygon.Position.X) > 50000 || Math.Abs(polygon.Position.Y) > 50000)
				{
					toRemove.Add(polygon);
				}
			}
			
			if (Input.IsKeyDown(OpenTK.Input.Key.Space))
			{
				if (timeElapsed - timeLastPolygonAdded > 0.1)
				{
					for (int i = 0; i < 100; ++i)
					{
						Polygons.Add(CreateSpawnedPolygon());
						timeLastPolygonAdded = timeElapsed;
					}
				}
			}

			foreach (var polygon in toRemove)
			{
				Polygons.Remove(polygon);
			}
		}

		private Polygon CreateRandomPolygon()
		{
			Polygon polygon = new Polygon();

			polygon.Position.X = (float)(random.NextDouble() * 1000);
			polygon.Position.Y = (float)(random.NextDouble() * 1000);
			polygon.ZIndex = random.Next() % 1000;
			polygon.Speed.X = (float)(0);
			polygon.Speed.Y = -(float)(0);
			polygon.Radius = (float)(1 + random.NextDouble() * 39);
			polygon.Edges = 3 + random.Next() % 6;
			polygon.Rotation = (float)(random.NextDouble() * Math.PI);
			polygon.RotationSpeed = 0.4f + (float)random.NextDouble() * 5.0f;
			polygon.BorderColor = colors[random.Next() % colors.Length];
			polygon.FillColor = colors[random.Next() % colors.Length];
			polygon.BorderThickness = 0.5f + (float)random.NextDouble() * 2.5f;

			return polygon;
		}

		private Polygon CreateSpawnedPolygon()
		{
			Polygon polygon = new Polygon();

			polygon.Position.X = (float)(random.NextDouble() * 1000);
			polygon.Position.Y = (float)(random.NextDouble() * 1000);
			polygon.ZIndex = random.Next() % 1000;
			polygon.Speed.X = (float)(18 - random.NextDouble() * 24);
			polygon.Speed.Y = -(float)(8 + random.NextDouble() * 32);
			polygon.Radius = (float)(5 + random.NextDouble() * 15);
			polygon.Edges = 3 + random.Next() % 7;
			polygon.Rotation = (float)(random.NextDouble() * Math.PI);
			polygon.RotationSpeed = 0.4f + (float)random.NextDouble() * 5.0f;
			polygon.BorderColor = colors[random.Next() % colors.Length];
			polygon.FillColor = colors[random.Next() % colors.Length];
			polygon.BorderThickness = 0.5f + (float)random.NextDouble() * 2.5f;

			return polygon;
		}
	}
}
