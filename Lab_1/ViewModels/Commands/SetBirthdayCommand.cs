using System;
using System.Windows.Input;
using Lab_1.Tools.Managers;

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
            LoaderManager.Instance.ShowLoader();
            var validCustomer = await ViewModel.SetBirthday();
            if (validCustomer) ViewModel.CheckBirthday();
            else ApplicationViewModel.ShowError("Incorrect date!");
            LoaderManager.Instance.HideLoader();
        }

        public event EventHandler CanExecuteChanged;
    }
}