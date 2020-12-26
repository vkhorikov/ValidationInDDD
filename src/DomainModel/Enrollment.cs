namespace DomainModel
{
    public class Enrollment
    {
        public Student Student { get; }
        public Course Course { get; }
        public Grade Grade { get; }
        
        public Enrollment(Student student, Course course, Grade grade)
        {
            Student = student;
            Course = course;
            Grade = grade;
        }
    }
}
