using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Lab_1.Annotations;
using Lab_1.Models;
using Lab_1.ViewModels.Commands;

namespace Lab_1.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        #region Fields

        private const string ChineseCap = "Your chinese zodiac:\n";
        private const string WesternCap = "Your western zodiac:\n";
        private const string AgeCap = "Your age:\n";
        private Person _customer;

        #endregion

        #region Props

        public string Age => _customer != null ? AgeCap + _customer.Age : "";

        public string Wz => _customer != null ? WesternCap + _customer.Wz : "";

        public string Cz => _customer != null ? ChineseCap + _customer.Cz : "";

        public DateTime VmBirthday { get; set; }

        public SetBirthdayCommand SetBirthdayCommand { get; set; }

        #endregion

        public ApplicationViewModel()
        {
            VmBirthday = DateTime.Today;
            SetBirthdayCommand = new SetBirthdayCommand(this);
        }

        public async Task<bool> SetBirthday()
        {
            return await Task.Run(() =>
            {
                try
                {
                    _customer = new Person(VmBirthday);
                    OnPropertyChanged(nameof(Age));
                    OnPropertyChanged(nameof(Wz));
                    OnPropertyChanged(nameof(Cz));
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            });
        }

        internal void CheckBirthday()
        {
            if (_customer.IsBirthday())
                MessageBox.Show(Person.GetGreeting());
        }

        public static void ShowError(string why)
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