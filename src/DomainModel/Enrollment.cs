using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace DomainModel
{
    public class StudentEnrollment : Entity
    {
        public Student Student { get; }
        public Enrollment Enrollment { get; }

        public StudentEnrollment(Student student, Enrollment enrollment)
        {
            Student = student;
            Enrollment = enrollment;
        }
    }


    public class Enrollment : ValueObject
    {
        public Course Course { get; }
        public Grade Grade { get; }

        public Enrollment(Course course, Grade grade)
        {
            Course = course;
            Grade = grade;
        }

        public static Result<Enrollment[], Error> Create((string course, string grade)[] input, Course[] allCourses)
        {
            var result = new List<Enrollment>();

            foreach ((string courseName, string gradeName) in input)
            {
                Grade grade = Grade.Create(gradeName).Value;

                Course course = allCourses.SingleOrDefault(x => x.Name == courseName.Trim());
                if (course == null)
                    return Errors.Student.CourseIsInvalid();

                result.Add(new Enrollment(course, grade));
            }

            return result.ToArray();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Course;
        }
    }
}
