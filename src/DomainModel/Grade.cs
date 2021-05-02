using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace DomainModel
{
    public class Grade : ValueObject
    {
        public static Grade A = new Grade(nameof(A));
        public static Grade B = new Grade(nameof(B));
        public static Grade C = new Grade(nameof(C));
        public static Grade D = new Grade(nameof(D));
        public static Grade F = new Grade(nameof(F));

        private static readonly Grade[] _all = { A, B, C, D, F };

        public string Value { get; }

        private Grade(string value)
        {
            Value = value;
        }

        public static Result<Grade, Error> Create(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Errors.General.ValueIsRequired();

            string grade = input.Trim().ToUpper();

            if (grade.Length > 1)
                return Errors.General.InvalidLength();

            if (_all.Any(x => x.Value == grade) == false)
                return Errors.General.ValueIsInvalid();

            return new Grade(grade);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }

    //public enum Grade
    //{
    //    A = 1,
    //    B = 2,
    //    C = 3,
    //    D = 4,
    //    F = 5
    //}
}
