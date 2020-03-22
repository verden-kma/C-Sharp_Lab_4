using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using Lab_1.Annotations;
using Lab_1.Models.Exceptions;

namespace Lab_1.Models
{
    internal class Person : INotifyPropertyChanged, IEditableObject
    {
        #region Fields

        private const string EmailPattern = "^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$";
        private static readonly Regex EmailRegex = new Regex(EmailPattern);
        private static uint _freeId;

        private readonly uint _id;
        private const uint MinAge = 0;
        private const uint MaxAge = 135;

        private bool _isEditing;
        private PersonCoreData _backup;
        private PersonCoreData _curr;

        internal PersonCoreData PersonCoreExtract => _curr;
        private PersonCalcData _backupCalc;
        private PersonCalcData _currCalc;

        #endregion

        private struct PersonCalcData
        {
            internal int Age;
            internal bool IsAdult;
            internal bool IsBirthday;
            internal Zodiac.WesternZodiac SunSign;
            internal Zodiac.ChineseZodiac ChineseSign;
        }

        #region Props

        public string Name
        {
            get => _curr.Name;
            set
            {
                if (string.IsNullOrWhiteSpace(value) ||
                    value.Length < 2 || value.Length > 20)
                {
                    OnPropertyChanged();
                    MessageBox.Show($"incorrect Name: \"{value}\"");
                    throw new IrrelevantName(value);
                }

                _curr.Name = value.Trim();
                OnPropertyChanged();
            }
        }

        public string Surname
        {
            get => _curr.Surname;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 20)
                {
                    OnPropertyChanged();
                    MessageBox.Show($"incorrect Surname: \"{value}\"");
                    throw new IrrelevantName(value);
                }

                _curr.Surname = value.Trim();
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _curr.Email;
            set
            {
                if (!EmailIsValid(value = value.Trim()))
                {
                    OnPropertyChanged();
                    MessageBox.Show($"Invalid email address: {value}");
                    throw new IncorrectEmail(value, EmailPattern);
                }
                _curr.Email = value;
                OnPropertyChanged();
            }
        }

        internal static bool EmailIsValid(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email);
        }

        public DateTime Birthday
        {
            get => _curr.Birthday.Date;
            private set
            {
                int proposedAge = CalcAge(value);
                if (!AgeIsValid(proposedAge, value))
                {
                    MessageBox.Show($"Invalid birthday: {value.Date.ToLongDateString()}, Age = {proposedAge}.");
                    throw new IncorrectBirthday(value.Date, MinAge, MaxAge);
                }

                Age = proposedAge;
                _curr.Birthday = value;
                OnPropertyChanged();
                IsAdult = proposedAge >= 18;
                IsBirthday = Birthday.Date.Month.Equals(DateTime.Today.Month) &&
                             Birthday.Date.Day.Equals(DateTime.Today.Day);
                SunSign = Zodiac.GetWestZodiac(Birthday);
                ChineseSign = Zodiac.GetChineseZodiac(Birthday);
            }
        }

        public int Age
        {
            get => _currCalc.Age;
            private set
            {
                _currCalc.Age = value;
                OnPropertyChanged();
            }
        }

        public bool IsAdult
        {
            get => _currCalc.IsAdult;
            private set
            {
                _currCalc.IsAdult = value;
                OnPropertyChanged();
            }
        }

        public bool IsBirthday
        {
            get => _currCalc.IsBirthday;
            private set
            {
                _currCalc.IsBirthday = value;
                OnPropertyChanged();
            }
        }

        public Zodiac.WesternZodiac SunSign
        {
            get => _currCalc.SunSign;
            private set
            {
                _currCalc.SunSign = value;
                OnPropertyChanged();
            }
        }

        public Zodiac.ChineseZodiac ChineseSign
        {
            get => _currCalc.ChineseSign;
            private set
            {
                _currCalc.ChineseSign = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructors

        internal Person(string name = "Apple", string surname = "Vowan", string email = PersonCoreData.DefaultEmail,
            DateTime? birthday = null) :
            this(birthday ?? PersonCoreData.DefaultBirthday)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
                throw new ArgumentException("Name and surname must not be empty.");
            if (!EmailIsValid(email)) throw new IncorrectEmail(email, EmailPattern);
            Name = name;
            Surname = surname;
            Email = email;
        }

        private Person(DateTime birthday)
        {
            _id = _freeId++;
            Birthday = birthday;
            Age = CalcAge(Birthday);
            AgeIsValid(Age, birthday);
            if (!AgeIsValid(Age, birthday))
                throw new IncorrectBirthday(birthday.Date, MinAge, MaxAge);
            IsAdult = Age >= 18;
            IsBirthday = Birthday.Date.Month.Equals(DateTime.Today.Month) &&
                         Birthday.Date.Day.Equals(DateTime.Today.Day);
            Birthday = birthday;
            SunSign = Zodiac.GetWestZodiac(birthday);
            ChineseSign = Zodiac.GetChineseZodiac(birthday);
        }

        private static int CalcAge(DateTime d)
        {
            int age = DateTime.Today.Year - d.Year;
            if (d.Date > d.AddYears(-age).Date) --age;
            return age;
        }

        private static bool AgeIsValid(int age, DateTime birthday)
        {
            return age <= MaxAge && age >= MinAge && birthday.Date <= DateTime.Today.Date;
        }

        internal static bool IsAlive(DateTime birthday)
        {
            return AgeIsValid(CalcAge(birthday), birthday);
        }

        #endregion

        internal string GetGreeting()
        {
            return $"Happy birthday, {Name} {Surname}!\nCheckout your mail {Email} :)";
        }

        public override string ToString()
        {
            return $"ID: {_id}\nName: {_curr.Name} Birthday: \n{Birthday}";
        }

        #region IEditableImplementation

        public void BeginEdit()
        {
            if (_isEditing) return;
            _isEditing = true;
            _backup = _curr;
            _backupCalc = _currCalc;
        }

        public void EndEdit()
        {
            _backup = new PersonCoreData();
            _backupCalc = new PersonCalcData();
            _isEditing = false;
        }

        public void CancelEdit()
        {
            if (!_isEditing) return;
            _curr = _backup;
            _currCalc = _backupCalc;
            _isEditing = false;
        }

        #endregion

        #region INotify

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}