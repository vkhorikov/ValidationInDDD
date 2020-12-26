namespace DomainModel
{
    public class Course : Entity
    {
        public string Name { get; }
        public int Credits { get; }

        public Course(long id, string name, int credits)
        {
            Id = id;
            Name = name;
            Credits = credits;
        }
    }
}
