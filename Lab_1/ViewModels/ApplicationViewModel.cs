using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Lab_1.Annotations;
using Lab_1.Models;
using Lab_1.Tools.Managers;
using Lab_1.Tools.MVVM;

namespace Lab_1.ViewModels
{
    internal class ApplicationViewModel : INotifyPropertyChanged
    {
        #region Fields

        private const string ChineseCap = "Your chinese zodiac:\n";
        private const string WesternCap = "Your western zodiac:\n";
        private const string AdultCap = "You are adult:\n";
        private const string BirthdayCap = "It is your birthday today:\n";
        private Person _customer;
        private string _name;
        private string _surname;
        private string _email;
        private RelayCommand<object> _proceedCommand;

        #endregion

        #region Props

        public RelayCommand<object> ProceedCommand
        {
            get
            {
                return _proceedCommand ?? (_proceedCommand = new RelayCommand<object>
                    (o => ProceedCommandImpl(), o => CanProceed()));
            }
        }

        private bool CanProceed()
        {
            return !string.IsNullOrWhiteSpace(_name) && !string.IsNullOrWhiteSpace(_surname) &&
                   !string.IsNullOrWhiteSpace(_email);
        }

        private async void ProceedCommandImpl()
        {
            LoaderManager.Instance.ShowLoader();
            string errorMessage = await SetCustomer();
            LoaderManager.Instance.HideLoader();
            if (string.IsNullOrEmpty(errorMessage)) CheckBirthday();
            else ShowError(errorMessage);
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                if (string.IsNullOrWhiteSpace(_name)) CanProceed();
                OnPropertyChanged();
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                if (string.IsNullOrWhiteSpace(_surname)) CanProceed();
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                if (string.IsNullOrWhiteSpace(_email)) CanProceed();
                OnPropertyChanged();
            }
        }

        public string IsBirthday => _customer != null ? BirthdayCap + _customer.IsBirthday : BirthdayCap;
        public string IsAdult => _customer != null ? AdultCap + _customer.IsAdult : AdultCap;
        public string Wz => _customer != null ? WesternCap + _customer.SunSign : WesternCap;
        public string Cz => _customer != null ? ChineseCap + _customer.ChineseSign : ChineseCap;

        public DateTime VmBirthday { get; set; }

        #endregion

        internal ApplicationViewModel()
        {
            // init default
            VmBirthday = DateTime.Today;
            OnPropertyChanged();
        }

        private async Task<string> SetCustomer()
        {
            return await Task.Run(() =>
            {
                try
                {
                    _customer = new Person(Name, Surname, Email, VmBirthday);
                    OnPropertyChanged(nameof(IsBirthday));
                    OnPropertyChanged(nameof(IsAdult));
                    OnPropertyChanged(nameof(Wz));
                    OnPropertyChanged(nameof(Cz));
                    return null;
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            });
        }

        private void CheckBirthday()
        {
            if (_customer.IsBirthday) MessageBox.Show(_customer.GetGreeting());
        }

        private static void ShowError(string why)
        {
            MessageBox.Show(why);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}