using System;

namespace Lab_1.Models.Exceptions
{
    internal class IrrelevantName : ArgumentException
    {
        private readonly uint _minLen;
        private readonly uint _maxLen;
        private readonly string _wrongName;
        internal IrrelevantName(string passedName, uint minLen = 2, uint maxLen = 20)
        {
            _wrongName = passedName;
            _minLen = minLen;
            _maxLen = maxLen;
        }

        public override string Message =>
            string.IsNullOrWhiteSpace(_wrongName)
                ? "Name is null or whitespace."
                : $"Allowed length of \"{_wrongName}\" is [{_minLen};{_maxLen}], passed name is {_wrongName.Length} chars long.";
    }
}