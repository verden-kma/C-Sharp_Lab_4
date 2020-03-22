using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Lab_1.Annotations;
using Lab_1.Models.Exceptions;

namespace Lab_1.Models
{
    internal class Person : INotifyPropertyChanged, IEditableObject
    {
        #region Fields

        private const string EmailPattern = "^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$";
        private readonly Regex _emailRegex = new Regex(EmailPattern);
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
                    value.Length < 2 || value.Length > 20) throw new IrrelevantName(value);
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
                    throw new IrrelevantName(value);
                _curr.Surname = value.Trim();
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _curr.Email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !_emailRegex.IsMatch(value = value.Trim()))
                    throw new IncorrectEmail(value, EmailPattern);
                _curr.Email = value;
                OnPropertyChanged();
            }
        }

        public DateTime Birthday
        {
            get => _curr.Birthday.Date;
            set
            {
                int proposedAge = CalcAge(value);
                if (!AgeIsValid(proposedAge, value))
                    throw new IncorrectBirthday(value.Date, MinAge, MaxAge);
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

        private int CalcAge(DateTime d)
        {
            int age = DateTime.Today.Year - d.Year;
            if (d.Date > d.AddYears(-age).Date) --age;
            return age;
        }

        private bool AgeIsValid(int age, DateTime birthday)
        {
            return !(age > MaxAge || age < MinAge || birthday.Date > DateTime.Today.Date);
        }

        internal Person(string name = "Apple", string surname = "Vovan", string email = PersonCoreData.DefaultEmail,
            DateTime? birthday = null) :
            this(birthday ?? PersonCoreData.DefaultBirthday)
        {
            Name = name;
            Surname = surname;
            Email = email;
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
        }

        public void EndEdit()
        {
            _backup = new PersonCoreData();
            _isEditing = false;
        }

        public void CancelEdit()
        {
            if (!_isEditing) return;
            _curr = _backup;
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