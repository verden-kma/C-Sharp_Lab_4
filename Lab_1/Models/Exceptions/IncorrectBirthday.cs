using System;

namespace Lab_1.Models.Exceptions
{
    internal class IncorrectBirthday : ArgumentException
    {
        private readonly DateTime _wrongDate;
        private readonly uint _minAge;
        private readonly uint _maxAge;
        internal IncorrectBirthday(DateTime wrongDate, uint min, uint max) 
        {
            _wrongDate = wrongDate;
            _minAge = min;
            _maxAge = max;
        }

        public override string Message => $"Person must be between {_minAge} and {_maxAge} y.o., but date passed is {_wrongDate.Date.ToShortDateString()}";
    }
}