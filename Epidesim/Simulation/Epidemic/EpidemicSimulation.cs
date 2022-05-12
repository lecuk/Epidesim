using Epidesim.Engine;
using Epidesim.Engine.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System;

namespace Epidesim.Simulation.Epidemic
{
	class EpidemicSimulation : ISimulation
	{
		public float Width { get; private set; }
		public float Height { get; private set; }

		public float Angle { get; private set; }

		public void Update(double deltaTime)
		{
			Angle += 0.7f * (float)deltaTime;
		}

		public EpidemicSimulation(float startAngle)
		{
			Angle = startAngle;
		}
	}
}