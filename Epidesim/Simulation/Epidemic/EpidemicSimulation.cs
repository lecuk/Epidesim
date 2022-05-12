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

		public void Start()
		{
		}

		public void Update(double deltaTime)
		{
		}

		public EpidemicSimulation()
		{
		}
	}
}