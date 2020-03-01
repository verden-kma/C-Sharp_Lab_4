using System;
using System.Text.RegularExpressions;

namespace Lab_1.Models
{
    public class Person
    {
        #region Fields

        private readonly Regex _emailRegex =
            new Regex("^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$");

        private string _name;
        private string _surname;
        private string _email;
        private const string DefaultEmail = "yablonskyivr@ukma.edu.ua";
        private static readonly DateTime DefaultDate = new DateTime(1995, 5, 12);

        #endregion

        #region Props

        private string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Name is empty.");
                if (value.Length < 2 || value.Length > 20)
                    throw new ArgumentException("Name length must be in range [2;20] chars");
                _name = value;
            }
        }

        private string Surname
        {
            get => _surname;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Surname is empty.");
                if (value.Length < 2 || value.Length > 20)
                    throw new ArgumentException("Surname length must be in range [2;20] chars.");
                _surname = value;
            }
        }

        private string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Email is empty.");
                if (!_emailRegex.IsMatch(value)) throw new ArgumentException("Invalid email address.");
                _email = value;
            }
        }

        private DateTime Birthday { get; }
        public int Age { get; }

        internal bool IsAdult => Age >= 18;
        internal Zodiac.WesternZodiac SunSign { get; }

        internal Zodiac.ChineseZodiac ChineseSign { get; }

        public bool IsBirthday => Birthday.Date.Month.Equals(DateTime.Today.Month) &&
                                  Birthday.Date.Day.Equals(DateTime.Today.Day);

        #endregion

        #region Constructors

        private Person(DateTime birthday)
        {
            Birthday = birthday;
            DateTime today = DateTime.Today;
            Age = today.Year - birthday.Year;
            if (Birthday.Date > today.AddYears(-Age).Date)
                --Age;
            if (Age > 135 || Age < 0 || birthday.Date > today.Date)
                throw new ArgumentException("incorrect birthday date");
            Birthday = birthday;
            SunSign = Zodiac.GetWestZodiac(birthday);
            ChineseSign = Zodiac.GetChineseZodiac(birthday);
        }

        internal Person(string name, string surname, string email = DefaultEmail, DateTime? birthday = null) : 
            this(birthday ?? DefaultDate)
        {
            Name = name;
            Surname = surname;
            Email = email;
        }

        private Person(string name, string surname, string email) : this(name, surname, email, null)
        {
        }

        private Person(string name, string surname, DateTime birthday) : this(name, surname, DefaultEmail, birthday)
        {
        }

        #endregion

        public string GetGreeting()
        {
            return $"Happy birthday, {Name}!";
        }
    }
}