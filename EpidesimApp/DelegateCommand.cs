using System;
using System.Windows.Input;

namespace Epidesim
{
	internal class DelegateCommand : ICommand
	{
		private Action<object> command;

		public DelegateCommand(Action<object> command)
		{
			this.command = command;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			command(parameter);
		}
	}
}