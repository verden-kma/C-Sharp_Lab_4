// using System;
// using System.Windows;
// using System.Windows.Input;
// using Lab_1.Tools.Managers;
//
// namespace Lab_1.ViewModels.Commands
// {
//     public class SetUpCustomer : ICommand
//     {
//         private ApplicationViewModel ViewModel { get; }
//
//         internal SetUpCustomer(ApplicationViewModel viewModel)
//         {
//             ViewModel = viewModel;
//         }
//
//         public bool CanExecute(object parameter)
//         {
//             MessageBox.Show("gfds");
//             return !string.IsNullOrWhiteSpace(ViewModel.Name) && !string.IsNullOrWhiteSpace(ViewModel.Surname) &&
//                    !string.IsNullOrWhiteSpace(ViewModel.Email);
//         }
//
//         public async void Execute(object parameter)
//         {
//             LoaderManager.Instance.ShowLoader();
//             string errorMessage = await ViewModel.SetCustomer();
//             LoaderManager.Instance.HideLoader();
//             if (errorMessage == null) ViewModel.CheckBirthday();
//             else ApplicationViewModel.ShowError(errorMessage);
//         }
//
//         public event EventHandler CanExecuteChanged;
//     }
// }