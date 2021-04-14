using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace DomainModel
{
    public class StudentName : ValueObject
    {
        public string Value { get; }

        private StudentName(string value)
        {
            Value = value;
        }

        public static Result<StudentName> Create(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Result.Failure<StudentName>("Value is required");

            string name = input.Trim();

            if (name.Length > 200)
                return Result.Failure<StudentName>("Value is too long");

            return Result.Success(new StudentName(name));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
