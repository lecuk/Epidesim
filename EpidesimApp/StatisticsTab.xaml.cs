using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Timers;
using System.Windows.Controls;

namespace Epidesim
{
	/// <summary>
	/// Interaction logic for StatisticsTab.xaml
	/// </summary>
	public partial class StatisticsTab : UserControl
	{
		private StatisticsViewModel ViewModel => DataContext as StatisticsViewModel;

		private Timer updateTimer;

		public StatisticsTab()
		{
			InitializeComponent();
		}

		private PlotModel CreateModel()
		{
			var model = new PlotModel();

			model.Axes.Add(new LinearAxis()
			{
				Position = AxisPosition.Bottom,
				MinimumPadding = 0,
				MaximumPadding = 0
			});
			model.Axes.Add(new LinearAxis()
			{
				Position = AxisPosition.Left,
				Minimum = 0,
				MinimumPadding = 0,
			});

			return model;
		}

		private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (IsVisible)
			{
				Dispatcher.Invoke(() =>
				{
					CasesPerDayGraph.Model.InvalidatePlot(updateData: true);
					CasesPerDayGraph.Model.ResetAllAxes();

					TotalCasesGraph.Model.InvalidatePlot(updateData: true);
					TotalCasesGraph.Model.ResetAllAxes();
				});
			}
		}

		private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			Loaded -= UserControl_Loaded; // cringe crutch but load event fires moere than one for some reason
			
			CasesPerDayGraph.Model = CreateModel();
			TotalCasesGraph.Model = CreateModel();

			CasesPerDayGraph.Model.Series.Add(ViewModel.NewCasesGraph);
			CasesPerDayGraph.Model.Series.Add(ViewModel.NewConfirmedCasesGraph);
			CasesPerDayGraph.Model.Series.Add(ViewModel.NewDeathsGraph);

			TotalCasesGraph.Model.Series.Add(ViewModel.TotalImmuneGraph);
			TotalCasesGraph.Model.Series.Add(ViewModel.TotalCasesGraph);
			TotalCasesGraph.Model.Series.Add(ViewModel.TotalDeathsGraph);

			updateTimer = new Timer(1000.0);
			updateTimer.Elapsed += UpdateTimer_Elapsed;
			updateTimer.Start();
		}

		private void UserControl_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			if (IsVisible)
			{
				Dispatcher.Invoke(() =>
				{
					CasesPerDayGraph.Model.InvalidatePlot(updateData: true);
					CasesPerDayGraph.Model.ResetAllAxes();

					TotalCasesGraph.Model.InvalidatePlot(updateData: true);
					TotalCasesGraph.Model.ResetAllAxes();
				});
			}
		}
	}
}
