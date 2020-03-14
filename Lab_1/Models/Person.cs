using System;
using System.Text.RegularExpressions;
using System.Threading;
using Lab_1.Models.Exceptions;

namespace Lab_1.Models
{
    internal class Person
    {
        #region Fields

        private const string EmailPattern = "^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$";
        private readonly Regex _emailRegex = new Regex(EmailPattern);

        private const uint MinAge = 0;
        private const uint MaxAge = 135;
        
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
                if (string.IsNullOrWhiteSpace(value) ||
                    value.Length < 2 || value.Length > 20) throw new IrrelevantName(value);
                _name = value.Trim();
            }
        }

        private string Surname
        {
            get => _surname;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 20) throw new IrrelevantName(value);
                _surname = value.Trim();
            }
        }

        private string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !_emailRegex.IsMatch(value)) throw new IncorrectEmail(value, EmailPattern);
                _email = value.Trim();
            }
        }

        private DateTime Birthday { get; }
        private int Age { get; }

        internal bool IsAdult => Age >= 18;
        internal Zodiac.WesternZodiac SunSign { get; }

        internal Zodiac.ChineseZodiac ChineseSign { get; }

        internal bool IsBirthday => Birthday.Date.Month.Equals(DateTime.Today.Month) &&
                                  Birthday.Date.Day.Equals(DateTime.Today.Day);

        #endregion

        #region Constructors

        private Person(DateTime birthday)
        {
            Thread.Sleep(2000);
            Birthday = birthday;
            DateTime today = DateTime.Today;
            Age = today.Year - birthday.Year;
            if (Birthday.Date > today.AddYears(-Age).Date)
                --Age;
            if (Age > MaxAge || Age < MinAge || birthday.Date > today.Date)
                throw new IncorrectBirthday(birthday.Date, MinAge, MaxAge);
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

        #endregion

        internal string GetGreeting()
        {
            return $"Happy birthday, {Name} {Surname}!\nCheckout your mail {Email} :)";
        }
    }
}