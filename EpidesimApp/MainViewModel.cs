using Epidesim.Simulation.Epidemic;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace Epidesim
{
	class MainViewModel : ViewModelBase
	{
		public MainViewModel()
		{
			SetupViewModel = new SetupViewModel(this);
			SimulationViewModel = new SimulationViewModel(this);
			StatisticsViewModel = new StatisticsViewModel(this);
		}

		public SetupViewModel SetupViewModel { get; set; }
		public SimulationViewModel SimulationViewModel { get; set; }
		public StatisticsViewModel StatisticsViewModel { get; set; }
	}
}