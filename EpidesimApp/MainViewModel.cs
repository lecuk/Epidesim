using Epidesim.Simulation.Epidemic;

namespace Epidesim
{
	class MainViewModel : ViewModelBase
	{
		public MainViewModel()
		{
			IsInitialized = false;
		}

		private EpidemicSimulation _simulation;
		public EpidemicSimulation Simulation
		{
			get => _simulation;
			set
			{
				_simulation = value;
				RaisePropertyChanged(nameof(Simulation));
			}
		}

		private EpidemicSimulationRenderer _simulationRenderer;
		public EpidemicSimulationRenderer SimulationRenderer
		{
			get => _simulationRenderer;
			set
			{
				_simulationRenderer = value;
				RaisePropertyChanged(nameof(SimulationRenderer));
			}
		}

		private bool _isInitialized;
		public bool IsInitialized
		{
			get => _isInitialized;
			set
			{
				_isInitialized = value;
				RaisePropertyChanged(nameof(IsInitialized));
			}
		}
	}
}