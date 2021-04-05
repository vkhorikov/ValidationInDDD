using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace DomainModel
{
    public class StudentName : ValueObject
    {
        public string Value { get; }

        public StudentName(string value)
        {
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
