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
        private Person _customer;

        public DateTime VmBirthday { get; set; }

        public SetBirthdayCommand SetBirthdayCommand { get; set; }

        public ApplicationViewModel()
        {
            SetBirthdayCommand = new SetBirthdayCommand(this);
        }

//                23-Feb-00
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Age => _customer != null ? _customer.Age.ToString() : "";

        public string Wz => _customer != null ? _customer.Wz.ToString() : "";

        public string Cz => _customer != null ? _customer.Cz.ToString() : "";

        public static void ShowError(string why)
        {
            MessageBox.Show(why);
        }
    }
}