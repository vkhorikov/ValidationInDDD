using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace DomainModel
{
    public class Email : ValueObject
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email, Error> Create(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Errors.General.ValueIsRequired();

            string email = input.Trim();

            if (email.Length > 150)
                return Errors.General.InvalidLength();

            if (Regex.IsMatch(email, @"^(.+)@(.+)$") == false)
                return Errors.General.ValueIsInvalid();

            return new Email(email);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
