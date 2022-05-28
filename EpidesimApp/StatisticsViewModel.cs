using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using Epidesim.Simulation.Epidemic;
using OxyPlot;
using OxyPlot.Series;

namespace Epidesim
{
	class StatisticsViewModel : ViewModelBase
	{
		public MainViewModel ParentViewModel { get; set; }

		public DataPointSeries TotalCasesGraph { get; set; }
		public DataPointSeries TotalDeathsGraph { get; set; }
		public DataPointSeries TotalImmuneGraph { get; set; }
		public DataPointSeries NewCasesGraph { get; set; }
		public DataPointSeries NewConfirmedCasesGraph { get; set; }
		public DataPointSeries NewDeathsGraph { get; set; }

		public int Population { get; set; }

		private Timer updateTimer;
		
		public StatisticsViewModel(MainViewModel parentViewModel)
		{
			ParentViewModel = parentViewModel;

			TotalCasesGraph = new AreaSeries()
			{
				Color = OxyColors.Pink,
				Fill = OxyColors.Pink
			};
			TotalDeathsGraph = new AreaSeries()
			{
				Color = OxyColors.Black,
				Fill = OxyColors.Black
			};
			TotalImmuneGraph = new AreaSeries()
			{
				Color = OxyColors.SkyBlue,
				Fill = OxyColors.SkyBlue
			};
			NewCasesGraph = new AreaSeries()
			{
				Color = OxyColors.Pink,
				Fill = OxyColors.Pink
			};
			NewConfirmedCasesGraph = new AreaSeries()
			{
				Color = OxyColors.Red,
				Fill = OxyColors.Red
			};
			NewDeathsGraph = new AreaSeries()
			{
				Color = OxyColors.Black,
				Fill = OxyColors.Black
			};

			TotalCasesGraph.Points.Add(new DataPoint(0, 0));
			TotalDeathsGraph.Points.Add(new DataPoint(0, 0));
			TotalImmuneGraph.Points.Add(new DataPoint(0, 0));

			updateTimer = new Timer(500.0);
			updateTimer.Elapsed += UpdateTimer_Elapsed;
			updateTimer.Start(); 
		}

		private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (ParentViewModel.SimulationViewModel.IsInitialized)
			{
				var simulation = ParentViewModel.SimulationViewModel.Simulation;
				var simulationStats = simulation.Stats;

				UpdateSimultionStats(simulationStats);
			}
		}

		public void UpdateSimultionStats(SimulationStats stats)
		{
			int currentPeriodCount = TotalCasesGraph.Points.Count;
			if (currentPeriodCount < stats.Periods.Count)
			{
				for (int i = currentPeriodCount; i < stats.Periods.Count; ++i)
				{
					var period = stats.Periods[i];

					TotalCasesGraph.Points.Add(new DataPoint(period.EndTime, period.TotalInfections));
					TotalDeathsGraph.Points.Add(new DataPoint(period.EndTime, period.TotalDeaths));
					TotalImmuneGraph.Points.Add(new DataPoint(period.EndTime, period.TotalImmune));

					NewCasesGraph.Points.Add(new DataPoint(period.StartTime, period.NewInfections));
					NewCasesGraph.Points.Add(new DataPoint(period.EndTime, period.NewInfections));
					NewConfirmedCasesGraph.Points.Add(new DataPoint(period.StartTime, period.NewConfirmedCases));
					NewConfirmedCasesGraph.Points.Add(new DataPoint(period.EndTime, period.NewConfirmedCases));
					NewDeathsGraph.Points.Add(new DataPoint(period.StartTime, period.NewDeaths));
					NewDeathsGraph.Points.Add(new DataPoint(period.EndTime, period.NewDeaths));
				}
			}
			else if (stats.Periods.Count == 0)
			{
				TotalCasesGraph.Points.Clear();
				TotalDeathsGraph.Points.Clear();
				TotalImmuneGraph.Points.Clear();
				NewCasesGraph.Points.Clear();
				NewConfirmedCasesGraph.Points.Clear();
				NewDeathsGraph.Points.Clear();
			}
		}
	}
}
