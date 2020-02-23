using System;

namespace Lab_1.Models
{
    public static class Zodiac
    {
        public static WesternZodiac GetWestZodiac(DateTime dt)
        {
            switch (dt.Month)
            {
                case 3 when dt.Day >= 21:
                case 4 when dt.Day <= 20:
                    return WesternZodiac.Aries;
                case 4 when dt.Day >= 21:
                case 5 when dt.Day <= 21:
                    return WesternZodiac.Taurus;
                case 5 when dt.Day >= 22:
                case 6 when dt.Day <= 21:
                    return WesternZodiac.Gemini;
                case 6 when dt.Day >= 22:
                case 7 when dt.Day <= 22:
                    return WesternZodiac.Cancer;
                case 7 when dt.Day >= 23:
                case 8 when dt.Day <= 23:
                    return WesternZodiac.Leo;
                case 8 when dt.Day >= 24:
                case 9 when dt.Day <= 22:
                    return WesternZodiac.Virgo;
                case 9 when dt.Day >= 23:
                case 10 when dt.Day <= 23:
                    return WesternZodiac.Libra;
                case 10 when dt.Day >= 24:
                case 11 when dt.Day <= 22:
                    return WesternZodiac.Scorpio;
                case 11 when dt.Day >= 23:
                case 12 when dt.Day <= 21:
                    return WesternZodiac.Sagittarius;
                case 12 when dt.Day >= 22:
                case 1 when dt.Day <= 20:
                    return WesternZodiac.Capricorn;
                case 1 when dt.Day >= 21:
                case 2 when dt.Day <= 18:
                    return WesternZodiac.Aquarius;
                default:
                    return WesternZodiac.Pisces;
            }
        }

        public static ChineseZodiac GetChineseZodiac(DateTime dt)
        {
            return (ChineseZodiac) (dt.Year % 12);
        }
        
        public enum WesternZodiac
        {
            Aries,
            Taurus,
            Gemini,
            Cancer,
            Leo,
            Virgo,
            Libra,
            Scorpio,
            Sagittarius,
            Capricorn,
            Aquarius,
            Pisces
        }
        
        //they are used indeed
        public enum ChineseZodiac
        {
            Monkey = 0,
            Rooster = 1,
            Dog = 2,
            Pig = 3,
            Rat = 4,
            Ox = 5,
            Tiger = 6,
            Rabbit = 7,
            Dragon = 8,
            Snake = 9,
            Horse = 10,
            Goat = 11
        }
    }

}