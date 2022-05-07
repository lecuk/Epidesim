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
			using (var window = new MainWindow(800, 450, "hello world!"))
			{
				window.Run(60.0);
			}
		}
	}
}
