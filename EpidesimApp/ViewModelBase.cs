using System.ComponentModel;

namespace Epidesim
{
	class ViewModelBase : INotifyPropertyChanged
	{
		public bool IsInDesignMode { get; protected set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}