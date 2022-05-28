using OpenTK.Wpf;
using System.Windows.Media;

namespace Epidesim
{
	class EpidesimGLControl : GLWpfControl
	{
		private SimulationViewModel ViewModel => DataContext as SimulationViewModel;

		public EpidesimGLControl()
		{
			Render += EpidesimGLControl_Render;
			SizeChanged += EpidesimGLControl_SizeChanged;
			IsVisibleChanged += EpidesimGLControl_IsVisibleChanged;
		}

		private void EpidesimGLControl_Render(System.TimeSpan timeSpan)
		{
			ViewModel.Update.Execute(timeSpan);
		}

		private void EpidesimGLControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		{
			if (Width != 0 && Height != 0)
			{
				if (ViewModel.IsInitialized)
				{
					ViewModel.Simulation.SetScreenSize((float)ActualWidth, (float)ActualHeight);
				}
			}
		}

		private void EpidesimGLControl_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (IsVisible && Width != 0 && Height != 0)
			{
				if (ViewModel.IsInitialized)
				{
					ViewModel.Simulation.SetScreenSize((float)ActualWidth, (float)ActualHeight);
				}
			}
		}
	}
}
