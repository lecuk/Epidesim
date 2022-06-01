using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
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
		
		public StatisticsTab()
		{
			InitializeComponent();
		}

		private PlotModel CreateModel(string title)
		{
			var model = new PlotModel()
			{
				Title = title,
				TitleFontSize = this.FontSize
			};

			model.Axes.Add(new LinearAxis()
			{
				Position = AxisPosition.Bottom,
				MinimumPadding = 0,
				MaximumPadding = 0,
				AbsoluteMinimum = 0
			});
			model.Axes.Add(new LinearAxis()
			{
				Position = AxisPosition.Left,
				Minimum = 0,
				MinimumPadding = 0,
				AbsoluteMinimum = 0
			});

			return model;
		}

		private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			Loaded -= UserControl_Loaded; // cringe crutch but load event fires moere than one for some reason
			
			CasesPerDayGraph.Model = CreateModel("Cases per day");
			TotalCasesGraph.Model = CreateModel("Total cases");

			CasesPerDayGraph.Model.Series.Add(ViewModel.NewCasesGraph);
			CasesPerDayGraph.Model.Series.Add(ViewModel.NewConfirmedCasesGraph);
			CasesPerDayGraph.Model.Series.Add(ViewModel.NewDeathsGraph);

			TotalCasesGraph.Model.Series.Add(ViewModel.TotalImmuneGraph);
			TotalCasesGraph.Model.Series.Add(ViewModel.TotalCasesGraph);
			TotalCasesGraph.Model.Series.Add(ViewModel.TotalDeathsGraph);
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
