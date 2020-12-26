using System.Linq;
using DomainModel;

namespace Api
{
    public sealed class CourseRepository
    {
        private static readonly Course[] _allCourses =
        {
            new Course(1, "Calculus", 5),
            new Course(2, "History", 4),
            new Course(3, "Literature", 4)
        };

        public Course GetByName(string name)
        {
            return _allCourses.SingleOrDefault(x => x.Name == name);
        }
    }
}
