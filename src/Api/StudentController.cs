using System;
using System.Linq;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using DomainModel;
using FluentValidation.Results;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
    [Route("api/students")]
    public class StudentController : ApplicationController
    {
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;
        private readonly StateRepository _stateRepository;

        public StudentController(
            StudentRepository studentRepository, CourseRepository courseRepository, StateRepository stateRepository)
        {
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;
            _stateRepository = stateRepository;
        }

        [HttpPost]
        public IActionResult Register(RegisterRequest request)
        {
            Address[] addresses = request.Addresses
                .Select(x => Address.Create(x.Street, x.City, x.State, x.ZipCode, _stateRepository.GetAll()).Value)
                .ToArray();

            Email email = Email.Create(request.Email).Value;
            string name = request.Name.Trim();

            Student existingStudent = _studentRepository.GetByEmail(email);
            if (existingStudent != null)
                return Error(Errors.Student.EmailIsTaken());

            var student = new Student(email, name, addresses);
            _studentRepository.Save(student);

            var response = new RegisterResponse
            {
                Id = student.Id
            };
            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult EditPersonalInfo(long id, EditPersonalInfoRequest request)
        {
            Student student = _studentRepository.GetById(id);
            if (student == null)
                return Error(Errors.General.NotFound(), nameof(id));

            Address[] addresses = request.Addresses
                .Select(x => Address.Create(x.Street, x.City, x.State, x.ZipCode, _stateRepository.GetAll()).Value)
                .ToArray();
            string name = request.Name.Trim();
            
            student.EditPersonalInfo(name, addresses);
            _studentRepository.Save(student);

            return Ok();
        }

        [HttpPost("{id}/enrollments")]
        public IActionResult Enroll(long id, EnrollRequest request)
        {
            Student student = _studentRepository.GetById(id);
            if (student == null)
                return Error(Errors.General.NotFound(), nameof(id));

            (string Course, string Grade)[] input = request.Enrollments
                .Select(x => (x.Course, x.Grade))
                .ToArray();
            Course[] allCourses = _courseRepository.GetAll();

            Result<Enrollment[], Error> enrollmentsOrError = Enrollment.Create(input, allCourses);
            if (enrollmentsOrError.IsFailure)
                return Error(enrollmentsOrError.Error);

            Result<object, Error> result = student.Enroll(enrollmentsOrError.Value);
            if (result.IsFailure)
                return Error(result.Error);

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            Student student = _studentRepository.GetById(id);

            var resonse = new GetResonse
            {
                Addresses = student.Addresses.Select(x =>
                    new AddressDto
                    {
                        Street = x.Street,
                        City = x.City,
                        State = x.State.Value,
                        ZipCode = x.ZipCode
                    })
                    .ToArray(),
                Email = student.Email.Value,
                Name = student.Name,
                Enrollments = student.Enrollments.Select(x => new CourseEnrollmentDto
                {
                    Course = x.Enrollment.Course.Name,
                    Grade = x.Enrollment.Grade.ToString()
                }).ToArray()
            };
            return Ok(resonse);
        }
    }
}
