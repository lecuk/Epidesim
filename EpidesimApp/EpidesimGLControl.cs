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
			SizeChanged += EpidesimGLControl_SizeChanged;
		}

		private void EpidesimGLControl_Render(System.TimeSpan timeSpan)
		{
			ViewModel.Update.Execute(timeSpan);
		}

		private void EpidesimGLControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		{
			if (ViewModel.IsInitialized)
			{
				ViewModel.Simulation.SetScreenSize((float)ActualWidth, (float)ActualHeight);
			}
		}
	}
}
