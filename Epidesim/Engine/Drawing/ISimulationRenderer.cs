namespace Epidesim.Engine.Drawing
{
	interface ISimulationRenderer<T> where T : ISimulation
	{
		float ScreenWidth { get; set; }
		float ScreenHeight { get; set; }

		void Render(T simulation);
	}
}
