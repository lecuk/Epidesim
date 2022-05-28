using Epidesim.Simulation.Epidemic;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace Epidesim
{
	class SetupViewModel : ViewModelBase
	{
		public SetupViewModel(MainViewModel parent)
		{
			var random = new Random();

			ParentViewModel = parent;
			CityBlueprint = CityBlueprint.Default(random);
			Illness = Illness.Default(random);
			CreatureBehaviour = CreatureBehaviour.Default(random);
		}

		public MainViewModel ParentViewModel { get; set; }
		public CityBlueprint CityBlueprint { get; set; }
		public Illness Illness { get; set; }
		public CreatureBehaviour CreatureBehaviour { get; set; }

		public ICommand ReloadSimulation => new DelegateCommand((param) =>
		{
			var simulation = ParentViewModel.SimulationViewModel.Simulation;

			simulation.CityBlueprint = CityBlueprint;
			simulation.Illness = Illness;
			simulation.CreatureBehaviour = CreatureBehaviour;
			simulation.Start();
		});
	}
}
