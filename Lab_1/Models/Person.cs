using System;

namespace Lab_1.Models
{
    public class Person
    {
        private DateTime _birthday;

        public Person(DateTime birthday)
        {
            _birthday = birthday;
            DateTime today = DateTime.Today;
            Age = today.Year - birthday.Year;
            if (_birthday.Date > today.AddYears(-Age).Date) 
                --Age;
            if (Age > 135 || Age < 0 || birthday.Date > today.Date)
                throw new ArgumentException("incorrect birthday date");
            _birthday = birthday;
            Wz = Zodiac.GetWestZodiac(birthday);
            Cz = Zodiac.GetChineseZodiac(birthday);
        }

        public bool IsBirthday()
        {
            return _birthday.Date.Month.Equals(DateTime.Today.Month) && _birthday.Date.Day.Equals(DateTime.Today.Day);
        }

        public static string GetGreeting()
        {
            return "Happy birthday!";
        }

        public int Age { get; }

        public Zodiac.WesternZodiac Wz { get; }

        public Zodiac.ChineseZodiac Cz { get; }
    }
}