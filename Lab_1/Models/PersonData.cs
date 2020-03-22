using System;

namespace Lab_1.Models
{
    [Serializable]
    internal struct PersonData
    {
        internal string Name;
        internal string Surname;
        internal string Email;
        internal DateTime Birthday;
        internal const string DefaultEmail = "yablonskyivr@ukma.edu.ua";
        internal static readonly DateTime DefaultBirthday = new DateTime(1995, 5, 12);
    }
}