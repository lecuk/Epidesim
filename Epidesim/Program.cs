using System;
using OpenTK;
using Epidesim.Engine;
using Epidesim.Simulation;
using System.Diagnostics;

namespace Epidesim
{
	static class Program
	{
		static void Main()
		{
			using (var window = MainWindow.Get())
			{
				window.Run(60.0);
			}
		}
	}
}
