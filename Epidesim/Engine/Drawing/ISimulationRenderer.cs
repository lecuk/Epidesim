namespace Epidesim.Engine.Drawing
{
	interface ISimulationRenderer<T> where T : ISimulation
	{
		void Render(T simulation);
	}
}
