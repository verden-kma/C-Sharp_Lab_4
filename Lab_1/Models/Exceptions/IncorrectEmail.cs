using System;

namespace Lab_1.Models.Exceptions
{
    internal class IncorrectEmail : ArgumentException
    {
        private readonly string _badEmail;
        private readonly string _pattern;
        /*
         * Invalid email address.
         */
        internal IncorrectEmail(string badEmail, string pattern)
        {
            _badEmail = badEmail;
            _pattern = pattern;
        }

        public override string Message =>
            string.IsNullOrWhiteSpace(_badEmail)
                ? "email address is empty"
                : $"\"{_badEmail}\" does not match pattern {_pattern}";
    }
}