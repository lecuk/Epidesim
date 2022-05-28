using Epidesim.Simulation.Epidemic;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace Epidesim
{
	class SimulationViewModel : ViewModelBase
	{
		public SimulationViewModel(MainViewModel parent)
		{
			ParentViewModel = parent;
		}

		public MainViewModel ParentViewModel { get; set; }

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

		public bool IsRunning
		{
			get => (Simulation != null) && !Simulation.IsPaused;
			set
			{
				Simulation.IsPaused = !value;
				RaisePropertyChanged(nameof(IsRunning));
			}
		}

		public string StartStopString => IsRunning ? "Stop" : "Start";
		public Brush StartStopColor => IsRunning ? Brushes.Yellow : Brushes.Lime;

		public ICommand StartStop => new DelegateCommand((param) =>
		{
			IsRunning = !IsRunning;
		});

		public ICommand Setup => new DelegateCommand((param) =>
		{
			IsRunning = false;
			IsInitialized = true;
			Simulation.Start();
		});

		public ICommand Update => new DelegateCommand((param) =>
		{
			var timeSpan = (TimeSpan)param;

			Input.Refresh();
			Simulation.Update((float)timeSpan.TotalSeconds);
			SimulationRenderer.Render(Simulation);
		});
	}
}
