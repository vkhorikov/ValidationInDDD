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

        public static Result<Address, Error> Create(
            string street, string city, string state, string zipCode, string[] allStates)
        {
            State stateObject = State.Create(state, allStates).Value;

            street = (street ?? "").Trim();
            city = (city ?? "").Trim();
            zipCode = (zipCode ?? "").Trim();

            if (street.Length < 1 || street.Length > 100)
                return Errors.General.InvalidLength("street");

            if (city.Length < 1 || city.Length > 40)
                return Errors.General.InvalidLength("city");

            if (zipCode.Length < 1 || zipCode.Length > 5)
                return Errors.General.InvalidLength("zip code");

            return new Address(street, city, stateObject, zipCode);
        }
    }

    public class State : ValueObject
    {
        public string Value { get; }

        private State(string value)
        {
            Value = value;
        }

        public static Result<State, Error> Create(string input, string[] allStates)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Errors.General.ValueIsRequired();
            
            string name = input.Trim().ToUpper();

            if (name.Length > 2)
                return Errors.General.InvalidLength();

            if (allStates.Any(x => x == name) == false)
                return Errors.Student.InvalidState(name);

            return new State(name);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
