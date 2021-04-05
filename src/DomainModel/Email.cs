using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace DomainModel
{
    public class Email : ValueObject
    {
        public string Value { get; }

        public Email(string value)
        {
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
