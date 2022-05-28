using Epidesim;
using OpenTK.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Epidesim
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainViewModel ViewModel => DataContext as MainViewModel;

		public MainWindow()
		{
			InitializeComponent();

			Input.Origin = GLControl;

			GLControl.Start(new GLWpfControlSettings()
			{
				MajorVersion = 3,
				MinorVersion = 0,
				RenderContinuously = true
			});

			ResourceInitializer.Init();

			ViewModel.SimulationViewModel.Simulation = new Simulation.Epidemic.EpidemicSimulation();
			ViewModel.SimulationViewModel.SimulationRenderer = new Simulation.Epidemic.EpidemicSimulationRenderer();
			ViewModel.SimulationViewModel.Simulation.Start();
			ViewModel.SimulationViewModel.IsInitialized = true;
		}
	}
}
