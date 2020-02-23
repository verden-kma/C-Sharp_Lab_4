using System;
using System.Windows.Input;

namespace Lab_1.ViewModels.Commands
{
    public class SetBirthdayCommand : ICommand
    {
        private ApplicationViewModel ViewModel { get; }

        public SetBirthdayCommand(ApplicationViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var validCustomer = await ViewModel.SetBirthday();
            if (validCustomer) ViewModel.CheckBirthday();
            else ApplicationViewModel.ShowError("Incorrect date!");
        }

        public event EventHandler CanExecuteChanged;
    }
}