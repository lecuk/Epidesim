using OpenTK.Wpf;
using System.Windows.Media;

namespace Epidesim
{
	class EpidesimGLControl : GLWpfControl
	{
		private MainViewModel ViewModel => DataContext as MainViewModel;

		public EpidesimGLControl()
		{
			Render += EpidesimGLControl_Render;
		}

		private void EpidesimGLControl_Render(System.TimeSpan obj)
		{
			if (ViewModel.IsInitialized)
			{
				Input.Refresh();
				ViewModel.Simulation.Update(0.016);
				ViewModel.SimulationRenderer.Render(ViewModel.Simulation);
			}
		}
	}
}
