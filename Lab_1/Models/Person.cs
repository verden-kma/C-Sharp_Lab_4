using System;
using System.ComponentModel;

using System.Text.RegularExpressions;
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
        private PersonData _backup;
        private PersonData _curr;

        internal PersonData PersonExtract => _curr;

        #endregion

        #region Props

        public uint Id => _id;

        public string Name
        {
            get => _curr.Name;
            set
            {
                if (string.IsNullOrWhiteSpace(value) ||
                    value.Length < 2 || value.Length > 20) throw new IrrelevantName(value);
                _curr.Name = value.Trim();
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
                _curr.Birthday = value;
                IsAdult = proposedAge >= 18;
                IsBirthday = Birthday.Date.Month.Equals(DateTime.Today.Month) &&
                             Birthday.Date.Day.Equals(DateTime.Today.Day);
                SunSign = Zodiac.GetWestZodiac(Birthday);
                ChineseSign = Zodiac.GetChineseZodiac(Birthday);
            }
        }

        public int Age { get; set; }

        public bool IsAdult { get; set; }
        public Zodiac.WesternZodiac SunSign { get; set; }

        public Zodiac.ChineseZodiac ChineseSign { get; set; }

        public bool IsBirthday { get; set; }

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

        internal Person(string name = "Apple", string surname = "Vovan", string email = PersonData.DefaultEmail,
            DateTime? birthday = null) :
            this(birthday ?? PersonData.DefaultBirthday)
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void BeginEdit()
        {
            if (_isEditing) return;
            _isEditing = true;
            _backup = _curr;
        }

        public void EndEdit()
        {
            _backup = new PersonData();
            _isEditing = false;
        }

        public void CancelEdit()
        {
            if (!_isEditing) return;
            _curr = _backup;
            _isEditing = false;
        }

        public override string ToString()
        {
            return $"ID: {_id}\nName: {_curr.Name} Birthday: \n{Birthday}";
        }
    }
}