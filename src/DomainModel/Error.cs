using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace DomainModel
{
    public sealed class Error : ValueObject
    {
        public string Code { get; }
        public string Message { get; }

        internal Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }

    public static class Errors
    {
        public static class Student
        {
            public static Error EmailIsTaken(string email) =>
                new Error("student.email.is.taken", $"Student email '{email}' is taken");

            public static Error InvalidState(string name) =>
                new Error("invalid.state", $"Invalid state: '{name}'");
        }

        public static class General
        {
            public static Error NotFound(long? id = null)
            {
                string forId = id == null ? "" : $" for Id '{id}'";
                return new Error("record.not.found", $"Record not found{forId}");
            }

            public static Error ValueIsInvalid() =>
                new Error("value.is.invalid", "Value is invalid");

            public static Error ValueIsRequired() =>
                new Error("value.is.required", "Value is required");

            public static Error InvalidLength(string name = null)
            {
                string label = name == null ? " " : " " + name + " ";
                return new Error("invalid.string.length", $"Invalid{label}length");
            }
        }
    }
}
