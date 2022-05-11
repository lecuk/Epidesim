using Epidesim.Engine;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidesim.Simulation
{
	class PolygonSimulation : ISimulation
	{
		public float Width { get; set; }
		public float Height { get; set; }

		public List<Polygon> Polygons { get; private set; }

		private double timeElapsed;
		private double timeLastPolygonAdded;

		private Random random;

		public PolygonSimulation(float width, float height)
		{
			Width = width;
			Height = height;
			Polygons = new List<Polygon>();
			random = new Random();
			timeElapsed = 0;
			timeLastPolygonAdded = 0;

			for (int i = 0; i < 100; ++i)
			{
				Polygon polygon = CreateRandomPolygon();
				Polygons.Add(polygon);
			}
		}

		private Color4[] colors = new Color4[] {
			Color4.Red, Color4.Purple, Color4.Green, Color4.Yellow, Color4.Magenta, Color4.Blue, Color4.Cyan, Color4.Orange
		};

		public void Update(double deltaTime)
		{
			timeElapsed += deltaTime;
			float fDeltaTime = (float)deltaTime;

			List<Polygon> toRemove = new List<Polygon>();

			foreach (Polygon polygon in Polygons)
			{
				polygon.Position += polygon.Speed * fDeltaTime * 0.05f;
				polygon.Rotation += polygon.RotationSpeed * fDeltaTime;
				
				if (Math.Abs(polygon.Position.X) > 1.5 || Math.Abs(polygon.Position.Y) > 1.5)
				{
					toRemove.Add(polygon);
				}
			}
			
			if (timeElapsed - timeLastPolygonAdded > 0.01)
			{
				for (int i = 0; i < 10; ++i)
				{
					//Polygons.Add(CreateSpawnedPolygon());
					timeLastPolygonAdded = timeElapsed;
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

			polygon.Position.X = (float)(random.NextDouble() * 1.5 - 0.75);
			polygon.Position.Y = (float)(random.NextDouble() * 1.5 - 0.75);
			polygon.Speed.X = (float)(0.18 - random.NextDouble() * 0.24);
			polygon.Speed.Y = -(float)(0.08 + random.NextDouble() * 0.32);
			polygon.Radius = (float)(0.01 + random.NextDouble() * 0.09);
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
			
			polygon.Position.X = -1.1f;
			polygon.Position.Y = 1.1f;

			bool top = random.Next() % 2 == 1;
			if (top)
			{
				polygon.Position.X = (float)(random.NextDouble() * 2.0 - 1.0);
			}
			else
			{
				polygon.Position.Y = (float)(random.NextDouble() * 2.0 - 1.0);
			}

			polygon.Speed.X = (float)(0.18 - random.NextDouble() * 0.24);
			polygon.Speed.Y = -(float)(0.08 + random.NextDouble() * 0.32);
			polygon.Radius = (float)(0.01 + random.NextDouble() * 0.09);
			polygon.Edges = 3 + random.Next() % 6;
			polygon.Rotation = (float)(random.NextDouble() * Math.PI);
			polygon.RotationSpeed = 0.4f + (float)random.NextDouble() * 5.0f;
			polygon.BorderColor = colors[random.Next() % colors.Length];
			polygon.FillColor = colors[random.Next() % colors.Length];
			polygon.BorderThickness = 0.5f + (float)random.NextDouble() * 2.5f;

			return polygon;
		}
	}
}
