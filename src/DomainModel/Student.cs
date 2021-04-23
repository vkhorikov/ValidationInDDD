using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace DomainModel
{
    public class Student : Entity
    {
        public Email Email { get; }
        public string Name { get; private set; }
        public Address[] Addresses { get; private set; }

        private readonly List<Enrollment> _enrollments = new List<Enrollment>();
        public virtual IReadOnlyList<Enrollment> Enrollments => _enrollments.ToList();

        protected Student()
        {
        }

        public Student(Email email, string name, Address[] addresses)
            : this()
        {
            Email = email;
            EditPersonalInfo(name, addresses);
        }

        public void EditPersonalInfo(string name, Address[] addresses)
        {
            Name = name;
            Addresses = addresses;
        }

        public virtual void Enroll(Course course, Grade grade)
        {
            if (_enrollments.Count >= 2)
                throw new Exception("Cannot have more than 2 enrollments");
            
            if (_enrollments.Any(x => x.Course == course))
                throw new Exception($"Student '{Name}' already enrolled into course '{course.Name}'");

            var enrollment = new Enrollment(this, course, grade);
            _enrollments.Add(enrollment);
        }
    }

    public class Address : Entity
    {
        public string Street { get; }
        public string City { get; }
        public State State { get; }
        public string ZipCode { get; }

        private Address(string street, string city, State state, string zipCode)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public static Result<Address> Create(
            string street, string city, string state, string zipCode)
        {
            State stateObject = State.Create(state).Value;

            street = (street ?? "").Trim();
            city = (city ?? "").Trim();
            zipCode = (zipCode ?? "").Trim();

            if (street.Length < 1 || street.Length > 100)
                return Result.Failure<Address>("Invalid street length");

            if (city.Length < 1 || city.Length > 40)
                return Result.Failure<Address>("Invalid city length");
            
            if (zipCode.Length < 1 || zipCode.Length > 5)
                return Result.Failure<Address>("Invalid zip code length");

            return new Address(street, city, stateObject, zipCode);
        }
    }

    public class State : ValueObject
    {
        public static readonly State VA = new State("VA");
        public static readonly State DC = new State("DC");
        public static readonly State[] All = { VA, DC };

        public string Value { get; }

        private State(string value)
        {
            Value = value;
        }

        public static Result<State> Create(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Result.Failure<State>("Value is required");

            string name = input.Trim();

            if (name.Length > 2)
                return Result.Failure<State>("Value is too long");

            if (All.Any(x => x.Value == name.ToUpper()) == false)
                return Result.Failure<State>("State is invalid");

            return Result.Success(new State(name));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
