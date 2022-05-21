namespace Epidesim.Engine.Drawing
{
	public interface ISimulationRenderer<T> where T : ISimulation
	{
		void Render(T simulation);
	}
}
